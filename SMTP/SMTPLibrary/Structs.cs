using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPLibrary
{
    public struct SendResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        public SendResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
