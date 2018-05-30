using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Behaviors
{
    public class RotateBehavior : Behavior<View>
    {
        public static readonly BindableProperty EasingFunctionProperty = BindableProperty.Create(
                                                                            nameof(EasingFunctionName),
                                                                            typeof(string),
                                                                            typeof(RotateBehavior),
                                                                            "SinIn",
                                                                            propertyChanged: OnEasingFunctionChanged);
        public static readonly BindableProperty DegreeProperty =
                        BindableProperty.Create(nameof(Degree),
                                                typeof(double),
                                                typeof(RotateBehavior),
                                                0);

        private static void OnEasingFunctionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as RotateBehavior).EasingFunctionName = (string)newValue;
            (bindable as RotateBehavior)._easingFunction = GetEasing((string)newValue);
        }

        private async void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Visibility"))
            {
                await ((View)sender).RotateTo(Degree, 250, _easingFunction);
                await ((View)sender).RotateTo(0, 250, _easingFunction);
            }
        }

        private Easing _easingFunction;

        public string EasingFunctionName
        {
            get { return (string)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        public double Degree
        {
            get { return (double)GetValue(DegreeProperty); }
            set { SetValue(DegreeProperty, value); }
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.PropertyChanged += OnItemPropertyChanged;
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.PropertyChanged -= OnItemPropertyChanged;
        }

        private static Easing GetEasing(string easingName)
        {
            switch (easingName)
            {
                case "BounceIn": return Easing.BounceIn;
                case "BounceOut": return Easing.BounceOut;
                case "CubicInOut": return Easing.CubicInOut;
                case "CubicOut": return Easing.CubicOut;
                case "Linear": return Easing.Linear;
                case "SinIn": return Easing.SinIn;
                case "SinInOut": return Easing.SinInOut;
                case "SinOut": return Easing.SinOut;
                case "SpringIn": return Easing.SpringIn;
                case "SpringOut": return Easing.SpringOut;
                default: throw new ArgumentException(easingName + " is not valid");
            }
        }
    }
}
