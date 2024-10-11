using RestSharp;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _apiKey;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(IConfiguration config)
    {
        _apiKey = config["BrevoSettings:ApiKey"];
        _senderEmail = config["BrevoSettings:SenderEmail"];
        _senderName = config["BrevoSettings:SenderName"];
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        try
        {
            // Tạo RestClient với endpoint của Brevo API
            var client = new RestClient("https://api.brevo.com/v3/smtp/email");
            var request = new RestRequest("/",Method.Post);

            // Thêm các header cần thiết
            request.AddHeader("accept", "application/json");
            request.AddHeader("api-key", _apiKey);
            request.AddHeader("content-type", "application/json");

            // Tạo body email
            var body = new
            {
                sender = new { email = _senderEmail, name = _senderName },
                to = new[] { new { email = toEmail } },
                subject = subject,
                htmlContent = htmlContent
            };

            // Thêm body vào request
            request.AddJsonBody(body);

            // Thực thi request và chờ phản hồi
            var response = await client.ExecuteAsync(request);

            // Kiểm tra phản hồi
            if (!response.IsSuccessful)
            {
                // Nếu có lỗi, ghi lại log chi tiết
                throw new Exception($"Failed to send email: {response.Content}");
            }
        }
        catch (Exception ex)
        {
            // Ghi lại log lỗi nếu quá trình gửi email thất bại
            throw new Exception($"Failed to send email: {ex.Message}");
        }
    }
}
