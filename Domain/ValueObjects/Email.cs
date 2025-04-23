using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public sealed class Email
    {
        public string Address { get; private set; }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email address cannot be empty.", nameof(address));

            if (!IsValidEmail(address))
                throw new ArgumentException("Invalid email address format.", nameof(address));

            Address = address;
        }
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public override string ToString() => Address;
    }
}
