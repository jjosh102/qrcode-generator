namespace QrCodeGenerator.Model;
public class SmsData : IQrCodeData
{
  public string Number { get; set; } = string.Empty;
  public string Message { get; set; } = string.Empty;

  public string ToEncodedString() => $"SMSTO:{Number}:{Message}";
}