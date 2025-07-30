namespace QrCodeGenerator.Model;

public class WifiData : IQrCodeData
{
  public string SSID { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public WifAuth AuthMode { get; set; }
  public bool IsHiddenSSID { get; set; }
  public string ToEncodedString() =>
      $"WIFI:T:{AuthMode};S:{SSID};P:{Password};{(IsHiddenSSID ? "H:true" : string.Empty)};";

  public enum WifAuth { WEP, WPA, nopass, WPA2 }
}