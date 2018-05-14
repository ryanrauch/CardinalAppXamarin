using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IHeatGradientService
    {
        List<Color> Colors { get; set; }
        int Max { get; set; }
        int Min { get; set; }
        Color SteppedColor(int step);
    }
}
