using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Services
{
    public class OtpService
    {
        private readonly EmailService _emailService;
        private readonly ConcurrentDictionary<string, string> _otpStore = new();
        public OtpService(EmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task<string> GenerateOtpAsync(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _otpStore[email] = otp;

            //_emailService.SendSimpleEmail(email, "Your OTP", $"Your OTP is: {otp}");
            Console.WriteLine($"Generated OTP for {email}: {otp}");

            return otp;
        }

        public bool VerifyOtp(string email, string otp)
        {
            if (_otpStore.TryGetValue(email, out var storedOtp) && storedOtp == otp)
            {
                _otpStore.TryRemove(email, out _);
                return true;
            }

            return false;
        }
    }
}
