using System.Threading.Tasks;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IRequestService
    {
        Task<TResult> GetAsync<TResult>(string uri);
        Task<TResult> PostAsync<TResult>(string uri, TResult data);
        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, bool auth = true); //special-case for login when auth==false
        Task<TResult> PutAsync<TResult>(string uri, TResult data);
        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data);
    }
}
