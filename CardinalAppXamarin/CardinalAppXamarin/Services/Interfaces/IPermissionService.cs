using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<PermissionStatus> CheckAndRequestPermissionAsync(Permission permission);
        Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission);
        Task<PermissionStatus> RequestPermissionAsync(Permission permission);
    }
}