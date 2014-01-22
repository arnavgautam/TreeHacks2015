namespace EventBuddy.Validation
{
    public interface IValidator
    {
        void Validate(object value);

        bool IsValid();
    }
}