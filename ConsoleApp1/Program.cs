using System;
using OtpNet;
using QRCoder;
using System.Drawing;

class Program
{
    // Kullanıcı için benzersiz bir anahtar oluşturur ve döndürür.
    private static byte[] GenerateSecretKey()
    {
        return KeyGeneration.GenerateRandomKey(20);
    }

    // Verilen gizli anahtar kullanılarak TOTP üretir.
    private static string GenerateTotp(byte[] secretKey)
    {
        var totp = new Totp(secretKey);
        return totp.ComputeTotp();
    }

    // Kullanıcıdan alınan TOTP'yi doğrular.
    private static bool ValidateTotp(string userEnteredTotp, byte[] secretKey)
    {
        var totp = new Totp(secretKey);
        // VerificationWindow parametresi, küçük zaman farklılıklarına tolerans sağlar.
        return totp.VerifyTotp(userEnteredTotp, out long timeStepMatched, new VerificationWindow(2, 2));
    }
    public static Bitmap GenerateQrCode(string text)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeImage = qrCode.GetGraphic(20);
        return qrCodeImage;
    }
    static void Main(string[] args)
    {
        // Kullanıcı için bir anahtar oluştur
        byte[] secretKey = GenerateSecretKey();
        string base32String = Base32Encoding.ToString(secretKey);
        Console.WriteLine($"Your Secret Key is: {base32String}");
        string issuer = "ExampleCorp";
        string userIdentifier = "MAIL_HERE";
        string provisionUri = $"otpauth://totp/{issuer}:{userIdentifier}?secret={base32String}&issuer={issuer}";

        Bitmap qrCodeImage = GenerateQrCode(provisionUri);
        Console.WriteLine($"Your Secret Key is: {(qrCodeImage.GetType)}");
        qrCodeImage.Save("QRCode.png");
        // TOTP üret ve ekrana yazdır
        string currentTotp = GenerateTotp(secretKey);
        Console.WriteLine($"Your TOTP is: {currentTotp}");

        // Kullanıcıdan TOTP girişi al
        Console.Write("Please enter the TOTP to verify: ");
        string userInput = Console.ReadLine();


        // TOTP doğrulama
        if (ValidateTotp(userInput, secretKey))
        {
            Console.WriteLine("Success: The TOTP is valid.");
        } 
        else
        {
            Console.WriteLine("Error: Invalid TOTP.");
        }
    }
}