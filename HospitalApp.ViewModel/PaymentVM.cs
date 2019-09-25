using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp.ViewModel
{
    public class PaymentVM
    {
        public long Id { get; set; }
        public string PatientID { get; set; }
        public decimal AmountPaid { get; set; }
        public string Date { get; set; }
        public long CapturedBy { get; set; }
        public string PaymentCapturedBy { get; set; }
        public string PaymentFor { get; set; }
        public string PaymentMode { get; set; }
        public string PatientFullName { get; set; }
    }
}
