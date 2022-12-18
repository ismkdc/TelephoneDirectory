﻿namespace TelephoneDirectory.Infrastructure.Errors;

public class TelephoneDirectoryException : Exception
{
    public TelephoneDirectoryException(CustomError customError)
    {
        CustomError = customError;
    }

    public CustomError CustomError { get; set; }
}