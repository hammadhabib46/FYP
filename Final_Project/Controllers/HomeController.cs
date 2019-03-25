using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Final_Project.Models;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;


namespace Final_Project.Controllers
{
	public class HomeController : Controller
	{
        static String activationcode;
        //
        // GET: /Home/

        public ActionResult Index()
		{
			return View();
		}
	   
		public ActionResult CreatorMode()
		{
			return View();
		}
		public ActionResult UsersMode()
		{
			return View();
		}
		public ActionResult SignUp()
		{
			return View();
		}

		public ActionResult UserLogin()
		{
            int MS_id = (int)Session["M_id"];

            using (testdbEntiies objj = new testdbEntiies())
            {

                var clz = objj.roledatas.SqlQuery("Select * from roledata where MS_iid ='" + MS_id + "'").ToList<roledata>();

                List<string> roleList = new List<string>();
                
                foreach (var x in clz)
                {
                    roleList.Add(x.Role_Name);
                }
                TempData["roles"] = roleList;
            }
            
            return View();
		}

		public ActionResult SelectMS()
		{
			return View();
		}

        public ActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyEmail(String vfc)
        {
            ViewBag.codeError = "Sucess";
            creator signupuserdet = (creator)Session["signupdata"];
            if (vfc != activationcode)
            {
                ViewBag.codeError = "Invalid code!, Try Again";

                return View();
            }
            string passEncrypt = Encrypt(signupuserdet.c_password);
            signupuserdet.c_password = passEncrypt;

            // if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.creators.Add(signupuserdet);
                    obj.SaveChanges();
                }
                ModelState.Clear();
                Session["msg"] = signupuserdet.firstname + " " + signupuserdet.lastname + " successfully registered";
            }


            return RedirectToAction("SignUp", "Home");
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";

            int len = cipherText.Length;
            
  //          cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public void Sendcode()
        {
            creator s = (creator)Session["signupdata"];
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("f158034@nu.edu.pk", "64>JyKS=");
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.Subject = "NEXUM - Creator Email Verification Code";
            msg.Body = "Dear " + s.firstname + " " + s.lastname + "," + " Your Email Verification Code is " + activationcode + "\n\n\nThank You for being a Creator at NEXUM !";
            String toaddress = (String)s.email;
            msg.To.Add(toaddress);
            String fromaddress = "NEXUM <f158034@nu.edu.pk>";
            msg.From = new MailAddress(fromaddress);
            try
            {
                smtp.Send(msg);
            }
            catch
            {
                throw;
            }


        }

        //sign up creator mode
        [HttpPost]
        public ActionResult SignUp(creator signup, string ConfirmPassword)
        {

            using (testdbEntiies objj = new testdbEntiies())
            {
                try
                {
                    var usr = objj.creators.Single(u => u.email == signup.email);
                    if (usr != null)
                    {
                        ModelState.AddModelError("email", "Email Already Exists");
                    }
                }
                catch (Exception ex)
                {
                    if (ConfirmPassword != signup.c_password)
                    {
                        ModelState.AddModelError("c_password", "Password Did not Match");
                    }
                    else
                    {
                        //-----------verifuy email
                        Session["signupdata"] = signup;
                        Random random = new Random();
                        activationcode = random.Next(1001, 9999).ToString();
                        Sendcode();
                        return RedirectToAction("VerifyEmail", "Home");
                    }
                }

            }
            return View();
        }

        //sign in creator mode
        [HttpPost]
		public ActionResult CreatorMode(CreatorLogin user)
		{
        
            String passDecrypt = Encrypt(user.Password);

            using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{
					var usr = objj.creators.Single(u => u.email == user.Name && u.c_password == passDecrypt);
					if (usr != null)
					{
						var userId = objj.creators.Where(u => u.email == user.Name).Select(u => u.id).FirstOrDefault();

						TempData["log_user"] = userId;
					   
						return RedirectToAction("MainMenu","Creator");
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("Password", "email or password is incorrect");
				}

			   }
			return View();
		}

