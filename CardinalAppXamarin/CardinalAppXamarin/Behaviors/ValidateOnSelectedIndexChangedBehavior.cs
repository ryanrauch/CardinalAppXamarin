using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace CardinalAppXamarin.Behaviors
{
    public class ValidateOnSelectedIndexChangedBehavior : Behavior<Picker>
    {
        private VisualElement _element;

        public static readonly BindableProperty ValidateCommandProperty =
                BindableProperty.Create("ValidateCommand", typeof(ICommand),
                    typeof(ValidateOnSelectedIndexChangedBehavior), default(ICommand),
                    BindingMode.OneWay, null);

        public ICommand ValidateCommand
        {
            get { return (ICommand)GetValue(ValidateCommandProperty); }
            set { SetValue(ValidateCommandProperty, value); }
        }

        protected override void OnAttachedTo(Picker bindable)
        {
            _element = bindable;
            bindable.SelectedIndexChanged += Bindable_SelectedIndexChanged;
            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        private void Bindable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValidateCommand != null && ValidateCommand.CanExecute(null))
            {
                ValidateCommand.Execute(null);
            }
        }

        protected override void OnDetachingFrom(Picker bindable)
        {
            _element = null;
            BindingContext = null;
            bindable.SelectedIndexChanged -= Bindable_SelectedIndexChanged;
            bindable.BindingContextChanged -= OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            BindingContext = _element?.BindingContext;
        }
    }
}
