using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exceptions
{
    public class AuthenticatonErrorException : Exception
    {
        public AuthenticatonErrorException() :base("Authentication is failed"){ }

        public AuthenticatonErrorException(string? message) : base(message)
        {
        }

        public AuthenticatonErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