        //sign in user mode
        [HttpPost]
        public ActionResult UserLogin(CreatorLogin user)
        {
            int ms_id = (int)Session["M_ID"];
            

            if (user.role == "Admin")
            {

                string passDecrypt = Encrypt(user.Password);
                using (testdbEntiies objj = new testdbEntiies())
                {
                    try
                    {
                        var usr = objj.creators.Single(u => u.email == user.Name && u.c_password == passDecrypt);
                        if (usr != null)
                        {

                            var userId = objj.creators.Where(u => u.email == user.Name).Select(u => u.id).FirstOrDefault();

                            if ((int)TempData["Cr_ID"] == (int)userId) // checking if logging in userID = MS creatorID
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                ModelState.AddModelError("Password", "Doesnt Match Any System");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Password", "email or password is incorrect");
                    }

                }
            }
            else
                if (user.role == "Student")
            {
                using (testdbEntiies objj = new testdbEntiies())
                {
                    try
                    {
                        var usr = objj.studfuctionals.Single(u => u.studF_RollNO == user.Name && u.studF_password == user.Password && u.studF_MSID == ms_id);
                        if (usr != null)
                        {
                            
                            var userId = objj.studfuctionals.Where(u => u.studF_RollNO == user.Name).Select(u => u.studF_ID).FirstOrDefault();
                            TempData["st_id"] = userId;

                      
                            return RedirectToAction("Index", "Student");
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Password", "User ID or password is incorrect");
                    }

                }

            }
            else
                if (user.role == "HR")
            {
                using (testdbEntiies objj = new testdbEntiies())
                {
                    try
                    {
                        var usr = objj.hrfuctionals.Single(u => u.HrF_userNumber == user.Name && u.HrF_password == user.Password && u.HrF_MsID == ms_id);
                        if (usr != null)
                        {
                            TempData["hr_id"] = usr.HrF_ID;
                            return RedirectToAction("Index", "HR");

                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Password", "User ID or password is incorrect");
                    }

                }

            }
            else  /// means thats a teacher
            {
                using (testdbEntiies objj = new testdbEntiies())
                {
                    try
                    {
                        var usr = objj.tchrfunctionals.Single(u => u.TchrF_RollID == user.Name && u.TchrF_password == user.Password && u.TchrF_MSID == ms_id);
                        if (usr != null)
                        {
                            TempData["hr_id"] = usr.TchrF_ID;
                            return RedirectToAction("Index", "Student");

                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Password", "User ID or password is incorrect");
                    }

                }

            }

            return View();
		   
		}
        
		//
		[HttpPost]
		public ActionResult SelectMS(CreatorLogin MS_Name)
		{
			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{

					{
						var MS_Present = objj.ms.Where(b => b.MS_InstName == MS_Name.Name).FirstOrDefault();
						//creatorname is used for MS name search

						if (MS_Present != null)
						{
							var creatorID = objj.ms.Where(u => u.MS_InstName == MS_Name.Name).Select(u => u.C_ID).FirstOrDefault();
							
							TempData["Cr_ID"] = (int)creatorID;


							var systemID = objj.ms.Where(u => u.MS_InstName == MS_Name.Name).Select(u => u.MS_ID).FirstOrDefault();

							Session["M_ID"] = systemID;
                            

							return RedirectToAction("UserLogin","Home");
						}
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("CreatorName", "This Sytem Does not Exist.");
				}

			}




			return View();
		}

	}

	public class CopyOfHomeController : Controller
	{
		//
		// GET: /Home/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult CreatorMode()
		{
			return View();
		}
		public ActionResult UsersMode()
		{
			return View();
		}
		public ActionResult SignUp()
		{
			return View();
		}

		public ActionResult UserLogin()
		{
			return View();
		}

		public ActionResult SelectMS()
		{
			return View();
		}



		//sign up creator mode
		[HttpPost]
		public ActionResult SignUp(creator signup, string ConfirmPassword)
		{

			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{
					var usr = objj.creators.Single(u => u.email == signup.email);
					if (usr != null)
					{
						ModelState.AddModelError("email", "Email Already Exists");
					}
				}
				catch (Exception ex)
				{
					if (ConfirmPassword != signup.c_password)
					{
						ModelState.AddModelError("c_password", "Password Did not Match");
					}
					else
					{
						if (ModelState.IsValid)
						{
							using (testdbEntiies obj = new testdbEntiies())
							{
								obj.creators.Add(signup);
								obj.SaveChanges();
							}
							ModelState.Clear();
							ViewBag.msg = signup.firstname + " " + signup.lastname + " successfully registered";
						}
					}
				}

			}
			return View();
		}


		//sign in creator mode
		[HttpPost]
		public ActionResult CreatorMode(CreatorLogin user)
		{

			string x = user.Password;
			x = user.Name;
			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{
					var usr = objj.creators.Single(u => u.email == user.Name && u.c_password == user.Password);
					if (usr != null)
					{
						var userId = objj.creators.Where(u => u.email == user.Name).Select(u => u.id).FirstOrDefault();

						TempData["log_user"] = userId;

						return RedirectToAction("MainMenu", "Creator");
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("Password", "email or password is incorrect");
				}

			}
			return View();
		}


		//sign in user
		[HttpPost]
		public ActionResult UserLogin(CreatorLogin user)
		{

			string x = user.Password;
			x = user.Name;// ............ ?

			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{
					var usr = objj.creators.Single(u => u.email == user.Name && u.c_password == user.Password);
					if (usr != null)
					{

						var userId = objj.creators.Where(u => u.email == user.Name).Select(u => u.id).FirstOrDefault();

						if ((int)TempData["Cr_ID"] == (int)userId) // checking if logging in userID = MS creatorID
						{
							return RedirectToAction("Index", "Admin");
						}
						else
						{
							ModelState.AddModelError("Password", "Doesnt Match Any System");
						}


					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("Password", "email or password is incorrect");
				}

			}

			return View();

		}


		//
		[HttpPost]
		public ActionResult SelectMS(CreatorLogin MS_Name)
		{
			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{

					{
						var MS_Present = objj.ms.Where(b => b.MS_InstName == MS_Name.Name).FirstOrDefault();
						//creatorname is used for MS name search

						if (MS_Present != null)
						{
							var creatorID = objj.ms.Where(u => u.MS_InstName == MS_Name.Name).Select(u => u.C_ID).FirstOrDefault();

							TempData["Cr_ID"] = (int)creatorID;


							var systemID = objj.ms.Where(u => u.MS_InstName == MS_Name.Name).Select(u => u.MS_ID).FirstOrDefault();

							TempData["M_ID"] = (int)systemID;


							return RedirectToAction("UserLogin", "Home");
						}
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("CreatorName", "This Sytem Does not Exist.");
				}

			}




			return View();
		}

	}
}