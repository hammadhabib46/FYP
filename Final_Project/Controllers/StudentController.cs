using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Final_Project.Controllers
{
	public class StudentController : Controller
	{
		//
		// GET: /Student/
		public ActionResult Index()
		{

       //     Session["M_ID"] = 86;    fully
       //     TempData["st_id"] = 16;   fully

            int MS_id = (int)Session["M_ID"];
            TempData.Keep("M_ID");

            int st_id = (int)TempData["st_id"];
            TempData.Keep("st_id");


            using (testdbEntiies objj = new testdbEntiies())
            {

                //var role_Id = objj.roledatas.Where((u => u.MS_iid == MS_id && u.Role_Name == name)).Select(u => u.Role_ID).FirstOrDefault();

                //int r_id = (int)role_Id;
                //TempData["Role_ID"] = r_id;




                //		var  noti_role = objj.role_funcdata.Where((u => u.Role_ID == r_id)).Select(u => u.GiveNotification).FirstOrDefault();
                //       TempData["NotiRole"] = false;



                var Data = objj.studfuctionals.SqlQuery("Select * from studfuctional where studF_ID = '" + st_id + "' ").FirstOrDefault();


                TempData["P_Name"] = Data.studF_FName + " " + Data.studF_LName;
                //    TempData["E_Mail"] = Data.email;
                TempData["Post"] = "Student";
            }

            using (testdbEntiies objj = new testdbEntiies())
            {

                var clz = objj.roledatas.SqlQuery("Select * from roledata where MS_iid ='" + MS_id + "'").ToList<roledata>();

                List<string> classList = new List<string>();

                TempData["Teach_Present"] = false;

                foreach (var x in clz)
                {
                    classList.Add(x.Role_Name);

                    if (x.Role_Name == "Teacher")
                        TempData["Teach_Present"] = true;

                }
                
            }
            
            // gather all notifications
            using (testdbEntiies objj = new testdbEntiies())
            {

                List<notification> NotiList = objj.notifications.Where(u => u.Reciever_ID == st_id).ToList<notification>();
                Session["NotiList"] = NotiList;


            }
            
            return View();
		}

        public ActionResult Notifications()
        {
            return View();
        }

        public ActionResult MarkallAsRead()
        {
            int st_id = (int)TempData["st_id"];
            TempData.Keep("st_id");

            using (testdbEntiies objj = new testdbEntiies())
            {

                List<notification> NotiList = objj.notifications.Where(u => u.Reciever_ID == st_id).ToList<notification>();

                foreach (notification n in NotiList)
                {
                    n.Status = true;
                    objj.SaveChanges();
                }

            }

            return RedirectToAction("Notifications", "Student");
        }

    }
}