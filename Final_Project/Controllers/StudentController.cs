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

            //Session["M_ID"] = 7;
            //TempData["st_id"] = 7;

            int MS_id = (int)Session["M_ID"];
            TempData.Keep("M_ID");

            int st_id = (int)TempData["st_id"];
            TempData.Keep("st_id");



            using (testdbEntiies objj = new testdbEntiies())
            {

                var role_Id = objj.roledatas.Where((u => u.MS_iid == MS_id && u.Role_Name == "Admin")).Select(u => u.Role_ID).FirstOrDefault();

                //int r_id = (int)role_Id;
                //TempData["Role_ID"] = r_id;
                
                var  noti_role = objj.role_funcdata.Where((u => u.Role_ID == role_Id)).Select(u => u.GiveNotification).FirstOrDefault();
                TempData["Noti"] = noti_role;

                string S_classid_S = objj.studfuctionals.Where(u => u.studF_ID == st_id).Select(u => u.studF_ClassName).FirstOrDefault();

                int S_classid = Int32.Parse(S_classid_S);
                Session["S_classid"] = S_classid;
                var Data = objj.studfuctionals.SqlQuery("Select * from studfuctional where studF_ID = '" + st_id + "' ").FirstOrDefault();

                Session["RoleNumber"] = Data.studF_RollNO;

                TempData["P_Name"] = Data.studF_FName + " " + Data.studF_LName;
                //    TempData["E_Mail"] = Data.email;
                TempData["Post"] = "Student";
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

                var rols = objj.roledatas.SqlQuery("Select * from roledata where MS_iid ='" + MS_id + "'").ToList<roledata>();

                List<string> classList = new List<string>();

                TempData["Teach_Present"] = false;
                /// comments are good below 

              foreach (var x in rols)
                {
                 classList.Add(x.Role_Name);
                  if (x.Role_Name == "Teacher")
                    {
                        TempData["Teach_Present"] = true;

     //                   role_funcdata rolFun = objj.role_funcdata.Where(u => u.Role_ID == x.Role_ID).FirstOrDefault();

                        var S_subjects = objj.subjects.Where(u => u.Cls_id == S_classid).ToList<subject>();   // subjects of that student
                        TempData["subjs"]= S_subjects;

   //                     if (rolFun.Marks == true)
                        {
                            TempData["Marks_Present"] = true;
                            
                        }
   //                     else
                        {
    //                        TempData["Marks_Present"] = false;
                        }

   //                     if (rolFun.Attendance == true)
                        {
                            TempData["Attendance_Present"] = true;
                        }
   //                     else
                        {
  //                          TempData["Attendance_Present"] = false;
                        }


                    }
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


        public ActionResult Marks(int data)
        {
            List<string> marks_output = new List<string>();

            int clasid = (int)Session["S_classid"];
            TempData.Keep();
            string rolnum = (string)Session["RoleNumber"];
            TempData.Keep();

            using (testdbEntiies objj = new testdbEntiies())
            {
                List<marksp> P_marksL = objj.marksps.Where(u => u.Msubj_ID == data && u.Mclas_ID == clasid).ToList<marksp>();

                foreach (marksp marP in P_marksL )
                {
                    marks_output.Add(marP.Mname);   // first
                    string date = marP.Mdate.HasValue ? marP.Mdate.Value.ToString() : string.Empty;

                    marks_output.Add(date);  // Second

                    double? totalMarks = objj.markstotals.Where(u=>u.Mforgn_ID == marP.Marks_ID).Select(u=>u.Mtotal).FirstOrDefault();

                    marks_output.Add(totalMarks.ToString());  // Thiird

                    double? obtained = objj.marksses.Where(u=> u.Mstd_ID == rolnum && u.Mrk_ID == marP.Marks_ID).Select(u=>u.Mstd_marks).FirstOrDefault();
                    marks_output.Add(obtained.ToString());   // Fourth

                }

            }
            Session["marks_output"] = marks_output;
                return View();
        }


        public ActionResult Attendence(int data)
        {
            List<string> att_output= new List<string>();

            int clasid = (int)Session["S_classid"];
            TempData.Keep();
            string rolnum = (string)Session["RoleNumber"];
            TempData.Keep();

            using (testdbEntiies objj = new testdbEntiies())
            {
                List<attendancep> all_P_att = objj.attendanceps.Where(u => u.C_IID == clasid).ToList<attendancep>();
                
                foreach (attendancep atp in all_P_att)
                {
                    string date = atp.Att_date.HasValue ? atp.Att_date.Value.ToString() : string.Empty;

                    att_output.Add(date);
                    attendance status = objj.attendances.Where(u=> u.A_ID == atp.Att_ID && u.Studnt_ID == rolnum).FirstOrDefault();
                    string sstatus = status.A_status.ToString();
                    att_output.Add(status.A_status.ToString());
                }
                
                Session["att_output"] = att_output;
            }

            return View();
        }


       
    }
}