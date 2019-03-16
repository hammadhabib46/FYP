using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final_Project.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Sql;


namespace Final_Project.Controllers
{
	public class AdminController : Controller
	{
		//
		// GET: /Admin/

		public int GenerateRandomNo()
		{
			int _min = 1000;
			int _max = 9999;
			Random _rdm = new Random();
			return _rdm.Next(_min, _max);
		}


		public ActionResult Index()
		{

			string name = "Admin";
			int MS_id = (int)TempData["M_ID"];
			TempData.Keep();
		//	int MS_id = 21;
			int creatorid = (int)TempData["Cr_ID"]; 
			TempData.Keep();
		//	int creatorid = 9;

			using (testdbEntiies objj = new testdbEntiies())
			{

				var role_Id = objj.roledatas.Where((u => u.MS_iid == MS_id && u.Role_Name == name)).Select(u => u.Role_ID).FirstOrDefault();   

				int r_id = (int)role_Id;
				TempData["Role_ID"] = r_id;


				var  noti_role = objj.role_funcdata.Where((u => u.Role_ID == r_id)).Select(u => u.GiveNotification).FirstOrDefault();   

				TempData["NotiRole"] = (bool)noti_role;


				var Data= objj.creators.SqlQuery("Select * from creator where id = '" + creatorid + "' ").FirstOrDefault<creator>();

				
				TempData["P_Name"] = Data.firstname + Data.lastname;
				TempData["E_Mail"] = Data.email;
				TempData["Post"] = "Admin";

				
			}

			return View();
		}

		public ActionResult AddStudent()
		{
			int MS_d = (int)TempData["M_ID"];
			TempData.Keep();
				int creatorid = (int)TempData["Cr_ID"];
			TempData.Keep();
		//	int MS_d = 21;
			int roll_id=(int)TempData["Role_ID"];
			TempData.Keep();
			string st = "Student";


			using (testdbEntiies objj = new testdbEntiies())
			{

                try
                {
                    var clz = objj.classes.SqlQuery("Select * from classes where MS_id ='" + MS_d + "'").ToList<@class>();

                    TempData["clz"] = clz;



                    List<string> classList = new List<string>();

                    foreach (var x in clz)
                    {
                        classList.Add(x.Class_Name);
                        TempData.Keep();

                    }
                    TempData["classL"] = classList;
                    TempData["ClassBool"] = true;  
                }
                catch(Exception ex)
                {
                    TempData["ClassBool"] = false;  
                 
                
                }

				
				

				var Data = objj.roledatas.SqlQuery("Select * from roledata where MS_iid = '" + MS_d + "' AND Role_Name = '"+st+"' ").FirstOrDefault<roledata>();

				TempData["Sphone"] = Data.Role_Phone;
				TempData["Sdon"] = Data.Role_Dob;
				TempData["Saddress"] = Data.Role_Address;
				TempData["Scnic"] = Data.Role_CNIC;
				TempData["Sgender"] = Data.Role_Gender;
				TempData["Spic"] = Data.Role_Pic;
				TempData["SgruarCNIC"] = Data.Role_FthrCNIC;
				TempData["SfatherContact"] = Data.Role_FthrPhone;
				TempData["SEmail"] = Data.Role_Email;
				TempData["Sportal"] = Data.Role_Portal;


			}



			return View();
		}

		public ActionResult DeleteStudent()
		{

			return View();
		}

		public ActionResult UpdateHR()
		{
			return View();
		
		}
		public ActionResult UpdateStudent()
		{
			return View();
		}


		[HttpPost]
		public ActionResult AddStudent(studfuctional stu_add)
		{
			 int MS_d = (int)TempData["M_ID"];
			TempData.Keep();

		  //  int MS_d = 21;
			int random;

			string rol;
			//////////// random generation of student id
			bool num=false;
			do
			{
				 random = GenerateRandomNo();
				 rol = random.ToString();
				 rol = "S_" + rol;
				using (testdbEntiies obj = new testdbEntiies())
				{
					try
					{
						var usr = obj.studfuctionals.Single(u => u.studF_RollNO == rol);
					}
					catch(Exception e)
					{

						num = true;
					}
					   
				}


			} while (num == false);

			///////////////////////////random generation of student id
			stu_add.studF_MSID = MS_d;
			stu_add.studF_RollNO = rol;
			stu_add.studF_password = rol;



			if (ModelState.IsValid)
			{
				using (testdbEntiies obj = new testdbEntiies())
				{
					obj.studfuctionals.Add(stu_add);
					obj.SaveChanges();
				}
				ModelState.Clear();
				ViewBag.msg1 = "Successfully Added";
			}



			return View();
		}

        
        [HttpPost]
        public ActionResult DeleteStudent(string Roll_number)
        {
        //    int MS_id = (int)TempData["M_ID"];
            int MS_id = 21;
            TempData.Keep();

            using (testdbEntiies objj = new testdbEntiies())
            {


                try
                {
                    var usr = objj.studfuctionals.Single(u => u.studF_RollNO == Roll_number && u.studF_MSID == MS_id);
                    if (usr != null)
                    {
                        var Data = objj.creators.SqlQuery("Delete from studfuctional where studF_RollNO = '" + Roll_number + "' ").FirstOrDefault<creator>();
                        ViewBag.delete = "SuccessFully Deleted";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.delete = "Deleted";

                }

            }


            return View();
        }
	}
}