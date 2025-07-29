namespace QrCodeGenerator.Model;

public class GeoData : IQrCodeData
{
  public string Latitude { get; set; } = string.Empty;
  public string Longitude { get; set; } = string.Empty;

  public string ToEncodedString() => $"geo:{Latitude},{Longitude}";
}