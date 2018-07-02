using CardinalAppXamarin.Behaviors.Base;
using System;
using System.Linq;
using Xamarin.Forms;

namespace CardinalAppXamarin.Behaviors
{
    public class ViewTappedButtonBehavior : BindableBehavior<View>
    {
        protected override void OnAttachedTo(View bindable)
        {
            var exists = bindable.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;

            if (exists != null)
                exists.Tapped += View_Tapped;

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            var exists = bindable.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;

            if (exists != null)
            {
                exists.Tapped -= View_Tapped;
            }

            base.OnDetachingFrom(bindable);
        }

        bool _isAnimating = false;

        void View_Tapped(object sender, EventArgs e)
        {
            if (_isAnimating)
            {
                return;
            }
            _isAnimating = true;
            var view = (View)sender;
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await view.ScaleTo(0.85d, Constants.ClickFadeDuration / 2, Easing.SinIn);
                    await view.ScaleTo(1d, Constants.ClickFadeDuration / 2, Easing.SinIn);
                }
                finally
                {
                    _isAnimating = false;
                }
            });
        }
    }
}
