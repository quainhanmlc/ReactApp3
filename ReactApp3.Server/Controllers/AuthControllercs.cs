using Microsoft.AspNetCore.Mvc;
using ReactApp3.Server.Helpers;
using ReactApp3.Server.Models;
using ReactApp3.Server.Services;
using System;
using System.Threading.Tasks;

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
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password);
                var token = _jwtHelper.GenerateToken(user);
                return Ok(new { token });
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
            if (user == null) return BadRequest(new { message = "Email không tồn tại trong hệ thống." });

            // Tạo mã xác nhận và lưu vào DB
            var resetToken = await _userService.GeneratePasswordResetTokenAsync(user);

            // Gửi mã qua email
            var subject = "Password Reset";
            var content = $"Mã xác nhận của bạn là: {resetToken}";
            await _emailService.SendEmailAsync(forgotPasswordDto.Email, subject, content);

            return Ok(new { message = "Mã xác nhận đã được gửi qua email." });
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
