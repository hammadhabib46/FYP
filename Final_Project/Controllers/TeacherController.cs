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
    public class TeacherController : Controller
    {
        DateTime now = DateTime.Now;

        // GET: Teacher
        public ActionResult Index()
        {
         //   Session["M_ID"] = 5;
            ////////////////////// up is the testing area attributes
            //string name = "Teacher";
            int MS_id = (int)Session["M_ID"];

       //     TempData["TchrF_ID"] = 1; // will get it dynamically while teacher login at 'home controller'
            Session["TID"] = TempData["TchrF_ID"];
            TempData.Keep("TchrF_ID");
            int T_id = (int)TempData["TchrF_ID"];
            TempData.Keep("TchrF_ID");

            // getting User name To view
            using (testdbEntiies objj = new testdbEntiies())
            {

                //var Trole_Id = objj.roledatas.Where((u => u.MS_iid == MS_id && u.Role_Name == name)).Select(u => u.Role_ID).FirstOrDefault();

                //int r_id = (int)Trole_Id;
                //TempData["TRole_ID"] = r_id;


                var Data = objj.tchrfunctionals.SqlQuery("Select * from tchrfunctional where TchrF_ID = '" + T_id + "' ").FirstOrDefault();

                TempData["T_Name"] = Data.TchrF_FName + " " + Data.TchrF_LName;
                // TempData["T_EMail"] = Data.studF_Email;
                TempData["T_Post"] = "Teacher";
            }

            // getting list of classes
            // getting subj name and class id from subjects via subject id
            using (testdbEntiies objt = new testdbEntiies())
            {
                TempData["subjBool"] = false; //...................?????

                try
                {
                    var subs = objt.tchrfsubjects.SqlQuery("Select * from tchrfsubjects where Tchrr_ID ='" + T_id + "'").ToList<@tchrfsubject>();
                    Session["Tsubs"] = subs;
                    List<String> classdatashow = new List<String>();
                    List<int> classdataidshow = new List<int>();
                    List<int> classdatasubidshow = new List<int>();
                    foreach (var x in subs)
                    {
                        var subnam = objt.subjects.SqlQuery("Select * from subjects where Subj_ID ='" + x.Subjj_ID + "'").FirstOrDefault<@subject>();
                        var clasnam = objt.classes.SqlQuery("Select * from classes where Class_ID ='" + subnam.Cls_id + "'").FirstOrDefault<@class>();
                        classdatashow.Add(clasnam.Class_Name + " [ " + subnam.Subj_name + " ]");
                        classdataidshow.Add(clasnam.Class_ID);
                        classdatasubidshow.Add(subnam.Subj_ID);
                        TempData["ClassBool"] = true;

                    }

                    Session["tchrdata"] = classdatashow;
                    Session["tchrdataid"] = classdataidshow;
                    Session["tchrdatasubiid"] = classdatasubidshow;
                }
                catch (Exception ex)
                {
                    TempData["subjBool"] = false;
                }

            }


            // gather all notifs
            // gather all notifications
            using (testdbEntiies objj = new testdbEntiies())
            {

                List<notification> NotiList = objj.notifications.Where(u => u.Reciever_ID == T_id).ToList<notification>();
                Session["NotiList"] = NotiList;
            }
            return View();
        }
        public ActionResult Attendance(int? data, int? datt, String dattt)
        {

            Session["classIDT"] = data;
            Session["subIDT"] = datt;
            Session["ClassSub_Name"] = dattt;
            // find the attribute2s of the students in  current maangement system to show in the coloumn name

            ////getting rows and col data

            return View();
        }
        public ActionResult Marks(int? data, int? datt, String dattt)
        {

            Session["MclassIDT"] = data;
            Session["MsubIDT"] = datt;
            Session["MClassSub_Name"] = dattt;
            return View();
        }
        public ActionResult Notifications()
        {

            return View();
        }

        public ActionResult MarkallAsRead()
        {
            int st_id = (int)Session["TID"];
           
            using (testdbEntiies objj = new testdbEntiies())
            {
                List<notification> NotiList = objj.notifications.Where(u => u.Reciever_ID == st_id).ToList<notification>();

                foreach (notification n in NotiList)
                {
                    n.Status = true;
                    objj.SaveChanges();
                }
            }
            return RedirectToAction("Notifications", "Teacher");
        }

        public ActionResult ViewSubjects()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Attendance(DateTime? dateofAtt, checkHelper objlist)
        {
            if ((Boolean)Session["maintaintwoforms"] == false)
            {
                int f = (int)Session["TID"];
                int g = (int)Session["subIDT"];
                int h = (int)Session["classIDT"];
                Session["f"] = f;
                Session["g"] = g;
                Session["h"] = h;
                Session["dateatt"] = dateofAtt;
                using (testdbEntiies objj = new testdbEntiies())
                {
                    var attcheck = objj.attendanceps.Where(u => u.T_ID == f && u.S_IID == g && u.C_IID == h && u.Att_date == dateofAtt).Select(u => u.Att_ID).FirstOrDefault();

                    Session["prevAtt"] = attcheck;
                    if ((int)Session["prevAtt"] == 0)// attandce sheet pely sy nai ha
                    {
                        //new att sheet 
                        int msss = (int)Session["M_ID"];
                        string stdffff = Session["classIDT"].ToString();


                        //--------------------start
                        // getting roll no and student name from database for new attendnce sheet
                        var StudData = objj.studfuctionals.SqlQuery("Select * from studfuctional where studF_MSID = '" + msss + "' AND studf_Classname = '" + stdffff + "' ").ToList<studfuctional>();

                        List<attendance> stAttlist = new List<attendance>();



                        foreach (var x in StudData)
                        {
                            attendance o = new attendance();
                            o.Studnt_NAME = x.studF_FName + " " + x.studF_LName;
                            o.Studnt_ID = x.studF_RollNO;
                            o.A_status = false;
                            stAttlist.Add(o);
                        }



                        // add Stud data got from StudData above to attendncsecondry obj. named atobjj by using list etc

                        // new attendnce record in session 
                        Session["studdataa"] = stAttlist;
                        //-----------------------------------------------------------------------------------------------

                        attendancep atobj = new attendancep();

                        atobj.T_ID = (int)Session["TID"];
                        atobj.S_IID = (int)Session["subIDT"];
                        atobj.C_IID = (int)Session["classIDT"];
                        atobj.Att_date = dateofAtt;

                        if (ModelState.IsValid)
                        {
                            using (testdbEntiies objjw = new testdbEntiies())
                            {
                                objjw.attendanceps.Add(atobj);
                                objjw.SaveChanges();
                            }
                            ModelState.Clear();

                        }
                        //------------------------------------attndcne primary --> data jara ha new record ka ------------
                    }
                    else
                    {
                        //-------------------------------------------------got previous record for attndnce 


                        //attendancep obje = new attendancep();
                        //obje = (attendancep)Session["prevAtt"];
                        //int i = obje.Att_ID;


                        // getting attendance data from attendance secondary table , already present in database
                        var StudDataAttPrev = objj.attendances.SqlQuery("Select * from Attendances where A_ID = '" + (int)Session["prevAtt"] + "' ").ToList<attendance>();

                        Session["studdataa"] = StudDataAttPrev;

                        //------------------------------previous attndce record in session



                    }

                }
                Session["maintaintwoforms"] = true;
                return RedirectToAction("Attendance", "Teacher");


            }//-----------------------------------------------------------------------------------------------------------------
            if ((Boolean)Session["maintaintwoforms"] == true)
            {
                List<attendance> todblst = new List<attendance>(); int iide;
                int ff = (int)Session["f"]; //teacher id  
                int gg = (int)Session["g"]; // subject id 
                int hh = (int)Session["h"]; // class id 
                DateTime datee = (DateTime)Session["dateatt"]; // date


                //getting attndnceP table id to enter data along with it in attndnceS table
                using (testdbEntiies obji = new testdbEntiies())
                {
                    var alreadyinid = obji.attendanceps.Where(u => u.T_ID == ff && u.S_IID == gg && u.C_IID == hh && u.Att_date == datee).Select(u => u.Att_ID).FirstOrDefault();
                    iide = (int)alreadyinid;
                }
                List<Acheck> ll = objlist.checklist;

                //----------recieved data from 1 below
                todblst = (List<attendance>)Session["studdataa"];
                for (int a = 0, b = 0; a < ll.Count && b < todblst.Count; a++, b++)
                {
                    todblst[b].A_status = ll[a].checkvalue;
                    todblst[b].A_ID = iide;
                }


                using (testdbEntiies intodb = new testdbEntiies())
                {
                    List<attendance> updatetable = intodb.attendances.Where(u => u.A_ID == iide).ToList<attendance>();

                    if ((int)Session["prevAtt"] != 0)
                    {
                        for (int i = 0; i < updatetable.Count; i++)
                        {
                            updatetable[i].A_status = todblst[i].A_status;

                            //    intodb.attendances.Add(itemm);   //// should replace
                            intodb.SaveChanges();

                        }
                    }

                    if ((int)Session["prevAtt"] == 0)
                    {

                        foreach (attendance atc in todblst)
                        {

                            intodb.attendances.Add(atc);   //// should add
                            intodb.SaveChanges();

                        }

                    }
                }


                Session["maintaintwoforms"] = false;
                return RedirectToAction("Attendance", "Teacher");


            }

            return View();
        }
        [HttpPost]
        public ActionResult Marks(DateTime? dateofAssesmnt, string typeofmarks, marksHelper objMhelpr, Double? totalMs)
        {

            if ((Boolean)Session["Mrkmaintaintwoforms"] == false)
            {
                string Mnam = typeofmarks;
                Session["Mrktypename"] = Mnam;
                //M-->marks 
                int Mf = (int)Session["TID"];
                int Mg = (int)Session["MsubIDT"];
                int Mh = (int)Session["MclassIDT"];
                Session["mrkf"] = Mf;
                Session["mrkg"] = Mg;
                Session["mrkh"] = Mh;
                Session["mrkdat"] = dateofAssesmnt;
                // mrkg ---> subj id , mrkf ----> tchr id, mrkh ---> classid

                Session["dateatt"] = (DateTime)dateofAssesmnt;

                //string Mnam = mrktypenam;
                //Session["Mrktypename"] = Mnam;


                using (testdbEntiies objj = new testdbEntiies())
                {
                    var markcheck = objj.marksps.Where(u => u.Mtchr_ID == Mf && u.Msubj_ID == Mg && u.Mclas_ID == Mh && u.Mname == Mnam && u.Mdate == dateofAssesmnt).Select(u => u.Marks_ID).FirstOrDefault();

                    Session["prevMarks"] = markcheck;
                    if ((int)Session["prevMarks"] == 0)
                    {
                        int Mrksms = (int)Session["M_ID"];
                        string Mrkstd = Session["MclassIDT"].ToString();
                        var MarksStudData = objj.studfuctionals.SqlQuery("Select * from studfuctional where studF_MSID = '" + Mrksms + "' AND studf_Classname = '" + Mrkstd + "' ").ToList<studfuctional>();

                        List<markss> Mstlist = new List<markss>();
                        foreach (var x in MarksStudData)
                        {
                            markss m = new markss();
                            m.Mstd_ID = x.studF_RollNO;
                            m.Mstd_NAME = x.studF_FName + " " + x.studF_LName;
                            m.Mstd_marks = null;
                            Mstlist.Add(m);
                        }



                        // add Stud data got from StudData above to attendncsecondry obj. named atobjj by using list etc

                        Session["studMarksdataa"] = Mstlist;

                        marksp Mrkobj = new marksp();
                        Mrkobj.Mtchr_ID = (int)Session["mrkf"];
                        Mrkobj.Msubj_ID = (int)Session["mrkg"];
                        Mrkobj.Mclas_ID = (int)Session["mrkh"];
                        Mrkobj.Mname = (string)Session["Mrktypename"];
                        Mrkobj.Mdate = (DateTime)Session["dateatt"];



                        if (ModelState.IsValid)
                        {
                            using (testdbEntiies mrkob = new testdbEntiies())
                            {
                                mrkob.marksps.Add(Mrkobj);
                                mrkob.SaveChanges();
                            }
                            ModelState.Clear();

                        }

                    }
                    else
                    {
                        // getting attendance data from attendance secondary table , already present in database
                        var StudDataMarksPrev = objj.marksses.SqlQuery("Select * from markss where Mrk_ID = '" + (int)Session["prevMarks"] + "' ").ToList<markss>();

                        Session["studMarksdataa"] = StudDataMarksPrev;

                        using (testdbEntiies tbe = new testdbEntiies())
                        {
                            int tamiz = (int)Session["prevMarks"];
                            var mrmrktot = tbe.markstotals.Where(u => u.Mforgn_ID == tamiz).Select(u => u.Mtotal).FirstOrDefault();
                            Session["mmttotl"] = (double)mrmrktot;
                        }


                    }

                }
                Session["Mrkmaintaintwoforms"] = true;
                return RedirectToAction("Marks", "Teacher");

            }
            if ((Boolean)Session["Mrkmaintaintwoforms"] == true)
            {
                List<markss> mtodblst = new List<markss>();
                int mid;
                int Mrktid = (int)Session["mrkf"];
                int submrk = (int)Session["mrkg"];
                int mrkclass = (int)Session["mrkh"];
                DateTime dtmobbj = (DateTime)Session["mrkdat"];

                using (testdbEntiies obji = new testdbEntiies())
                {
                    var alreadyinmarks = obji.marksps.Where(u => u.Mtchr_ID == Mrktid && u.Msubj_ID == submrk && u.Mclas_ID == mrkclass && u.Mdate == dtmobbj).Select(u => u.Marks_ID).FirstOrDefault();
                    mid = (int)alreadyinmarks;
                    Session["PrevMarkidd"] = mid;

                }
                List<Mobtained> llm = objMhelpr.mrksList;

                mtodblst = (List<markss>)Session["studMarksdataa"];
                for (int a = 0, b = 0; a < llm.Count && b < mtodblst.Count; a++, b++)
                {
                    mtodblst[b].Mstd_marks = llm[a].obtainedMarks;
                    mtodblst[b].Mrk_ID = mid;
                }

                foreach (markss mar in mtodblst)
                {
                    if (mar.Mstd_marks > totalMs)
                    {
                        ViewBag.MarksError = "Obtained Marks Should be Less than Total Marks";
                        return View();
                    }
                }

                

                using (testdbEntiies Mintodb = new testdbEntiies())
                {

                    if ((int)Session["prevMarks"] == 0)
                    {
                        foreach (var Mitem in mtodblst)
                        {
                            Mintodb.marksses.Add(Mitem);

                            Mintodb.SaveChanges();

                            ModelState.Clear();
                        }
                    }
                    if ((int)Session["prevMarks"] != 0)
                    {
                        List<markss> updateMarks = Mintodb.marksses.Where(u => u.Mrk_ID == mid).ToList<markss>();
                        for (int i = 0; i < updateMarks.Count; i++)
                        {
                            updateMarks[i].Mstd_marks = mtodblst[i].Mstd_marks;
                            Mintodb.SaveChanges();

                        }
                        
                    }
                }
                
                markstotal objecct = new markstotal();
                objecct.Mforgn_ID = (int)Session["PrevMarkidd"];
                objecct.Mtotal = totalMs;



                using (testdbEntiies testdbobj = new testdbEntiies())
                {

                    if ((int)Session["prevMarks"] == 0)
                    {
                        testdbobj.markstotals.Add(objecct);

                        testdbobj.SaveChanges();

                        ModelState.Clear();

                    }
                    if ((int)Session["prevMarks"] != 0)
                    {
                        markstotal markx = testdbobj.markstotals.Where(u => u.Mforgn_ID == objecct.Mforgn_ID).FirstOrDefault();

                        markx.Mtotal = objecct.Mtotal;
                        testdbobj.SaveChanges();
                        
                    }
                    
                }
                
                Session["Mrkmaintaintwoforms"] = false;
                return RedirectToAction("Marks", "Teacher");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Notifications(int x)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ViewSubjects(int x)
        {
            return View();
        }


    }
}