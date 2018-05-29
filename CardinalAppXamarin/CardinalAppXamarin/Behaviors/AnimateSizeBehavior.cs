using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Behaviors
{
    public class AnimateSizeBehavior : Behavior<View>
    {
        public static readonly BindableProperty EasingFunctionProperty =
                                BindableProperty.Create(nameof(EasingFunctionName), 
                                                        typeof(string), 
                                                        typeof(AnimateSizeBehavior),
                                                        "SinIn",
                                                        propertyChanged: OnEasingFunctionChanged);

        public static readonly BindableProperty ScaleProperty =
                                BindableProperty.Create(nameof(Scale),
                                                        typeof(double),
                                                        typeof(AnimateSizeBehavior),
                                                        1.25);

        private Easing _easingFunction;

        public string EasingFunctionName
        {
            get { return (string)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Focused += OnItemFocused;
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Focused -= OnItemFocused;
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

        private static void OnEasingFunctionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as AnimateSizeBehavior).EasingFunctionName = (string)newValue;
            (bindable as AnimateSizeBehavior)._easingFunction = GetEasing((string)newValue);
        }

        private async void OnItemFocused(object sender, FocusEventArgs e)
        {
            await ((View)sender).ScaleTo(Scale, 250, _easingFunction);
            await((View)sender).ScaleTo(1.00, 250, _easingFunction);
        }
    }
}
