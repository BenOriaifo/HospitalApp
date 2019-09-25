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
    public class PatientService
    {
        public List<PatientVM> GetAllPatients()
        {
            List<PatientVM> patients = new List<PatientVM>();
            PatientVM patient = null;

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
                using (SqlConnection con =new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    StringBuilder builder = new StringBuilder();

                    builder.Append("SELECT * FROM dbo.Patient");                    

                    command.CommandText = builder.ToString();
                    command.Connection = con;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        var records = table.Select();

                        foreach (var row in records)
                        {
                            patient = new PatientVM();
                            patient.Id = Convert.ToInt64(row["Id"]);
                            patient.FirstName = row["FirstName"] as string;
                            patient.LastName = row["LastName"] as string;
                            patient.Gender = row["Gender"] as string;
                            patient.Address = row["Address"] as string;
                            patient.Phone = row["Phone"] as string;
                            patient.Email = row["Email"] as string;
                            patient.PatientID = row["PatientID"] as string;
                            patient.BirthDate = row["BirthDate"] as string;
                            patient.Status = row["Status"] as string;
                            patient.RegistrationDate = row["RegistrationDate"] as string;
                            patient.MaritalStatus = row["MaritalStatus"] as string;

                            patients.Add(patient);
                        }
                    }
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

            return patients;
        }

        public bool SavePatient(Patient model)
        {
            bool state = false;

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    StringBuilder builder = new StringBuilder();

                    builder.Append("INSERT INTO dbo.Patient(FirstName, LastName,Gender,Address ,Phone,Email,PatientID,BirthDate,Status ,RegistrationDate,MaritalStatus)");
                    builder.Append("  VALUES(@First, @LastName, @Gender, @Address , @Phone, @Email, @PatientID, @BirthDate, @Status , @RegistrationDate, @MaritalStatus)");

                    command.CommandText = builder.ToString();

                    command.Parameters.AddWithValue("@First", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@Gender", model.Gender);
                    command.Parameters.AddWithValue("@Address", model.Address);
                    command.Parameters.AddWithValue("@Phone", model.Phone);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@PatientID", model.PatientID);
                    command.Parameters.AddWithValue("@BirthDate", model.BirthDate);
                    command.Parameters.AddWithValue("@Status", "Active");
                    command.Parameters.AddWithValue("@RegistrationDate", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    command.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus);
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

        public bool IsPatientIDExist(string patientId)
        {
            bool state = false;
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    StringBuilder builder = new StringBuilder();

                    builder.Append("SELECT PatientID FROM dbo.Patient WHERE PatientID = @patId");

                    command.CommandText = builder.ToString();

                    command.Parameters.AddWithValue("@patId", patientId);
                    command.Connection = con;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        var records = table.Select();

                        foreach (var row in records)
                        {                           
                            var patient = row["PatientID"] as string;

                            if (patient.Trim() == patientId.Trim())
                                state = true;
                        }
                    }
                }
            }
            catch (SqlException oe)
            {
                ExceptionBag bag = new ExceptionBag();
                bag.Message = oe.Message;
                bag.Date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                bag.ExecutingOperation = "IsPatientIDExist";
                bag.InnerException = oe.InnerException == null ? string.Empty : oe.InnerException.ToString();
                ExceptionLogger.LogToFileAsync(bag);
            }

            return state;
        }
    }
}
