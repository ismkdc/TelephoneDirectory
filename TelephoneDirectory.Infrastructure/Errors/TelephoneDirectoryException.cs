namespace TelephoneDirectory.Infrastructure.Errors;

public class TelephoneDirectoryException : Exception
{
    public TelephoneDirectoryException(CustomError customError) : base(customError.ErrorMessage)
    {
        CustomError = customError;
    }

    public CustomError CustomError { get; set; }
}