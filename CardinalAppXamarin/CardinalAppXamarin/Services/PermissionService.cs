using CardinalAppXamarin.Services.Interfaces;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services
{
    public class PermissionService : IPermissionService
    {
        public async Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission)
        {
           return await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
        }

        public async Task<PermissionStatus> RequestPermissionAsync(Permission permission)
        {
            var status = PermissionStatus.Unknown;
            var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
            //Best practice to always check that the key exists
            if (results.ContainsKey(permission))
            {
                status = results[permission];
            }
            return status;
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync(Permission permission)
        {
            var status = await CheckPermissionStatusAsync(permission);
            if(status != PermissionStatus.Granted)
            {
                status = await RequestPermissionAsync(permission);
            }
            return status;
        }
    }
}
