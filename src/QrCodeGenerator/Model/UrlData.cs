namespace QrCodeGenerator.Model;

public class UrlData : IQrCodeData
{
  public string Url { get; set; } = string.Empty;

  public string ToEncodedString() => Url;
}
