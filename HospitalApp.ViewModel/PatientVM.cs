using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp.ViewModel
{
    public class PatientVM
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PatientID { get; set; }
        public string BirthDate { get; set; }
        public string Status { get; set; }
        public string RegistrationDate { get; set; }
        public string MaritalStatus { get; set; }
    }
}
