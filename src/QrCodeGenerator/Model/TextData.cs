namespace QrCodeGenerator.Model;

public class TextData : IQrCodeData
{
  public string Text { get; set; } = string.Empty;

  public string ToEncodedString() => Text;
}
