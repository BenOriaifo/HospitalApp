using HospitalApp.Service;
using System.Linq.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalApp.ViewModel;

namespace HospitalApp.Web.Controllers
{
    public class PaymentController : Controller
    {
        PaymentService _service = new PaymentService();
        // GET: Payment
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


            //Paging Size    
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var payments = _service.GetAllPayments();
            // Getting all Customer data    
            var payment = (from pay in payments
                           select pay);

            //Sorting    
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                payment = payment.OrderBy(sortColumn + " " + sortColumnDir);
            }
            //Search
            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchData = searchValue.Trim();
                payment = payment.Where(m => m.PatientID.ToString().ToLower().Contains(searchData.ToLower()) ||
                                            m.Date.ToString().ToLower().Contains(searchData) ||
                                            m.PaymentCapturedBy.ToString().ToLower().Contains(searchData) ||
                                            m.PatientFullName.ToString().ToLower().Contains(searchData) ||
                                            m.PaymentFor.ToString().ToLower().Contains(searchData) ||
                                            m.PaymentMode.ToString().ToLower().Contains(searchData)).ToList();
            }

            //total number of rows count     
            recordsTotal = payment.Count();
            //Paging     
            var data = payment.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = payments.Count, data = data }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Add(PaymentVM paymentVM)
        {
            string result = string.Empty;
            if (paymentVM == null)
            {
                return Json(new { message = "Record to insert empty" });
            }
            else
            {
                var insertResult = _service.SavePayment(paymentVM);

                if (insertResult)
                    result = "Record successfully saved";
                else
                    result = "Failed to save record";

                return Json(result);
            }
        }

        public ActionResult GetEmployees()
        {
            List<EmployeeVM> emp = _service.GetEmployeeNames();

            return Json(emp, JsonRequestBehavior.AllowGet);
        }
    }
}