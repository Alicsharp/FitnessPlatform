using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException()
            : base("تداخل در بروزرسانی داده‌ها رخ داد.")
        {
        }

        public ConcurrencyException(string message)
            : base(message)
        {
        }

        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
