using Microsoft.AspNetCore.Mvc;
using ReactApp3.Server.Helpers;
using ReactApp3.Server.Models.DTO;
using ReactApp3.Server.Services;
using System;
using System.Threading.Tasks;
using static EmailService;

namespace ReactApp3.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtHelper _jwtHelper;
        private readonly EmailService _emailService;

        public AuthController(UserService userService, JwtHelper jwtHelper, EmailService emailService)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
            _emailService = emailService;
        }



        // Đăng ký
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = await _userService.RegisterAsync(registerDto.Username, registerDto.Email, registerDto.Password);

                // Tạo mã xác nhận và lưu vào DB
                var confirmCode = Guid.NewGuid().ToString().Substring(0, 6); // Mã 6 ký tự
                await _emailService.SaveConfirmCodeAsync(user.Id, confirmCode);

                // Gửi mã xác nhận qua email
                var subject = "Email Confirmation";
                var content = $"Mã xác nhận của bạn là: {confirmCode}";
                await _emailService.SendEmailAsync(registerDto.Email, subject, content);

                return Ok(new { message = "User registered. Check your email for the confirmation code." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            var confirmEmail = await _emailService.GetValidConfirmCodeAsync(confirmEmailDto.UserId, confirmEmailDto.Code);

            if (confirmEmail == null)
                return BadRequest(new { message = "Invalid or expired confirmation code." });

            // Đánh dấu email là đã xác nhận
            await _emailService.ConfirmEmailAsync(confirmEmail);

            // Kích hoạt tài khoản người dùng
            var user = await _userService.GetUserByIdAsync(confirmEmail.UserId);
            user.IsActive = true;
            await _userService.UpdateUserAsync(user);

            return Ok(new { message = "Email confirmed successfully. Please log in." });
        }


        // Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password);

                if (user == null)
                    return Unauthorized(new { message = "Invalid username or password." });

                var roles = await _userService.GetUserRolesAsync(user.Id); // Lấy danh sách các vai trò

                var token = _jwtHelper.GenerateToken(user); // Tạo JWT token

                return Ok(new
                {
                    token = token,
                    user = new
                    {
                        user.Id,
                        user.Username,
                        user.Email
                    },
                    roles = roles  // Trả về danh sách vai trò của người dùng
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


        // Quên mật khẩu
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userService.GetUserByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest(new { message = "Email không tồn tại trong hệ thống." });

            // Xóa token cũ nếu đã tồn tại
            if (!string.IsNullOrEmpty(user.PasswordResetToken))
            {
                user.PasswordResetToken = null;
                user.PasswordResetExpiry = null;
            }

            // Tạo mã token mới và lưu vào DB
            var resetToken = await _userService.GeneratePasswordResetTokenAsync(user);

            // Gửi email khôi phục mật khẩu với đường link chứa token
            var resetLink = $"https://localhost:5173/reset-password/{resetToken}";
            var subject = "Password Reset";
            var content = $"Click vào link sau để đặt lại mật khẩu: {resetLink}";

            await _emailService.SendEmailAsync(forgotPasswordDto.Email, subject, content);

            return Ok(new { message = "Link khôi phục đã được gửi qua email." });
        }


        // Đặt lại mật khẩu
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                await _userService.ResetPasswordAsync(resetPasswordDto.Email, resetPasswordDto.Token, resetPasswordDto.NewPassword);
                return Ok(new { message = "Mật khẩu đã được đặt lại thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Đổi mật khẩu
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var user = await _userService.AuthenticateAsync(changePasswordDto.Username, changePasswordDto.CurrentPassword);
            if (user == null) return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không chính xác." });

            await _userService.UpdatePasswordAsync(user, changePasswordDto.NewPassword);
            return Ok(new { message = "Mật khẩu đã được thay đổi thành công." });
        }
    }
}
