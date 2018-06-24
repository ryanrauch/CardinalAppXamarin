using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class HexagonButton : Button
    {
        public static readonly BindableProperty PointedTopProperty =
                            BindableProperty.Create(nameof(PointedTop),
                                                    typeof(bool),
                                                    typeof(HexagonButton),
                                                    true);
        public bool PointedTop
        {
            get { return (bool)GetValue(PointedTopProperty); }
            set { SetValue(PointedTopProperty, value); }
        }
    }
}
