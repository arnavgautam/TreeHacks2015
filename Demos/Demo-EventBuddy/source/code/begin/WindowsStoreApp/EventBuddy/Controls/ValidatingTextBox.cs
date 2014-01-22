namespace EventBuddy.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EventBuddy.Validation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed class ValidatingTextBox : TextBox
    {
        public static DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(ValidatingTextBox), new PropertyMetadata(null));

        public static DependencyProperty ValidatorProperty = DependencyProperty.Register("Validator", typeof(IValidator), typeof(ValidatingTextBox), new PropertyMetadata(null));

        public ValidatingTextBox()
        {
            this.DefaultStyleKey = typeof(ValidatingTextBox);
            this.GotFocus += this.WatermarkTextBoxGotFocus;
            this.LostFocus += this.WatermarkTextBoxLostFocus;
            this.TextChanged += this.OnTextChanged;
        }
        
        public IValidator Validator
        {
            get { return (IValidator)GetValue(ValidatorProperty); }
            set { SetValue(ValidatorProperty, value); }
        }

        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }

        public void Validate()
        {
            this.Validator.Validate(this.Text);
        }

        public bool IsValid()
        {
            return this.Validator.IsValid();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // we need to set the initial state of the watermark
            GoToWatermarkVisualState(false);
        }

        private void WatermarkTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            this.GoToWatermarkVisualState();
        }

        private void WatermarkTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            this.GoToWatermarkVisualState(false);
        }

        private void GoToWatermarkVisualState(bool hasFocus = true)
        {
            this.Validator.Validate(this.Text);

            // if our text is empty and our control doesn't have focus then show the watermark
            // otherwise the control eirther has text or has focus which in either case we need to hide the watermark
            if (!this.Validator.IsValid() && !hasFocus)
                GoToVisualState("ValidationFailed"); 
            else
                GoToVisualState("ValidationSucceded");
        }

        private void GoToVisualState(string stateName, bool useTransitions = true)
        {
            VisualStateManager.GoToState(this, stateName, useTransitions);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            this.GoToWatermarkVisualState();
        }
    }
}
