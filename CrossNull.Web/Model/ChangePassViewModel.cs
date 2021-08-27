using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Web.Model
{
    public class ChangePassViewModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
