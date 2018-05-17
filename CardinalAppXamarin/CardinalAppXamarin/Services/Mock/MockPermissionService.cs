using CardinalAppXamarin.Services.Interfaces;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services.Mock
{
    public class MockPermissionService : IPermissionService
    {
        public async Task<PermissionStatus> CheckAndRequestPermissionAsync(Permission permission)
        {
            await Task.Delay(10);
            return PermissionStatus.Granted;
        }

        public async Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission)
        {
            await Task.Delay(10);
            return PermissionStatus.Granted;
        }

        public async Task<PermissionStatus> RequestPermissionAsync(Permission permission)
        {
            await Task.Delay(10);
            return PermissionStatus.Granted;
        }
    }
}
