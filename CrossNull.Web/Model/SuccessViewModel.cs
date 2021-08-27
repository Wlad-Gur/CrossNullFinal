using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Web.Model
{
    public class SuccessViewModel
    {
        public SuccessViewModel(string message)
        {
            if (message == null)
            {
                throw new ArgumentException("Message can't be null");
            }
            Message = message;
        }
        public string Message { get; }
    }
}
