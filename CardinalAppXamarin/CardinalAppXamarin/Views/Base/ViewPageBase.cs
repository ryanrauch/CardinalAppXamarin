using Autofac;
using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Views.Base
{
    public class ViewPageBase<T> : ContentPage where T : ViewModelBase
    {
        readonly T _viewModel;
        public T ViewModel
        {
            get { return _viewModel; }
        }

        public ViewPageBase()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                _viewModel = App.Container.Resolve<T>();
            }
            BindingContext = _viewModel;
            AttachEventHandlers();
        }

        private void AttachEventHandlers()
        {
            Appearing += (sender, e) =>
            {
                if (BindingContext is ViewModelBase viewModelBase)
                {
                    viewModelBase.OnAppearing();
                }
            };

            Disappearing += (sender, e) =>
            {
                if (BindingContext is ViewModelBase viewModelBase)
                {
                    viewModelBase.OnDisappearing();
                }
            };
        }
    }
}
