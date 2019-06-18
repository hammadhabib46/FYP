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
           
            ////////////////////// up is the testing area attributes
            string name = "Admin";
            int MS_id = (int)Session["M_ID"];

            
            
            int creatorid = (int)TempData["Cr_ID"];
            TempData.Keep("Cr_ID");
            Session["Cr_IDD"] = creatorid;

            Session["nofi"] = true;

            // getting User name To view
            using (testdbEntiies objj = new testdbEntiies())
            {

                var role_Id = objj.roledatas.Where((u => u.MS_iid == MS_id && u.Role_Name == name)).Select(u => u.Role_ID).FirstOrDefault();

                int r_id = (int)role_Id;
                TempData["Role_ID"] = r_id;




                //		var  noti_role = objj.role_funcdata.Where((u => u.Role_ID == r_id)).Select(u => u.GiveNotification).FirstOrDefault();
                //       TempData["NotiRole"] = false;



                var Data = objj.creators.SqlQuery("Select * from creator where id = '" + creatorid + "' ").FirstOrDefault<creator>();


                TempData["P_Name"] = Data.firstname + Data.lastname;
                TempData["E_Mail"] = Data.email;
                TempData["Post"] = "Admin";
            }

            // Getting either the System has tacher//  or Hr or not   /// adn putting the teachers in a list
            using (testdbEntiies objj = new testdbEntiies())
            {

                var clz = objj.roledatas.SqlQuery("Select * from roledata where MS_iid ='" + MS_id + "'").ToList<roledata>();

                List<string> classList = new List<string>();

                TempData["Teach_Present"] = false;
                TempData["HR_Present"] = false;
                TempData["Acc_Present"] = true;   // testing

                foreach (var x in clz)
                {
                    classList.Add(x.Role_Name);

                    if (x.Role_Name == "Teacher")
                    {
                        TempData["Teach_Present"] = true;
                        List<tchrfunctional> tch = objj.tchrfunctionals.SqlQuery("Select * from tchrfunctional where TchrF_MSID ='" + MS_id + "'").ToList<tchrfunctional>();
                        Session["All_Teachers"] = tch;

                    }

                    if (x.Role_Name == "HR")
                    {
                        TempData["HR_Present"] = true;
                        hrfuctional  hr = objj.hrfuctionals.SqlQuery("Select * from hrfuctional where HrF_MsID ='" + MS_id + "'").FirstOrDefault();
                        Session["All_Hr"] = hr;
                    }

                    if (x.Role_Name == "Acc")
                    {
                        TempData["Acc_Present"] = true;
                        accfuctional ac = objj.accfuctionals.SqlQuery("Select * from accfuctional where AccF_id ='" + MS_id + "'").FirstOrDefault();
                        Session["All_Ac"] = ac;
                    }
                }
            }

            // getting list of classes and teachers
            using (testdbEntiies objj = new testdbEntiies())
            {
                try
                {
                    var clz = objj.classes.SqlQuery("Select * from classes where MS_id ='" + MS_id + "'").ToList<@class>();

                    Session["clz"] = clz;
                    
                }
                catch (Exception ex)
                {
                    TempData["ClassBool"] = false;
                }

            }

            // getting either the system has fee fucntionality or not
            using (testdbEntiies objj = new testdbEntiies())
            {

                try
                {
                    var role_Id = objj.role_funcdata.Where(u => u.Role_ID == creatorid).Select(u => u.TrackAccProgress).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                    Session["fee"] = true;
                }
                catch (Exception ex)
                {
                    Session["fee"] = false;
                }
            }
            return View();
        }

        public ActionResult AddStudent()
        {
            //int MS_d = 72;
            //int creatorid = 21;
            //int rollid = 180;
            ////////////////////up is the testing data
            int MS_d = (int)Session["M_ID"];

            int creatorid = (int)TempData["Cr_ID"];
            //      TempData.Keep();
            int roll_id = (int)TempData["Role_ID"];
            //       TempData.Keep();
            string st = "Student";
            //////////////////////////////////////////////////

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
                catch (Exception ex)
                {
                    TempData["ClassBool"] = false;
                }

                var Data = objj.roledatas.SqlQuery("Select * from roledata where MS_iid = '" + MS_d + "' AND Role_Name = '" + st + "' ").FirstOrDefault<roledata>();

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
                TempData.Keep();

            }
            return View();
        }

        public ActionResult AddTeacher()
        {
            ////////////////////up is the testing data
            int MS_d = (int)Session["M_ID"];

            int creatorid = (int)TempData["Cr_ID"];
            TempData.Keep("Cr_ID");
            int roll_id = (int)TempData["Role_ID"];
            TempData.Keep("Role_ID");
            string st = "Teacher";
            //////////////////////////////////////////////////
            ///
            using (testdbEntiies objj = new testdbEntiies())
            {

                var Data = objj.roledatas.SqlQuery("Select * from roledata where MS_iid = '" + MS_d + "' AND Role_Name = '" + st + "' ").FirstOrDefault<roledata>();

                TempData["Sdon"] = Data.Role_Dob;
                TempData["Saddress"] = Data.Role_Address;
                TempData["Scnic"] = Data.Role_CNIC;
                TempData["Sgender"] = Data.Role_Gender;
                TempData["Spic"] = Data.Role_Pic;
                TempData["SEmail"] = Data.Role_Email;

                TempData["Tportal"] = Data.Role_Portal;
                TempData.Keep();

            }
            return View();
        }

        public ActionResult AddHR()
        {
            ////////////////////up is the testing data
            int MS_d = (int)Session["M_ID"];

            int creatorid = (int)TempData["Cr_ID"];
            TempData.Keep("Cr_ID");
            int roll_id = (int)TempData["Role_ID"];
            TempData.Keep("Role_ID");
            string st = "HR";
            //////////////////////////////////////////////////
            ///
            using (testdbEntiies objj = new testdbEntiies())
            {

                var Data = objj.roledatas.SqlQuery("Select * from roledata where MS_iid = '" + MS_d + "' AND Role_Name = '" + st + "' ").FirstOrDefault<roledata>();

                Data.Role_Email = true;

                TempData["Hdon"] = Data.Role_Dob;
                TempData["Haddress"] = Data.Role_Address;
                TempData["Hcnic"] = Data.Role_CNIC;
                TempData["Hgender"] = Data.Role_Gender;
                TempData["Hpic"] = Data.Role_Pic;
                TempData["HEmail"] = Data.Role_Email;
                TempData["Hqualif"] = Data.Role_Qualif;

                TempData["Hportal"] = Data.Role_Portal;
                TempData.Keep();

            }
            return View();
        }

        public ActionResult AddAccountant()
        {
            ////////////////////up is the testing data
            int MS_d = (int)Session["M_ID"];

            int creatorid = (int)TempData["Cr_ID"];
            TempData.Keep("Cr_ID");
            int roll_id = (int)TempData["Role_ID"];
            TempData.Keep("Role_ID");
            string st = "Accountant";
            //////////////////////////////////////////////////
            ///
            using (testdbEntiies objj = new testdbEntiies())
            {

                var Data = objj.roledatas.SqlQuery("Select * from roledata where MS_iid = '" + MS_d + "' AND Role_Name = '" + st + "' ").FirstOrDefault<roledata>();

                Data.Role_Email = true;

                TempData["Adon"] = Data.Role_Dob;
                TempData["Aaddress"] = Data.Role_Address;
                TempData["Acnic"] = Data.Role_CNIC;
                TempData["Agender"] = Data.Role_Gender;
                TempData["Apic"] = Data.Role_Pic;
                TempData["AEmail"] = Data.Role_Email;
                TempData["Aqualif"] = Data.Role_Qualif;

                TempData["Aportal"] = Data.Role_Portal;
                TempData.Keep();

            }
            return View();
        }

        
        public ActionResult AddFee()
        {
            Session["ShowInvoice"] = false;
            return View();
        }

        public ActionResult AssignSubjects()
        {
            int mid = (int)Session["M_ID"];
            using (testdbEntiies objj = new testdbEntiies())
            {

                try
                {
                    List<tchrfunctional> allteach = objj.tchrfunctionals.Where(u => u.TchrF_MSID == mid).ToList<tchrfunctional>();    //// getting RoleID from database(ms) using Role name + ms id
                    Session["allTeach"] = allteach;
                  
                }
                catch (Exception ex)
                {
                    
                }

            }
            
            return View();
        }

      
        public ActionResult AssignSubjectx(int s_id)
        {
            int t_id=(int)Session["Tcch_id"];
            tchrfsubject ob = new tchrfsubject();
            ob.Tchrr_ID = t_id;

            ob.Subjj_ID = s_id;

            using (testdbEntiies objj = new testdbEntiies())
            {
                objj.tchrfsubjects.Add(ob);
                objj.SaveChanges();
            }

            ViewBag.subA = "Subject Assigned Successfully";


            return RedirectToAction("AssignSubjects", "Admin");
        }


        public ActionResult AssignS_Classes(int t_id)
        {
            int MS_id = (int)Session["M_ID"];
            Session["Tcch_id"] = t_id;

            using (testdbEntiies objj = new testdbEntiies())
            {
                
                    var clz = objj.classes.SqlQuery("Select * from classes where MS_id ='" + MS_id + "'").ToList<@class>();

                    Session["clz"] = clz;

             }


            return View();
        }
        

        public ActionResult AssignS_Subjects(int c_id)
        {
            using (testdbEntiies objj = new testdbEntiies())
            {
                List<subject> subs = objj.subjects.Where(u=> u.Cls_id == c_id).ToList<subject>();
                Session["subss"] = subs;
            }
                return View();
        }

        public ActionResult ClassDetails(int data)
        {
            Session["Class_Name"] = data;
            // find the attributes of the students in  current maangement system to show in the coloumn name


            using (testdbEntiies objj = new testdbEntiies())
            {
                var StudData = objj.studfuctionals.SqlQuery("Select * from studfuctional where studf_Classname = '" + data + "' ").ToList<studfuctional>();
                Session["studdata"] = StudData;

                string st = "Student";
                var ColData = objj.roledatas.SqlQuery("Select * from roledata where MS_iid = '" + (int)Session["M_ID"] + "' AND Role_Name = '" + st + "' ").FirstOrDefault<roledata>();

                Session["coldata"] = ColData;

                TempData.Keep();
            }

            return View();
        }

        public ActionResult DeleteStudent()
        {

            return View();
        }
        
        public ActionResult Notification_Teacher()
        {

            return View();
        }


        public ActionResult Notification_Accountant()
        {
            return View();
        }

        public ActionResult ViewFeeRecords()
        {
            Session["ViewData"] = false;
            return View();
        }

        public ActionResult Notification()
        {
            return View();
        }


        public ActionResult Notification_Hr()
        {

            return View();
        }

        public ActionResult DeleteTeacher()
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
            int MS_d = (int)Session["M_ID"];
            TempData.Keep();

            //     int MS_d = 72;
            string rol;
            //         if ((bool)TempData["Sportal"] == true) ////////////////  if it will be true Student Portal will be created
            {
                int random;

                //////////// random generation of student id
                bool num = false;
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
                        catch (Exception e)
                        {
                            num = true;
                        }

                    }

                } while (num == false);

                ///////////////////////////random generation of student id
                stu_add.studF_MSID = MS_d;
                stu_add.studF_RollNO = rol;
                stu_add.studF_password = rol;
            }

            /////////////sending Class ID to Table instead of Class Name

            using (testdbEntiies objj = new testdbEntiies())
            {
                var Data = objj.classes.SqlQuery("Select * from classes where ms_id = '" + MS_d + "' and Class_Name = '" + stu_add.studF_ClassName + "' ").FirstOrDefault<@class>();
                stu_add.studF_ClassName = (string)Data.Class_ID.ToString();
            }

            stu_add.studF_PendingFee = 0;
            ///

           if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.studfuctionals.Add(stu_add);
                    obj.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.msg1 = "Successfully Added! User Name :" + rol + "Password is " + rol;
            }
            return View();
        }

        [HttpPost]
        public ActionResult DeleteStudent(string Roll_number)
        {
           int MS_id = (int)Session["M_ID"];
         
            TempData.Keep();
            bool del = false;
            using (testdbEntiies objj = new testdbEntiies())
            {

                try
                {
                    var usr = objj.studfuctionals.First(u => u.studF_RollNO == Roll_number && u.studF_MSID == MS_id);
                    if (usr != null)
                    {
                        del = true;
                        var Data = objj.studfuctionals.SqlQuery("Delete from studfuctional where studF_RollNO = '" + Roll_number + "' ").First<studfuctional>();
                        return View();
                    }

                }
                catch (Exception ex)
                {
                    if (del == true)
                        ViewBag.deleteStudent = "SuccessFully Deleted";
                    else
                        ViewBag.deleteStudent = "Deletion Unsuccessful";
                }

            }


            return View();
        }

        [HttpPost]
        public ActionResult AddTeacher(tchrfunctional tch_add)
        {
            int MS_d = (int)Session["M_ID"];


            //   int MS_d = 72;

            if ((bool)TempData["Tportal"] == true) ////////////////  if it will be true Teacher Portal will be created
            {
                int random;
                string rol;
                //////////// random generation of student id
                bool num = false;
                do
                {
                    random = GenerateRandomNo();
                    rol = random.ToString();
                    rol = "T_" + rol;
                    using (testdbEntiies obj = new testdbEntiies())
                    {
                        try
                        {
                            var usr = obj.tchrfunctionals.Single(u => u.TchrF_RollID == rol);
                        }
                        catch (Exception e)
                        {
                            num = true;
                        }
                    }

                } while (num == false);

                ///////////////////////////random generation of student id
                tch_add.TchrF_MSID = MS_d;
                tch_add.TchrF_RollID = rol;
                tch_add.TchrF_MSID = MS_d;
                tch_add.TchrF_password = rol;
            }

            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.tchrfunctionals.Add(tch_add);
                    obj.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.msg1 = "Successfully Added " + " User Number = " + tch_add.TchrF_RollID + "  " + " User Password = " + tch_add.TchrF_password + "  ";

            }
            return View();
        }

        [HttpPost]
        public ActionResult DeleteTeacher(string Roll_number)
        {
              int MS_id = (int)Session["M_ID"];
          
            TempData.Keep();
            bool del = false;
            using (testdbEntiies objj = new testdbEntiies())
            {
                try
                {
                    var usr = objj.tchrfunctionals.First(u => u.TchrF_RollID == Roll_number && u.TchrF_MSID == MS_id);
                    if (usr != null)
                    {
                        del = true;
                        var Data = objj.tchrfunctionals.SqlQuery("Delete from tchrfunctional where TchrF_RollID  = '" + Roll_number + "' ").First<tchrfunctional>();
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    if (del == true)
                        ViewBag.deleteStudent = "SuccessFully Deleted";
                    else
                        ViewBag.deleteStudent = "Deletion Unsuccessful";
                }

            }


            return View();
        }

        [HttpPost]
        public ActionResult AddHR(hrfuctional hrdata)
        {
            int MS_d = (int)Session["M_ID"];


            //     int MS_d = 72;

            if ((bool)TempData["Hportal"] == true) ////////////////  if it will be true Teacher Portal will be created
            {
                int random;
                string rol;
                //////////// random generation of student id
                bool num = false;
                do
                {
                    random = GenerateRandomNo();
                    rol = random.ToString();
                    rol = "H_" + rol;
                    using (testdbEntiies obj = new testdbEntiies())
                    {
                        try
                        {
                            var usr = obj.hrfuctionals.Single(u => u.HrF_userNumber == rol);
                        }
                        catch (Exception e)
                        {
                            num = true;
                        }
                    }

                } while (num == false);

                ///////////////////////////random generation of student id
                hrdata.HrF_MsID = MS_d;
                hrdata.HrF_userNumber = rol;

                if ((bool)TempData["Hportal"] == true)
                    hrdata.HrF_password = rol;
            }

            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.hrfuctionals.Add(hrdata);
                    obj.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.msg1 = "Successfully Added "+" User Number = "+ hrdata.HrF_userNumber +"  " + " User Password = " + hrdata.HrF_userNumber + "  ";
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult AddFee(manageFee Fee_obj)
        {
            int msid = (int)Session["M_ID"];
            /////////// checking is the record of current month is not put in the Table && putting it
            if (Fee_obj.RecordUpdated((int)Session["M_ID"]) == true)
            {
                List<string> Sdata = Fee_obj.UpdateStudentFee(Fee_obj, (int)Session["M_ID"]);
                if (Sdata[0] == "Success")
                {
                    Session["ShowInvoice"] = true;

                    using (testdbEntiies objx = new testdbEntiies())
                    {
                        var msData = objx.ms.Where(u => u.MS_ID == msid).FirstOrDefault();
                        Sdata.Add(msData.MS_InstName);
                        Sdata.Add(msData.MS_InstAddress);
                        Sdata.Add(msData.MS_InstPhone);
                        Sdata.Add(msData.MS_InstBranch);
                        Sdata.Add(DateTime.Now.ToString("MMMM dd yy"));
                        Sdata.Add(Fee_obj.rollnumber);
                    }


                    Session["DataInvoice"] = Sdata;

                }
                else if (Sdata[0] == "RollNumberError")
                {
                    ModelState.AddModelError("rollnumber", "ERROR DUE TO ROLL NUMBER MISMATCH");
                }
                else if (Sdata[0] == "AlreadySubmitedFee")
                {
                    ModelState.AddModelError("rollnumber", "ERROR DUE TO SUbmitted Fee");
                }
                else if (Sdata[0] == "Wrong Date")
                {
                    ModelState.AddModelError("rollnumber", "ERROR DUE TO Wrong Date");
                }

            }



            return View();
        }

        [HttpPost]
        public ActionResult ViewFeeRecords(manageFee Fee_obj)
        {
            int ms_id = (int)Session["M_ID"];
            Session["ViewData"] = true;
            Session["FeeRecords"] = Fee_obj.viewFeeData(Fee_obj, ms_id);

            return View();
        }


        [HttpPost]
        public ActionResult Notification(teacherSaverCheck objj, string notification_text)
        {
            List<@class> classData = (List<@class>)Session["clz"];   // all classes of that managment system
            bool Cvalue = false;
            List<int> classlist = new List<int>();
            
            List<check> obj = objj.checklist; /// checklist items
            
            
            for(int i=0; i< obj.Count; i++)
            {
                Cvalue = obj[i].checkvalue;
                if (Cvalue == true)
                {
                    classlist.Add(classData[i].Class_ID);
                    
                }
                Cvalue = false;
            }

            if (classlist.Count == 0)
            {
                ViewBag.noti_S = "Sorry Cant Send Please Select any Class ";
            }
            else
            {

                int userid = (int)Session["Cr_IDD"];
                NotificationModel nf = new NotificationModel();
                bool resutl = nf.sendNotifications(classlist, notification_text, userid);

                if (resutl == true)
                {
                    ViewBag.noti_S = "Successfully Sent Notification";
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult Notification_Teacher(teacherSaverCheck objj,string notification_text)
        {
            List<tchrfunctional> tch = new List<tchrfunctional>();
            int MS_id = (int)Session["M_ID"];
            using (testdbEntiies objx = new testdbEntiies())
            {
                tch = objx.tchrfunctionals.SqlQuery("Select * from tchrfunctional where TchrF_MSID ='" + MS_id + "'").ToList<tchrfunctional>();
            }
            List<tchrfunctional> tchrList = tch;
            bool Cvalue = false;
            List<int> tid_list = new List<int>();

            List<check> obj = objj.checklist; /// checklist items


            for (int i = 0; i < obj.Count; i++)
            {
                Cvalue = obj[i].checkvalue;
                if (Cvalue == true)
                {
                    tid_list.Add(tchrList[i].TchrF_ID);

                }
                Cvalue = false;
            }


            if(tid_list.Count == 0)
            {
                ViewBag.noti_T = "Sorry Cant Send Please Select any Teacher ";
            }
            else
            {

                int userid = (int)Session["Cr_IDD"];
                NotificationModel nf = new NotificationModel();
                bool resutl = nf.sendNotificationsTeacher(tid_list, notification_text, userid);

                if (resutl == true)
                {
                    ViewBag.noti_T = "Successfully Sent Notification";
                }
            }



            return View();
        }

        [HttpPost]
        public ActionResult Notification_Hr(string notification_text)
        {
            hrfuctional hr = new hrfuctional();
            int MS_id = (int)Session["M_ID"];
            using (testdbEntiies objx = new testdbEntiies())
            {
                hr = objx.hrfuctionals.SqlQuery("Select * from hrfuctional where HrF_MsID ='" + MS_id + "'").FirstOrDefault();
            }
            hrfuctional hrdata = hr;
            
                int userid = (int)Session["Cr_IDD"];
                NotificationModel nf = new NotificationModel();
                bool resutl = nf.sendNotificationsHr(hrdata.HrF_ID, notification_text, userid);

                if (resutl == true)
                {
                    ViewBag.noti_H = "Successfully Sent Notification";
                }
        



            return View();
        }


        [HttpPost]
        public ActionResult AddAccountant(accfuctional acdata)
        {
            int MS_d = (int)Session["M_ID"];


            //     int MS_d = 72;

            if ((bool)TempData["Aportal"] == true) ////////////////  if it will be true Teacher Portal will be created
            {
                int random;
                string rol;
                //////////// random generation of student id
                bool num = false;
                do
                {
                    random = GenerateRandomNo();
                    rol = random.ToString();
                    rol = "A_" + rol;
                    using (testdbEntiies obj = new testdbEntiies())
                    {
                        try
                        {
                            var usr = obj.accfuctionals.Single(u => u.AccF_userNumber == rol);
                        }
                        catch (Exception e)
                        {
                            num = true;
                        }
                    }

                } while (num == false);

                ///////////////////////////random generation of student id
                acdata.AccF_MsID = MS_d;
                acdata.AccF_userNumber = rol;

                acdata.AccF_password = rol;
            }

            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.accfuctionals.Add(acdata);
                    obj.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.msg1 = "Successfully Added " + " User Number = " + acdata.AccF_userNumber + "  " + " User Password = " + acdata.AccF_userNumber + "  ";
            }
            return View();
        }


        [HttpPost]
        public ActionResult Notification_Accountant(string notification_text)
        {
            accfuctional ac = new accfuctional();
            int MS_id = (int)Session["M_ID"];
            using (testdbEntiies objx = new testdbEntiies())
            {
                 ac = objx.accfuctionals.SqlQuery("Select * from accfuctional where AccF_id ='" + MS_id + "'").FirstOrDefault();
            }
            
            int userid = (int)Session["Cr_IDD"];
            NotificationModel nf = new NotificationModel();
            bool resutl = nf.sendNotificationsAcc(ac.AccF_ID, notification_text, userid);

            if (resutl == true)
            {
                ViewBag.noti_A = "Successfully Sent Notification";
            }




            return View();
        }


    }
}