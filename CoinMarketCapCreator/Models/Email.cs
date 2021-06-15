using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMarketCapCreator.Models
{
    class Email
    {
        public string email { get; set; }
        public string password { get; set; }

        public Email(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
