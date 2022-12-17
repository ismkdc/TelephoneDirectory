namespace TelephoneDirectory.Infrastructure.Errors;

public static class CustomErrors
{
    public static CustomError E_100 = new(nameof(E_100), "Beklenmedik bir sistem hatası oluştu!");

    public static CustomError E_101 = new(nameof(E_101), "Ad bilgisi dolu olmalıdır!");
    public static CustomError E_102 = new(nameof(E_102), "Verilen Id ile kayıt bulunamadı!");

    public static CustomError E_103 = new(nameof(E_103), "İçerik bilgisi boş bırakılamaz!");
}

public record CustomError(string ErrorCode, string ErrorMessage);