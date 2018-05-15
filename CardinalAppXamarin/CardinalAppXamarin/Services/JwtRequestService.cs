using CardinalAppXamarin.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services
{
    public class JwtRequestService : IRequestService
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly HttpClient _client;
        private readonly String _endPoint;

        private bool _initialized { get; set; }
        private String _jwt { get; set; }

        public JwtRequestService()
        {
            _initialized = false;
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
            _client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
            _endPoint = Constants.CardinalWebApiBase;
        }

        public async Task<Boolean> PostAuthenticationRequestAsync(string username, string password, bool persistent)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("username", username));
            parameters.Add(new KeyValuePair<string, string>("password", password));
            parameters.Add(new KeyValuePair<string, string>("persistent", persistent.ToString()));

            var content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await _client.PostAsync(_endPoint + "api/Token", content);
            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                SetJsonWebToken(token);
                _initialized = true;
                return true;
            }
            return false;
        }

        public void SetJsonWebToken(String jsonWebToken)
        {
            var trim = new char[] { '\"' };
            _jwt = jsonWebToken.TrimStart(trim).TrimEnd(trim);
        }

        private void CheckInitialization()
        {
            if (!_initialized)
                throw new InvalidOperationException("JwtRequestService has not been initialized");

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _jwt);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            CheckInitialization();
            if (!String.IsNullOrEmpty(_endPoint))
            {
                uri = _endPoint + uri;
            }
            HttpResponseMessage response = await _client.GetAsync(uri);
            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
            return result;
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data)
        {
            return PostAsync<TResult, TResult>(uri, data);
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data)
        {
            CheckInitialization();
            if (!String.IsNullOrEmpty(_endPoint))
            {
                uri = _endPoint + uri;
            }
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            HttpResponseMessage response = await _client.PostAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));
            await HandleResponse(response);
            string responseData = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data)
        {
            return PutAsync<TResult, TResult>(uri, data);
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data)
        {
            CheckInitialization();
            if (!String.IsNullOrEmpty(_endPoint))
            {
                uri = _endPoint + uri;
            }
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            HttpResponseMessage response = await _client.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));
            await HandleResponse(response);
            string responseData = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException(content);
                }
                throw new HttpRequestException(content);
            }
        }
    }
}