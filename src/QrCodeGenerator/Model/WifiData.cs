namespace QrCodeGenerator.Model;

public class WifiData : IQrCodeData
{
  public string SSID { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string Encryption { get; set; } = "WPA";

  public string ToEncodedString() =>
      $"WIFI:T:{Encryption};S:{SSID};P:{Password};;";
}