using HospitalApp.Model;
using HospitalApp.Utility;
using HospitalApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp.Service
{
    public class PaymentService
    {
        public List<PaymentVM> GetAllPayments()
        {
            List<PaymentVM> payments = new List<PaymentVM>();
            PaymentVM payment = null;

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
                using (SqlConnection con =new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    StringBuilder builder = new StringBuilder();

                    builder.Append("SELECT Payments.Id, Payments.PatientID, Payments.AmountPaid, Patient.FirstName, Patient.LastName,Payments.Date, Payments.PaymentFor, Payments.PaymentMode, Employee.Name PaymentCapturedBy FROM dbo.Payments");
                    builder.Append("  INNER JOIN Employee ON Payments.CapturedBy = Employee.Id");
                    builder.Append("  INNER JOIN Patient ON Payments.PatientID = Patient.PatientID");

                    command.CommandText = builder.ToString();
                    command.Connection = con;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        var records = table.Select();

                        foreach (var row in records)
                        {
                            payment = new PaymentVM();
                            payment.Id = Convert.ToInt64(row["Id"]);
                            payment.PatientID = row["PatientID"] as string;
                            payment.AmountPaid = Convert.ToDecimal(row["AmountPaid"]);
                            payment.Date = row["Date"] as string;
                            payment.PaymentCapturedBy = row["PaymentCapturedBy"] as string;
                            payment.PaymentFor = row["PaymentFor"] as string;
                            payment.PaymentMode = row["PaymentMode"] as string;
                            payment.PatientFullName = row["FirstName"] as string + " " + row["LastName"] as string; ;

                            payments.Add(payment);
                        }
                    }
                }
            }
            catch (SqlException oe)
            {
                ExceptionBag bag = new ExceptionBag();
                bag.Message = oe.Message;
                bag.Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                bag.ExecutingOperation = "GetAllPayments";
                bag.InnerException = oe.InnerException == null ? string.Empty : oe.InnerException.ToString();
                ExceptionLogger.LogToFileAsync(bag);
            }

            return payments;
        }

        public bool SavePayment(PaymentVM model)
        {
            bool state = false;
            long empId = FetchEmployeeId(model.PaymentCapturedBy);
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    StringBuilder builder = new StringBuilder();

                    builder.Append("INSERT INTO dbo.Payments(PatientID, AmountPaid, Date, CapturedBy, PaymentFor, PaymentMode)");
                    builder.Append("  VALUES(@id, @amt, @date, @capBy , @payFor, @PayMode)");

                    command.CommandText = builder.ToString();

                    command.Parameters.AddWithValue("@id", model.PatientID);
                    command.Parameters.AddWithValue("@amt", model.AmountPaid);
                    command.Parameters.AddWithValue("@date", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff tt"));
                    command.Parameters.AddWithValue("@capBy", empId);
                    command.Parameters.AddWithValue("@payFor", model.PaymentFor);
                    command.Parameters.AddWithValue("@PayMode", model.PaymentMode);
                    command.Connection = con;

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                        state = true;
                }
            }
            catch (SqlException oe)
            {
                ExceptionBag bag = new ExceptionBag();
                bag.Message = oe.Message;
                bag.Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                bag.ExecutingOperation = "GetAllPatients";
                bag.InnerException = oe.InnerException == null ? string.Empty : oe.InnerException.ToString();
                ExceptionLogger.LogToFileAsync(bag);
            }

            return state;
        }

        public long FetchEmployeeId(string employeeName)
        {
            long empId = 0;
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    StringBuilder builder = new StringBuilder();

                    builder.Append("SELECT Id FROM dbo.Employee");
                    builder.Append(" WHERE Name = @name");

                    command.CommandText = builder.ToString();
                    command.Parameters.AddWithValue("@name", employeeName);
                    command.Connection = con;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        var records = table.Select();

                        foreach (var row in records)
                        {
                            
                            empId = Convert.ToInt64(row["Id"]);
                        }
                    }
                }
            }
            catch (SqlException oe)
            {
                ExceptionBag bag = new ExceptionBag();
                bag.Message = oe.Message;
                bag.Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                bag.ExecutingOperation = "GetAllPayments";
                bag.InnerException = oe.InnerException == null ? string.Empty : oe.InnerException.ToString();
                ExceptionLogger.LogToFileAsync(bag);
            }

            return empId;
        }

        public List<EmployeeVM> GetEmployeeNames()
        {
            List<EmployeeVM> names = new List<EmployeeVM>();
            EmployeeVM emp = null;

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    StringBuilder builder = new StringBuilder();

                    builder.Append("SELECT Id, Name FROM dbo.Employee");                    

                    command.CommandText = builder.ToString();
                    command.Connection = con;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        var records = table.Select();

                        foreach (var row in records)
                        {
                            emp = new EmployeeVM();
                            emp.Id = Convert.ToInt64(row["Id"]);
                            emp.Name = row["Name"] as string;

                            names.Add(emp);
                        }
                    }
                }
            }
            catch (SqlException oe)
            {
                ExceptionBag bag = new ExceptionBag();
                bag.Message = oe.Message;
                bag.Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                bag.ExecutingOperation = "GetEmployeeNames";
                bag.InnerException = oe.InnerException == null ? string.Empty : oe.InnerException.ToString();
                ExceptionLogger.LogToFileAsync(bag);
            }

            return names;
        }
    }
}
