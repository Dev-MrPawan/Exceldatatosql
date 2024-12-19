using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Excel_Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Excel_Data.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        private string connectionString = "Data Source =DESKTOP-K94L948;Database=Exceldata; Integrated Security=true";

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            webHostEnvironment = hostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }
        public Regmodel adminlogin(Regmodel logmodel, string connectionString)
        {
            Regmodel obm = new Regmodel();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("proAdmin", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@email", logmodel.email);
                cmd.Parameters.AddWithValue("@Password", logmodel.Password);


                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                sda.Fill(ds);
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    obm.email = ds.Tables[1].Rows[0]["email"].ToString();
                    obm.result = ds.Tables[0].Rows[0][0].ToString();

                }
            }
            return obm;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Check if a file is provided
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "Please upload a file.";
                return View("Index");
            }

            // Validate file extension
            var allowedExtensions = new[] { ".xls", ".xlsx", ".csv" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            string uniqueFileNameb = null;
            if (!allowedExtensions.Contains(fileExtension))
            {

                ViewBag.Error = "Invalid file type. Please upload a .xls, .xlsx, or .csv file.";
                return View("Upload_Excel");
            }
            else
            {
   

                if (file != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "excel");
                    uniqueFileNameb = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filepath = Path.Combine(uploadsFolder, uniqueFileNameb);
                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

            }
           string filename = uniqueFileNameb;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("proupload", con)
                {

                    CommandType = CommandType.StoredProcedure
                };
                try
                {
                    var scoreResults = new List<ScoreResult>();

                    using (var workbook = new XLWorkbook(webHostEnvironment.WebRootPath +"/excel/"+  filename))
                    {
                        var isHeaderRow = true;

                        foreach (var row in workbook.Worksheet(1).RowsUsed())
                        {
                            if (isHeaderRow)
                            {
                                isHeaderRow = false;
                                continue;
                            }

                            var name = row.Cell(1).GetString();
                            var age = row.Cell(2).GetString();
                            var email = row.Cell(3).GetString();
                        

                            for (int i = 3; i <= row.LastCellUsed().Address.ColumnNumber; i += 3)
                            {
                                Console.WriteLine(row.Cell(i).GetString());

                                var scoreResult = new ScoreResult
                                {
                                   name= name,
                                   age=age,
                                   email=email,

                                    // Score = row.Cell(i + 1).GetValue<int>(),
                                };

                                scoreResults.Add(scoreResult);
                            }
                        }
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add("name");
                    dt.Columns.Add("age");
                    dt.Columns.Add("email");
                  

                    foreach (var s in scoreResults)
                    {

                        // Console.WriteLine($"Name: {s.Name}");
                        dt.Rows.Add(new object[] { $"{s.name}", $"{s.age}", $"{s.email}" });
                        //dt.Rows.Add(new object[] { $" {s.Email}" });
                    }
                   string df = HttpContext.Session.GetString("email");
                 //   cmd.Parameters.AddWithValue("@action", "insert1");
                    cmd.Parameters.AddWithValue("@username", df);
                    cmd.Parameters.AddWithValue("@filename", filename);
                    cmd.Parameters.AddWithValue("@ex", dt);
                    

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    // rg.userid = ds.Tables[0].Rows[0][0].ToString();
                }


                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.ToString());
                }

            }



                ViewBag.Message = "File uploaded successfully.";
            return RedirectToAction("Excel_Data");
        }


        public IActionResult Login()
        {
            return View();
           
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {

            Regmodel loginmodel = new Regmodel();
            loginmodel.email = email;
            loginmodel.Password = password;
            loginmodel = adminlogin(loginmodel, connectionString);
            if (loginmodel.result == "1")
            {
                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("flags", "1");
                return RedirectToAction("Upload_Excel", "Home");
            }
            else
            {
                ViewBag.errors = "Invalid ID or Password";
                return View("Login");
            }
        }

        public IActionResult Excel_Data()
        {
            string s = HttpContext.Session.GetString("flags");
            if (s == "1")
            {
               
                exlist rl = new exlist();

                rl = userlit(connectionString);
                return View(rl);
            }
            else
            {
                return RedirectToAction("home", "Login");
            }
        }
        public uplist userlits(string connectionString)
        {
            uplist rl = new uplist();
            List<up> studentList = new List<up>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Proinfo", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@action", "up");

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    up rp = new up();

                    rp.username = ds.Tables[0].Rows[i]["username"].ToString();
                    rp.filename = ds.Tables[0].Rows[i]["filename"].ToString();
                   
                    rp.tdate = ds.Tables[0].Rows[i]["tdate"].ToString();            

                    rp.indexid = ds.Tables[0].Rows[i]["indexid"].ToString();
                    studentList.Add(rp);
                }
                rl.list = studentList;
                return rl;
            }
        }
        public exlist userlit(string connectionString)
        {
            exlist rl = new exlist();
            List<ex> studentList = new List<ex>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Proinfo", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@action", "ex");

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ex rp = new ex();

                    rp.name = ds.Tables[0].Rows[i]["name"].ToString();
                    rp.email = ds.Tables[0].Rows[i]["email"].ToString();
                    rp.age = ds.Tables[0].Rows[i]["age"].ToString();
                    rp.tdate = ds.Tables[0].Rows[i]["tdate"].ToString();            

                    rp.indexid = ds.Tables[0].Rows[i]["indexid"].ToString();
                    studentList.Add(rp);
                }
                rl.list = studentList;
                return rl;
            }
        }
        public IActionResult Upload_Excel()
        {
            string s = HttpContext.Session.GetString("flags");
            if (s == "1")
            {
                return View();
            }
            else
            {
                return View("home", "Login");
            }
        }
        public IActionResult Upload_History()
        {
            string s = HttpContext.Session.GetString("flags");
            if (s == "1")
            {

                uplist   rl = new uplist();

                rl = userlits(connectionString);
                return View(rl);
            }
            else
            {
                return RedirectToAction("home", "Login");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
