namespace QrCodeGenerator.Model;

public class VCardData : IQrCodeData
{
  public string Name { get; set; } = string.Empty;
  public string Org { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Phone { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Url { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;

  public string ToEncodedString() =>
      $"MECARD:N:{Name};ORG:{Org};TITLE:{Title};TEL:{Phone};EMAIL:{Email};URL:{Url};ADR:{Address};;";
}