namespace QrCodeGenerator.Model;
public class EmailData : IQrCodeData
{
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public string ToEncodedString() => $"MATMSG:TO:{Email};SUB:{Subject};BODY:{Body};;";
}