using HospitalApp.Service;
using System;
using HospitalApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace HospitalApp.Web.Controllers
{
    public class PatientController : Controller
    {
        PatientService _service = new PatientService();

        public PatientController()
        {

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData()
        {
           
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


            //Paging Size (10,20,50,100)    
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var patients = _service.GetAllPatients();
            // Getting all Customer data    
            var patient = (from tempcustomer in patients
                                select tempcustomer);

            //Sorting    
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                patient = patient.OrderBy(sortColumn + " " + sortColumnDir);
            }
            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchData = searchValue.Trim();
                patient = patient.Where(m => m.FirstName.ToString().ToLower().Contains(searchData.ToLower()) || 
                                            m.LastName.ToString().ToLower().Contains(searchData) ||
                                            m.Gender.ToString().ToLower().Contains(searchData) ||
                                            m.PatientID.ToString().ToLower().Contains(searchData) ||
                                            m.BirthDate.ToString().ToLower().Contains(searchData) ||
                                            m.Email.ToString().ToLower().Contains(searchData) ||
                                            m.Phone.ToString().ToLower().Contains(searchData) ||
                                            m.Status.ToString().ToLower().Contains(searchData) ||
                                            m.MaritalStatus.ToString().ToLower().Contains(searchData) ||
                                            m.RegistrationDate.ToString().ToLower().Contains(searchData)).ToList();
            }

            //total number of rows count     
            recordsTotal = patient.Count();
            //Paging     
            var data = patient.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = patients.Count, data = data }, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult Add(Patient patientVM)
        {
            string message = string.Empty;
            string result = string.Empty;
            if(patientVM == null)
            {
                message = "Record to insert empty";
                return Json(message);
            }
            else
            {
                var insertResult = _service.SavePatient(patientVM);

                if (insertResult)
                    result = "Record successfully saved";
                else
                    result = "Failed to save record";

                return Json(result);
            }
        }

        [HttpPost]
        public ActionResult IsPatientExists(string patientID)
        {
            string message = string.Empty;
            if (patientID == null)
            {
                message = "Patient ID is empty";
                return Json(message);
            }
            else
            {
                var exists = _service.IsPatientIDExist(patientID);

                if (exists)
                    message = "Patient ID exists...Patient ID must be unique";
                else
                    message = "Patient ID does not exists...Continue";

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}