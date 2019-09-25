using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp.Utility
{
    public class ExceptionBag
    {
        public string ExecutingOperation { set; get; }
        public string InnerException { set; get; }
        public string Date { set; get; }
        public string Message { set; get; }
    }
}
