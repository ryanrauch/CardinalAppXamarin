using System.Windows.Input;
using Xamarin.Forms;

namespace CardinalAppXamarin.Behaviors
{
    public class ValidateOnDateSelectedBehavior : Behavior<DatePicker>
    {
        private VisualElement _element;

        public static readonly BindableProperty ValidateCommandProperty =
                BindableProperty.Create("ValidateCommand", typeof(ICommand),
                    typeof(ValidateOnDateSelectedBehavior), default(ICommand),
                    BindingMode.OneWay, null);

        public ICommand ValidateCommand
        {
            get { return (ICommand)GetValue(ValidateCommandProperty); }
            set { SetValue(ValidateCommandProperty, value); }
        }

        protected override void OnAttachedTo(DatePicker bindable)
        {
            _element = bindable;
            bindable.DateSelected += Bindable_DateSelected;
            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        private void Bindable_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (ValidateCommand != null && ValidateCommand.CanExecute(null))
            {
                ValidateCommand.Execute(null);
            }
        }

        protected override void OnDetachingFrom(DatePicker bindable)
        {
            _element = null;
            BindingContext = null;
            bindable.DateSelected -= Bindable_DateSelected;
            bindable.BindingContextChanged -= OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, System.EventArgs e)
        {
            BindingContext = _element?.BindingContext;
        }
    }
}
