using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMobile.Models
{
    public class ServiceResponse
    {
        public bool Flag { get; set; } // Indicates success/failure
        public string Message { get; set; } //for custom messages
        public int DatabaseResponseValue { get; set; } //Optional DB return code (like rows affected)
    }
}
