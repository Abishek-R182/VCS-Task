using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using VCSProject.Models;
using VCSProject.Services;

namespace VCSProject.Controllers
{
    public class EmployeeController : Controller
    {
        public readonly ApplicationDBContext context;
        private readonly IWebHostEnvironment environment;
        private readonly string connectionString;

        public EmployeeController(ApplicationDBContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
            this.environment = environment;
            this.context = context;
        }
        public IActionResult Index()
        {
            var employee = context.Employees.ToList();
            return View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Employee employeedto)
        {

            string qualifications = string.Join(",", employeedto.Qualifications);
            if (employeedto.Image == null)
            {
                ModelState.AddModelError("Image", "The image file is required");
            }


            

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", employeedto.Name);
                cmd.Parameters.AddWithValue("@DateOfBirth", employeedto.DateOfBirth);
                cmd.Parameters.AddWithValue("@Email", employeedto.Email);
                cmd.Parameters.AddWithValue("@Gender", employeedto.Gender);
                cmd.Parameters.AddWithValue("@Qualification", qualifications);
                

                string uniqueFileName = null;

                if (employeedto.Image != null && employeedto.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(environment.WebRootPath, "Screenshots");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + employeedto.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        employeedto.Image.CopyTo(fileStream);
                    }
                }
                cmd.Parameters.AddWithValue("@ImagePath", uniqueFileName ?? (object)DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();              
            }




            return RedirectToAction("Index");

        }


        public IActionResult Edit(int id)
        {
            var employee = context.Employees.Find(id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }



            var employeedto = new Employeedto()
            {
                Name = employee.Name,
                DateOfBirth = employee.DateOfBirth,
                Email = employee.Email,
                Gender = employee.Gender,
                //Qualification = employee.Qualification,


            };


            ViewData["EmployeeId"] = employee.ID;
            ViewData["Image"] = employee.ImagePath;

            return View(employeedto);


        }

        [HttpPost]
        public IActionResult Edit(int id, Employee employeedto)
        {

            var employee = context.Employees.Find(id);
            string qualifications = string.Join(",", employeedto.Qualifications);
            if (employee == null)
            {
                return RedirectToAction("Index", "Employee");
            }

            

          
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", employee.ID);
                cmd.Parameters.AddWithValue("@Name", employeedto.Name);
                cmd.Parameters.AddWithValue("@DateOfBirth", employeedto.DateOfBirth);
                cmd.Parameters.AddWithValue("@Email", employeedto.Email);
                cmd.Parameters.AddWithValue("@Gender", employeedto.Gender);
                cmd.Parameters.AddWithValue("@Qualification", qualifications);

                string uniqueFileName = null;
                string OldImagePath = employee.ImagePath;

                if (employeedto.Image != null && employeedto.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(environment.WebRootPath, "Screenshots");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + employeedto.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        employeedto.Image.CopyTo(fileStream);
                    }

                    if (!string.IsNullOrEmpty(OldImagePath)) // Ensure old file exists in DB
                    {
                        string oldFilePath = Path.Combine(uploadsFolder, OldImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }

                if (uniqueFileName != null)
                {
                    cmd.Parameters.AddWithValue("@ImagePath", uniqueFileName ?? (object)DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ImagePath", OldImagePath ?? (object)DBNull.Value);
                }

                conn.Open();
                cmd.ExecuteNonQuery();

            }


            return RedirectToAction("Index", "Employee");

        }

    

        public IActionResult Delete(int id)
        {
            var employee = context.Employees.Find(id);

            if (employee == null)
            {
                return RedirectToAction("Index", "Employee");
            }

            var employeedto = new Employeedto()
            {
                Name = employee.Name,
                DateOfBirth = employee.DateOfBirth,
                Email = employee.Email,
                Gender = employee.Gender,
                //Qualification = employee.Qualification,


            };

            ViewData["EmployeeId"] = employee.ID;
            ViewData["Image"] = employee.ImagePath;

            return View(employeedto);
        }


        [HttpPost]
        public IActionResult Delete(int id, Employee employeedto)
        {
            var employee = context.Employees.Find(id);

            if (employee == null)
            {
                return RedirectToAction("Index", "Employee");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", employee.ID);

                conn.Open();

                // Execute the stored procedure
                cmd.ExecuteNonQuery();
            }


            return RedirectToAction("Index");

        }





    }
}
