using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class RequestOtpDTO{
            public string Email { get; set; }
        }

    public class VerifyOtpDTO
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}


