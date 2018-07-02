using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class HexagonButtonView : View
    {
        public static readonly BindableProperty RadiusProperty = BindableProperty.Create(
                nameof(Radius),
                typeof(double),
                typeof(HexagonButtonView),
                10.0);
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly BindableProperty PointyTopProperty = BindableProperty.Create(
                nameof(PointyTop),
                typeof(bool),
                typeof(HexagonButtonView),
                true);
        public bool PointyTop
        {
            get { return (bool)GetValue(PointyTopProperty); }
            set { SetValue(PointyTopProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(HexagonButtonView),
                string.Empty);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
                            BindableProperty.Create(nameof(TextColor),
                                                    typeof(Color),
                                                    typeof(HexagonButtonView),
                                                    Color.Black);
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty =
                    BindableProperty.Create(nameof(FontSize),
                                            typeof(float),
                                            typeof(HexagonButtonView),
                                            12f);
        public float FontSize
        {
            get { return (float)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily),
                                    typeof(string),
                                    typeof(HexagonButtonView),
                                    "highlandgothiclightflf");
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly BindableProperty FATextProperty = BindableProperty.Create(
                nameof(FAText),
                typeof(string),
                typeof(HexagonButtonView),
                string.Empty);

        public string FAText
        {
            get { return (string)GetValue(FATextProperty); }
            set { SetValue(FATextProperty, value); }
        }

        public static readonly BindableProperty FAFontFamilyProperty =
                BindableProperty.Create(nameof(FAFontFamily),
                                        typeof(string),
                                        typeof(HexagonButtonView),
                                        "FontAwesome5FreeSolid");
        public string FAFontFamily
        {
            get { return (string)GetValue(FAFontFamilyProperty); }
            set { SetValue(FAFontFamilyProperty, value); }
        }

        public static readonly BindableProperty FAFontSizeProperty =
            BindableProperty.Create(nameof(FAFontSize),
                                    typeof(float),
                                    typeof(HexagonButtonView),
                                    24f);
        public float FAFontSize
        {
            get { return (float)GetValue(FAFontSizeProperty); }
            set { SetValue(FAFontSizeProperty, value); }
        }
    }
}
