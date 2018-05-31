using System;

namespace FlyoverKit.iOS
{
    public class FlyoverCameraConfiguration : IEquatable<FlyoverCameraConfiguration>
    {
        /// <summary>
        /// The duration
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// The altitude above the ground, measured in meters
        /// </summary>
        public double Altitude { get; set; }

        /// <summary>
        /// The viewing angle of the camera, measured in degrees
        /// </summary>
        public double Pitch { get; set; }

        /// <summary>
        /// The heading step
        /// </summary>
        public double HeadingStep { get; set; }

        /// <summary>
        /// The region change animation
        /// </summary>
        public RegionChangeAnimation RegionChangeAnimation { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="duration">The duration</param>
        /// <param name="altitude">The altitude</param>
        /// <param name="pitch">The pitch</param>
        /// <param name="headingStep">The heading step</param>
        /// <param name="regionChangeAnimation">The region change animation</param>
        public FlyoverCameraConfiguration(
            double duration,
            double altitude,
            double pitch,
            double headingStep,
            RegionChangeAnimation regionChangeAnimation = RegionChangeAnimation.None)
        {
            Duration = duration;
            Altitude = altitude;
            Pitch = pitch;
            HeadingStep = headingStep;
            RegionChangeAnimation = regionChangeAnimation;
        }

        /// <summary>
        /// Construct via ConfigurationTheme
        /// </summary>
        /// <param name="theme"></param>
        public FlyoverCameraConfiguration(FlyoverCameraConfigurationTheme theme)
        {
            switch (theme)
            {
                case FlyoverCameraConfigurationTheme.LowFlying:
                    Duration = 4.0;
                    Altitude = 600;
                    Pitch = 45.0;
                    HeadingStep = 20;
                    break;
                case FlyoverCameraConfigurationTheme.FarAway:
                    Duration = 4.0;
                    Altitude = 1330;
                    Pitch = 55;
                    HeadingStep = 20;
                    break;
                case FlyoverCameraConfigurationTheme.Giddy:
                    Duration = 0.0;
                    Altitude = 250;
                    Pitch = 80;
                    HeadingStep = 50;
                    break;
                case FlyoverCameraConfigurationTheme.AstronautView:
                    Duration = 20.0;
                    Altitude = 2000;
                    Pitch = 100.0;
                    HeadingStep = 35.0;
                    break;
                case FlyoverCameraConfigurationTheme.Default:
                default:
                    Duration = 4.0;
                    Altitude = 600;
                    Pitch = 45.0;
                    HeadingStep = 20;
                    break;
            }
        }

        public static bool operator ==(FlyoverCameraConfiguration lhs,
                                       FlyoverCameraConfiguration rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            if (ReferenceEquals(rhs, null))
            {
                return ReferenceEquals(lhs, null);
            }
            return lhs.Duration.Equals(rhs.Duration)
                && lhs.Altitude.Equals(rhs.Altitude)
                && lhs.Pitch.Equals(rhs.Pitch)
                && lhs.HeadingStep.Equals(rhs.HeadingStep);
        }

        public static bool operator !=(FlyoverCameraConfiguration lhs,
                                       FlyoverCameraConfiguration rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var f = (FlyoverCameraConfiguration)obj;
            return Duration == f.Duration
                && Altitude == f.Altitude
                && Pitch == f.Pitch
                && HeadingStep == f.HeadingStep;
        }

        public bool Equals(FlyoverCameraConfiguration f)
        {
            return Duration == f.Duration
                && Altitude == f.Altitude
                && Pitch == f.Pitch
                && HeadingStep == f.HeadingStep;
        }

        public override int GetHashCode()
        {
            return Duration.GetHashCode()
                 ^ Altitude.GetHashCode()
                 ^ Pitch.GetHashCode()
                 ^ HeadingStep.GetHashCode();
        }
    }
}