﻿namespace TelephoneDirectory.WebAPI.Errors;

public class CustomErrors
{
    public static CustomError E_100 = new(nameof(E_100), "Beklenmedik bir sistem hatası oluştu!");
    
    public static CustomError E_101 = new(nameof(E_101), "Ad bilgisi dolu olmalıdır!");
    public static CustomError E_102 = new(nameof(E_102), "Verilen Id ile kayıt bulunamadı!");
}

public class CustomError
{
    public CustomError(string errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}