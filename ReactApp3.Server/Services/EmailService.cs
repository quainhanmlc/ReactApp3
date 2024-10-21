using MongoDB.Driver;
using ReactApp3.Server.Models;
using RestSharp;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _apiKey;
    private readonly string _senderEmail;
    private readonly string _senderName;
    
    private readonly IMongoCollection<ConfirmEmail> _confirmEmails;

    public EmailService(IConfiguration config, IMongoDatabase database)
    {
        _apiKey = config["BrevoSettings:ApiKey"];
        _senderEmail = config["BrevoSettings:SenderEmail"];
        _senderName = config["BrevoSettings:SenderName"];
        _confirmEmails = database.GetCollection<ConfirmEmail>("ConfirmEmails");
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
    // Lưu mã xác nhận vào DB
    public async Task SaveConfirmCodeAsync(string userId, string confirmCode)
    {
        var confirmEmail = new ConfirmEmail
        {
            UserId = userId,
            ConfirmCode = confirmCode,
            ExpiryTime = DateTime.Now.AddMinutes(30),
            CreateTime = DateTime.Now,
            IsConfirm = false
        };
        await _confirmEmails.InsertOneAsync(confirmEmail);
    }

    // Xác nhận mã email
    public async Task<ConfirmEmail> GetValidConfirmCodeAsync(string userId, string code)
    {
        var confirmEmail = await _confirmEmails
            .Find(x => x.UserId == userId && x.ConfirmCode == code && x.ExpiryTime > DateTime.Now)
            .FirstOrDefaultAsync();
        return confirmEmail;
    }

    // Đánh dấu email là đã xác nhận
    public async Task ConfirmEmailAsync(ConfirmEmail confirmEmail)
    {
        confirmEmail.IsConfirm = true;
        await _confirmEmails.ReplaceOneAsync(x => x.Id == confirmEmail.Id, confirmEmail);
    }

}
