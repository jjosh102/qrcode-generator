namespace QrCodeGenerator.Model;

public class PhoneData : IQrCodeData
{
    public string Number { get; set; } = string.Empty;

    public string ToEncodedString() => $"TEL:{Number}";
}