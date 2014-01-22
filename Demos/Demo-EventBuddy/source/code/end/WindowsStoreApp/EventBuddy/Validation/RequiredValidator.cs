namespace EventBuddy.Validation
{
    public class RequiredValidator : IValidator
    {
        private bool isValid;

        public void Validate(object value)
        {
            var text = value as string;
            this.isValid = !string.IsNullOrWhiteSpace(text);
        }

        public bool IsValid()
        {
            return this.isValid;
        }
    }
}

