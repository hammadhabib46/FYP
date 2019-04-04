using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Final_Project.Controllers
{
    public class AccountantController : Controller
    {
        // GET: Accountant
        // GET: /Student/
        public ActionResult Index()
        {

            //     Session["M_ID"] = 86;    fully
            //     TempData["st_id"] = 16;   fully

            int MS_id = (int)Session["M_ID"];
            TempData.Keep("M_ID");

            int ac_id = (int)TempData["ac_id"];
            TempData.Keep("ac_id");


            using (testdbEntiies objj = new testdbEntiies())
            {

                //var role_Id = objj.roledatas.Where((u => u.MS_iid == MS_id && u.Role_Name == name)).Select(u => u.Role_ID).FirstOrDefault();

                //int r_id = (int)role_Id;
                //TempData["Role_ID"] = r_id;




                //		var  noti_role = objj.role_funcdata.Where((u => u.Role_ID == r_id)).Select(u => u.GiveNotification).FirstOrDefault();
                //       TempData["NotiRole"] = false;



                var Data = objj.accfuctionals.SqlQuery("Select * from accfuctional where accF_id = '" + ac_id + "' ").FirstOrDefault();
                

                TempData["P_Name"] = Data.AccF_FName + " " + Data.AccF_LName;
                //    TempData["E_Mail"] = Data.email;
                TempData["Post"] = "Accountant";
            }
            
            // gather all notifications
            using (testdbEntiies objj = new testdbEntiies())
            {
                List<notification> NotiList = objj.notifications.Where(u => u.Reciever_ID == ac_id).ToList<notification>();
                Session["NotiList"] = NotiList;
            }

            return View();
        }

        public ActionResult ViewFeeRecords()
        {
            Session["ViewData"] = false;
            return View();
        }

        public ActionResult AddFee()
        {
            Session["ShowInvoice"] = false;
            return View();
        }

        public ActionResult Notifications()
        {
            return View();
        }

        public ActionResult MarkallAsRead()
        {
            int ac_id = (int)TempData["ac_id"];
            TempData.Keep("ac_id");

            using (testdbEntiies objj = new testdbEntiies())
            {

                List<notification> NotiList = objj.notifications.Where(u => u.Reciever_ID == ac_id).ToList<notification>();

                foreach (notification n in NotiList)
                {
                    n.Status = true;
                    objj.SaveChanges();
                }

            }

            return RedirectToAction("Notifications", "Accountant");
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


    }
}