using Microsoft.AspNetCore.Components;
using QrCodeGenerator.Model;
using Microsoft.JSInterop;
using QRCoder;

namespace QrCodeGenerator.Components.Pages;

public partial class Generator : ComponentBase
{
  private readonly QRCodeGenerator _qrCodeGenerator;
  private readonly IJSRuntime _jsRuntime;

  private readonly UrlData urlData = new();
  private readonly SmsData smsData = new();
  private readonly EmailData emailData = new();
  private readonly VCardData vcardData = new();
  private readonly TextData textData = new();
  private readonly WifiData wifiData = new();
  private readonly PhoneData phoneData = new();
  private readonly GeoData geoData = new();

  private QRCodeOption? _selectedOption;
  private string _imageDataUrl = string.Empty;
  private bool _dropdownOpen = false;
  private string _fileName = string.Empty;
  private bool _showFilenameInput = false;
  private string _errorMessage = string.Empty;

  private List<QRCodeOption> _options = [];

  public Generator(QRCodeGenerator qRCodeGenerator, IJSRuntime jsRuntime)
  {
    _qrCodeGenerator = qRCodeGenerator;
    _jsRuntime = jsRuntime;
  }

  protected override void OnInitialized()
  {
    BuildOptions();
    _selectedOption = _options.First();
    GenerateQRCode();
  }

  private void BuildOptions()
  {
    _options =
        [
            new(QRCodeType.Url, "URL", CreateIcon("M13.828 10.172a4 4 0 010 5.656l-1.414 1.414a4 4 0 01-5.656 0l-1.414-1.414a4 4 0 010-5.656M10.172 13.828a4 4 0 005.656 0l1.414-1.414a4 4 0 000-5.656l-1.414-1.414a4 4 0 00-5.656 0")),
            new(QRCodeType.Sms, "SMS", CreateIcon("M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z")),
            new(QRCodeType.Email, "Email", CreateIcon("M4 4h16v16H4V4zm2 4l6 4 6-4")),
            new(QRCodeType.VCard, "VCard", CreateIcon("M4 6h16v12H4V6zm2 2v2h12V8H6zm0 4v2h8v-2H6z")),
            new(QRCodeType.Text, "Text", CreateIcon("M4 6h16M4 10h16M4 14h10")),
            new(QRCodeType.Wifi, "WiFi", CreateIcon("M2.05 8.54a16 16 0 0119.9 0M4.93 11.4a12 12 0 0114.14 0M7.76 14.26a8 8 0 018.48 0M12 18h.01")),
            new(QRCodeType.Phone, "Phone", CreateIcon("M3 5a2 2 0 012-2h3.6a1 1 0 01.97.757l.7 2.8a1 1 0 01-.254.962L8.414 9.414a16 16 0 006.172 6.172l1.895-1.895a1 1 0 01.962-.254l2.8.7a1 1 0 01.757.97V19a2 2 0 01-2 2h-.01C10.603 21 3 13.397 3 4.99V5z")),
            new(QRCodeType.Geo, "Location", CreateIcon("M12 2C8.134 2 5 5.134 5 9c0 6.627 7 13 7 13s7-6.373 7-13c0-3.866-3.134-7-7-7z M12 11a2 2 0 100-4 2 2 0 000 4z"))
        ];
  }

  private RenderFragment CreateIcon(string pathData) => builder =>
  {
    builder.OpenElement(0, "svg");
    builder.AddAttribute(1, "class", "w-4 h-4 text-gray-600");
    builder.AddAttribute(2, "fill", "none");
    builder.AddAttribute(3, "stroke", "currentColor");
    builder.AddAttribute(4, "stroke-width", "2");
    builder.AddAttribute(5, "viewBox", "0 0 24 24");
    builder.OpenElement(6, "path");
    builder.AddAttribute(7, "stroke-linecap", "round");
    builder.AddAttribute(8, "stroke-linejoin", "round");
    builder.AddAttribute(9, "d", pathData);
    builder.CloseElement();
    builder.CloseElement();
  };

  private void GenerateQRCode()
  {
    IQrCodeData? data = _selectedOption?.Type switch
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
    _imageDataUrl = $"data:image/png;base64,{base64Image}";

    Reset();
  }

  private void Reset()
  {
    urlData.Url = string.Empty;
    smsData.Number = smsData.Message = string.Empty;
    emailData.Email = emailData.Subject = emailData.Body = string.Empty;
    vcardData.Name = vcardData.Org = vcardData.Title = vcardData.Phone = vcardData.Email = vcardData.Url = vcardData.Address = string.Empty;
    textData.Text = string.Empty;
    wifiData.SSID = wifiData.Password = string.Empty;
    wifiData.Encryption = "WPA";
    phoneData.Number = string.Empty;
    geoData.Latitude = geoData.Longitude = string.Empty;
  }

  private async Task SaveQRCode()
  {
    _errorMessage = string.Empty;

    if (string.IsNullOrWhiteSpace(_fileName))
    {
      _errorMessage = "Please enter a file name before saving.";
      return;
    }

    if (!string.IsNullOrEmpty(_imageDataUrl))
    {
      var bytes = Convert.FromBase64String(_imageDataUrl.Split(',')[1]);
      await _jsRuntime.InvokeVoidAsync("downloadFileFromBytes", $"{_fileName}.png", "image/png", bytes);
      _showFilenameInput = false; 
    }
  }

  private void SelectType(QRCodeOption option)
  {
    _selectedOption = option;
    _dropdownOpen = false;
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

  private record QRCodeOption(QRCodeType Type, string DisplayName, RenderFragment Icon);
}
