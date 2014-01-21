namespace EventBuddy.Validation
{
    using System;

    public class DateValidator : IValidator
    {
        private bool isValid;

        public void Validate(object value)
        {
            var text = value as string;

            DateTime date;
            this.isValid = !string.IsNullOrWhiteSpace(text) && DateTime.TryParse(text, out date);
        }

        public bool IsValid()
        {
            return this.isValid;
        }
    }
}
