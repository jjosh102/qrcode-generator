using Microsoft.AspNetCore.Components;
using QrCodeGenerator.Model;
using Microsoft.JSInterop;
using QRCoder;

namespace QrCodeGenerator.Components.Pages;

public partial class Generator : ComponentBase
{
  private QRCodeGenerator _qrCodeGenerator;
  private IJSRuntime _jsRuntime;

  private readonly UrlData urlData = new();
  private readonly SmsData smsData = new();
  private readonly EmailData emailData = new();
  private readonly VCardData vcardData = new();
  private readonly TextData textData = new();
  private readonly WifiData wifiData = new();
  private readonly PhoneData phoneData = new();
  private readonly GeoData geoData = new();

  private QRCodeType selectedType = QRCodeType.Url;
  private string imageDataUrl = string.Empty;

  public Generator(
    QRCodeGenerator qRCodeGenerator,
    IJSRuntime jsRuntime)
  {
    _qrCodeGenerator = qRCodeGenerator;
    _jsRuntime = jsRuntime;
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
    GenerateQRCode();
  }

  protected override void OnParametersSet()
  {
    base.OnParametersSet();
    GenerateQRCode();
  }

  private void GenerateQRCode()
  {
    IQrCodeData? data = selectedType switch
    {
      QRCodeType.Url => urlData,
      QRCodeType.Sms => smsData,
      QRCodeType.Email => emailData,
      QRCodeType.VCard => vcardData,
      QRCodeType.Text => textData,
      QRCodeType.Wifi => wifiData,
      QRCodeType.Phone => phoneData,
      QRCodeType.Geo => geoData,
      _ => null
    };

    if (data is null) return;

    string encoded = data.ToEncodedString();
    if (string.IsNullOrWhiteSpace(encoded)) return;

    var qrCodeData = _qrCodeGenerator.CreateQrCode(encoded, QRCodeGenerator.ECCLevel.Q);
    var qrCode = new PngByteQRCode(qrCodeData);
    var qrCodeAsPngByteArr = qrCode.GetGraphic(20);
    var base64Image = Convert.ToBase64String(qrCodeAsPngByteArr);
    imageDataUrl = $"data:image/png;base64,{base64Image}";
  }

  private async Task SaveQRCode()
  {
    if (!string.IsNullOrEmpty(imageDataUrl))
    {
      var bytes = Convert.FromBase64String(imageDataUrl.Split(',')[1]);
      await _jsRuntime.InvokeVoidAsync("downloadFileFromBytes", "qrcode.png", "image/png", bytes);
    }
  }


  public enum QRCodeType
  {
    Url,
    Sms,
    Email,
    VCard,
    Text,
    Wifi,
    Phone,
    Geo
  }
}

