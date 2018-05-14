using CardinalAppXamarin.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Services
{
    public class HeatGradientService : IHeatGradientService
    {
        public List<Color> Colors { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        private readonly int _alpha = 92;

        public HeatGradientService()
        {
            Min = 0;
            Max = 16;
            Colors = new List<Color>();
            Colors.Add(Color.Transparent);
            Colors.Add(Color.FromRgba(0, 225, 130, _alpha));
            Colors.Add(Color.FromRgba(0, 195, 83, _alpha));
            Colors.Add(Color.FromRgba(0, 165, 70, _alpha));
            Colors.Add(Color.FromRgba(0, 135, 55, _alpha));
            Colors.Add(Color.FromRgba(0, 110, 45, _alpha));
            Colors.Add(Color.FromRgba(0, 95, 40, _alpha));
            Colors.Add(Color.FromRgba(255, 235, 0, _alpha));
            Colors.Add(Color.FromRgba(248, 181, 3, _alpha));
            Colors.Add(Color.FromRgba(255, 137, 41, _alpha));
            Colors.Add(Color.FromRgba(244, 98, 0, _alpha));
            Colors.Add(Color.FromRgba(232, 0, 0, _alpha));
            Colors.Add(Color.FromRgba(177, 0, 0, _alpha));
            Colors.Add(Color.FromRgba(234, 0, 138, _alpha));
            Colors.Add(Color.FromRgba(169, 0, 204, _alpha));
            Colors.Add(Color.FromRgba(121, 0, 123, _alpha));
            Colors.Add(Color.FromRgba(99, 0, 99, _alpha));
        }
        public HeatGradientService(List<Color> colors)
        {
            Min = 0;
            colors.Insert(0, Color.Transparent);
            Max = colors.Count;
            Colors = colors;
        }
        public Color SteppedColor(int step)
        {
            if (step <= Min)
                return Colors[Min];
            if (step >= Max)
                return Colors[Max];
            return Colors[step];
        }
    }
}
