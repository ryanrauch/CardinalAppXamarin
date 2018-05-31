
namespace FlyoverKit.iOS
{
    public static class FlyoverCameraConfigurationExtensions
    {
        public static FlyoverCameraConfiguration GetConfiguration(this FlyoverCameraConfigurationTheme theme)
        {
            switch (theme)
            {
                case FlyoverCameraConfigurationTheme.LowFlying:
                    return new FlyoverCameraConfiguration(4.0, 600, 45.0, 20);
                case FlyoverCameraConfigurationTheme.FarAway:
                    return new FlyoverCameraConfiguration(4.0, 1330, 55, 20);
                case FlyoverCameraConfigurationTheme.Giddy:
                    return new FlyoverCameraConfiguration(0.0, 250, 80, 50);
                case FlyoverCameraConfigurationTheme.AstronautView:
                    return new FlyoverCameraConfiguration(20.0, 2000.0, 100.0, 35.0);
                case FlyoverCameraConfigurationTheme.Default:
                default:
                    return new FlyoverCameraConfiguration(4.0, 600, 45.0, 20.0);
            }
        }
    }
}