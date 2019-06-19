using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final_Project.Models;
using System.IO;

namespace Final_Project.Controllers
{
    public class CreatorController : Controller
    {
        public bool HR_A = false;
        public bool HR_Portal = false;
        public bool Stud_Portal = false;

        Rolevalues objectr = new Rolevalues();
        //GET: / Creator / home screen
        public ActionResult MainMenu()
        {
            return View();
        }

        //GET: / Creator / Define MS 
        public ActionResult DefineMS()
        {
            return View();
        }

        //GET: / Creator / Define Classes + add new class in MS
        public ActionResult ManageClass()
        {

            return View();
        }

        public ActionResult ChooseTheme()
        {
            return View();
        }

        //GET: / Creator / Choose Roles for MS
        public ActionResult ManageRoles()
        {
            return View();
        }

        //GET: / Creator / Choose Student Attributes for MS
        public ActionResult StudentAttributes()
        {

            return View();
        }

        public ActionResult EditMS(marksHelper chose)
        {
            int msid = 40; // not causing worry
            string x = chose.Choose;
            using (testdbEntiies objj = new testdbEntiies())
            {
                var msData = objj.ms.Where(u => u.MS_InstName == x).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                msid = msData.MS_ID;
                TempData["MS_ID"] = msid;
                Session["MS_ID"] = msid;
            }

            Editor editor = new Editor(msid);
            Session["pre-editedData"] = editor;



            return View();
        }
        public ActionResult HRAttributes()
        {
            return View();
        }

        public ActionResult editAddClasses()
        {
            return View();
        }
        public ActionResult AccountantAttributes()
        {
            return View();
        }

        public ActionResult successfullyCreated()
        {

            return View();
        }

        public ActionResult adminFunctionlities()
        {
            return View();
        }

        public ActionResult missingRoles()
        {
            int msid = 111;
            Editor editor = new Editor();
            Session["roles"] = editor.getRoles(msid);


            return View();
        }

        public ActionResult StudFunctionlities()
        {
            return View();
        }

        public ActionResult HrFunctionlities()
        {

            return View();
        }

        public ActionResult AccountantFunctionalities()
        {
            return View();
        }

        public ActionResult TeacherAttributes()
        {
            return View();
        }
        public ActionResult TeacherFucntionalities()
        {
            return View();
        }


        public ActionResult Continue()
        {
            return View();
        }
        // for adding it to model.logo ,doing conversion for blob
        public byte[] ConvertToByte(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);
            return imageByte;
        }


        // POST method for adding MS data to database 
        [HttpPost]
        public ActionResult DefineMS(m superTable_input, HttpPostedFileBase img)
        {

            superTable_input.C_ID = int.Parse(TempData["log_user"].ToString());   ///////// user_id
            TempData.Keep();


            using (testdbEntiies objj = new testdbEntiies())
            {
                try
                {
                    var usr = objj.ms.Single(u => u.MS_InstName == superTable_input.MS_InstName);   /// cheching mS_name is unique
					if (usr != null)
                    {
                        ModelState.AddModelError("MS_InstName", "Management System Already Exists");
                    }
                }
                catch (Exception ex)
                {

                    if (ModelState.IsValid)
                    {
                        using (testdbEntiies obj = new testdbEntiies())
                        {
                            HttpPostedFileBase file = Request.Files["img"];
                            superTable_input.MS_logo = ConvertToByte(file);
                            obj.ms.Add(superTable_input);
                            obj.SaveChanges();
                        }

                        var userId = objj.ms.Where(u => u.MS_InstName == superTable_input.MS_InstName).Select(u => u.MS_ID).FirstOrDefault();   //// getting MS from database using MS_Name

                        TempData["MS_ID"] = userId;
                        TempData.Keep();

                        ModelState.Clear();
                        TempData["NoClassesCheck"] = 0;
                        TempData["ClassExist"] = false;
                        return RedirectToAction("ManageClass", "Creator");
                    }
                }

            }
            return View();
        }


        // POST method for adding Class data + subjects + fee in database ( in 2 separate tables)
        [HttpPost]
        public ActionResult ManageClass(string ClassName, Nullable<double> Fee, string subjects)
        {
            int mid = (int)TempData["MS_ID"];
            TempData.Keep();
            Session["MS_ID"] = mid;
            @class class_obj = new @class();
            class_obj.Class_Fee = Fee;
            class_obj.Class_Name = ClassName;
            //	int x= ;

            using (testdbEntiies objj = new testdbEntiies())
            {
                try
                {
                    // ek anagement system ki specific id k  class name k sath compare krna ha newly entered class name 
                    var usr = objj.classes.Single(u => u.Class_Name == class_obj.Class_Name && u.MS_id == mid);
                    if (usr != null)
                    {
                        ViewBag.doublesub = "Class Already Exists";
                        return View();
                    }
                }
                catch (Exception ex)
                {

                    if (ModelState.IsValid)
                    {
                        List<string> subs = subjects.Split(',').Reverse().ToList();

                        for (int i = 0; i < subs.Count; i++)
                        {
                            for (int j = 0; j < subs.Count; j++)
                            {
                                if (subs[i] == subs[j])
                                {
                                    if (i != j)
                                    {
                                        ViewBag.doublesub = "Subjects Cannot be dublicated";
                                        return View();
                                    }
                                }
                            }
                        }


                        using (testdbEntiies obj = new testdbEntiies())
                        {
                            class_obj.MS_id = (int)TempData["MS_ID"];
                            TempData.Keep();
                            obj.classes.Add(class_obj);
                            obj.SaveChanges();
                        }

                        var ClassId = objj.classes.Where(u => u.Class_Name == class_obj.Class_Name).Select(u => u.Class_ID).FirstOrDefault();   //// getting MS from database using MS_Name

                        TempData["Class_ID"] = ClassId;

                        ////////////////////////   inserting subjects in classes


                        subject Subb_model = new subject();


                        for (int i = 0; i < subs.Count; i++)
                        {
                            using (testdbEntiies obj_sub = new testdbEntiies())
                            {
                                Subb_model = new subject();
                                Subb_model.Cls_id = (int)ClassId;

                                Subb_model.Subj_name = subs[i];
                                obj_sub.subjects.Add(Subb_model);
                                obj_sub.SaveChanges();
                            }
                        }

                        //////////////////////////////

                        ModelState.Clear();
                        TempData["NoClassesCheck"] = 1;
                        return RedirectToAction("ManageClass", "Creator");
                    }
                    else
                    {
                        int x = class_obj.Class_ID;
                        string y = class_obj.Class_Name;
                        Nullable<double> fee = class_obj.Class_Fee;

                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult ManageRoles(bool AccountManager = false, bool HRmanager = false, bool Teacher = false)
        {
            TempData["Teacher"] = Teacher;
            TempData["Acc_Manager"] = AccountManager;
            TempData["HR_Manager"] = HRmanager;

            TempData["Hr_Portal"] = AccountManager;
            TempData["St_Portal"] = false;
            TempData["Teacher_Portal"] = Teacher;
            TempData["Acc_Portal"] = AccountManager;


            return RedirectToAction("StudentAttributes", "Creator");
        }


        [HttpPost]
        public ActionResult StudentAttributes(roledata obj_R, bool DOB = false, bool Address = false, bool CNIC = false, bool Gender = false, bool pic = false, bool FatherCNIC = false, bool GuardianContact = false, bool email = false, bool StudPortal = false)
        {
            //    TempData["MS_ID"] = 73;

            obj_R.MS_iid = (int)TempData["MS_ID"];
            TempData.Keep();
            //	obj_R.MS_iid = 5;
            obj_R.Role_Name = "Student";

            obj_R.Role_Portal = StudPortal;
            TempData["St_Portal"] = StudPortal;

            obj_R.Role_Pic = pic;
            obj_R.Role_FthrPhone = GuardianContact;
            obj_R.Role_Dob = DOB;
            obj_R.Role_Address = Address;
            obj_R.Role_Gender = Gender;
            obj_R.Role_FthrCNIC = FatherCNIC;
            obj_R.Role_CNIC = CNIC;
            obj_R.Role_Email = email;



            obj_R.Role_Fname = true;
            obj_R.Role_Lname = true;
            obj_R.Role_Phone = true;
            obj_R.Role_FmailyID = true;
            obj_R.Role_FthrName = true;


            if (ModelState.IsValid)
            {
                using (testdbEntiies objt = new testdbEntiies())
                {
                    objt.roledatas.Add(obj_R);
                    objt.SaveChanges();

                }
                ModelState.Clear();
            }

            using (testdbEntiies objj = new testdbEntiies())
            {
                var role_Id = objj.roledatas.Where((u => u.MS_iid == obj_R.MS_iid && u.Role_Name == obj_R.Role_Name)).Select(u => u.Role_ID).FirstOrDefault();   //// getting MS from database using MS_Name
                TempData["Stud_Role_ID"] = (int)role_Id;
            }



            if ((bool)TempData["Acc_Manager"] == true)
            {
                return RedirectToAction("AccountantAttributes", "Creator");
            }
            if ((bool)TempData["HR_Manager"] == true)
            {
                return RedirectToAction("HRAttributes", "Creator");
            }
            if ((bool)TempData["Teacher"] == true)
            {
                return RedirectToAction("TeacherAttributes", "Creator");
            }


            return RedirectToAction("adminFunctionlities", "Creator");
        }


        [HttpPost]
        public ActionResult AccountantAttributes(bool DOB = false, bool Address = false, bool CNIC = false, bool Gender = false, bool pic = false, bool Qualification = false)
        {
            roledata obj_R = new roledata();
            obj_R.MS_iid = (int)TempData["MS_ID"];

            obj_R.Role_Name = "Accountant";

            obj_R.Role_Qualif = Qualification;
            obj_R.Role_Pic = pic;
            obj_R.Role_Dob = DOB;
            obj_R.Role_Address = Address;
            obj_R.Role_Gender = false;
            obj_R.Role_CNIC = CNIC;


            obj_R.Role_Fname = true;
            obj_R.Role_Lname = true;
            obj_R.Role_Phone = true;
            obj_R.Role_FthrName = true;

            obj_R.Role_Portal = true;

            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.roledatas.Add(obj_R);
                    obj.SaveChanges();
                }

                using (testdbEntiies objj = new testdbEntiies())
                {
                    var role_Id = objj.roledatas.Where((u => u.MS_iid == obj_R.MS_iid && u.Role_Name == obj_R.Role_Name)).Select(u => u.Role_ID).FirstOrDefault();   //// getting MS from database using MS_Name
                    TempData["Acc_Role_ID"] = (int)role_Id;
                }

                ModelState.Clear();
            }


            if ((bool)TempData["HR_Manager"] == true)
            {
                HR_A = true;
                return RedirectToAction("HRAttributes", "Creator");
            }
            if ((bool)TempData["Teacher"] == true)
            {
                return RedirectToAction("TeacherAttributes", "Creator");
            }

            return RedirectToAction("adminFunctionlities", "Creator");
        }


        [HttpPost]
        public ActionResult HRAttributes(bool DOB = false, bool Address = false, bool CNIC = false, bool Gender = false, bool Email = false, bool pic = false, bool Qualification = false)
        {
            roledata obj_R = new roledata();
            obj_R.MS_iid = (int)TempData["MS_ID"];
            //	obj_R.MS_iid = 5;
            obj_R.Role_Name = "HR";

            obj_R.Role_Qualif = Qualification;
            obj_R.Role_Pic = pic;
            obj_R.Role_Dob = DOB;
            obj_R.Role_Address = Address;
            obj_R.Role_Gender = false;
            obj_R.Role_CNIC = CNIC;
            obj_R.Role_Email = Email;


            obj_R.Role_Portal = true;
            obj_R.Role_Portal = true;


            obj_R.Role_Fname = true;
            obj_R.Role_Lname = true;
            obj_R.Role_Phone = true;
            obj_R.Role_FthrName = true;


            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.roledatas.Add(obj_R);
                    obj.SaveChanges();

                }

                ModelState.Clear();
            }


            using (testdbEntiies objj = new testdbEntiies())
            {
                var role_Id = objj.roledatas.Where((u => u.MS_iid == obj_R.MS_iid && u.Role_Name == obj_R.Role_Name)).Select(u => u.Role_ID).FirstOrDefault();   //// getting MS from database using MS_Name
                TempData["Hr_Role_ID"] = (int)role_Id;
            }

            if ((bool)TempData["Teacher"] == true)
            {
                return RedirectToAction("TeacherAttributes", "Creator");
            }
            return RedirectToAction("adminFunctionlities", "Creator");
        }

        [HttpPost]
        public ActionResult TeacherAttributes(bool DOB = false, bool Address = false, bool CNIC = false, bool Gender = false, bool pic = false, bool email = false)
        {
            roledata obj_R = new roledata();
            obj_R.MS_iid = (int)TempData["MS_ID"];
            //   	obj_R.MS_iid = 5;
            obj_R.Role_Name = "Teacher";


            obj_R.Role_Pic = pic;
            obj_R.Role_Dob = DOB;
            obj_R.Role_Address = Address;
            obj_R.Role_Gender = Gender;
            obj_R.Role_CNIC = CNIC;
            obj_R.Role_Email = email;

            obj_R.Role_Portal = true;
            TempData["Teacher_Portal"] = true;



            obj_R.Role_Fname = true;
            obj_R.Role_Lname = true;
            obj_R.Role_Phone = true;
            obj_R.Role_FthrName = true;
            obj_R.Role_Qualif = true;


            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.roledatas.Add(obj_R);
                    obj.SaveChanges();
                }

                ModelState.Clear();
            }


            using (testdbEntiies objj = new testdbEntiies())
            {
                var role_Id = objj.roledatas.Where((u => u.MS_iid == obj_R.MS_iid && u.Role_Name == obj_R.Role_Name)).Select(u => u.Role_ID).FirstOrDefault();   //// getting MS from database using MS_Name
                TempData["Teacher_Role_ID"] = (int)role_Id;
            }

            return RedirectToAction("adminFunctionlities", "Creator");
        }


        [HttpPost]
        public ActionResult adminFunctionlities(bool Nofi = false, bool ManageFee = false)
        {
            /////////////// Saving Admin TO Role Data
            roledata obj_R = new roledata();
            obj_R.MS_iid = (int)TempData["MS_ID"];

            //	obj_R.MS_iid = 5;

            obj_R.Role_Name = "Admin";


            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.roledatas.Add(obj_R);
                    obj.SaveChanges();
                }
                ModelState.Clear();
            }

            using (testdbEntiies objj = new testdbEntiies())
            {
                var role_Id = objj.roledatas.Where((u => u.MS_iid == obj_R.MS_iid && u.Role_Name == obj_R.Role_Name)).Select(u => u.Role_ID).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                TempData["Admin_Role_ID"] = role_Id;
            }
            ModelState.Clear();
            ////////////////////////////////////////
            role_funcdata obj_RF = new role_funcdata();

            obj_RF.TrackAccProgress = ManageFee;

            obj_RF.Role_ID = (int)TempData["Admin_Role_ID"];
            TempData.Keep();

            obj_RF.GiveNotification = Nofi;
            obj_RF.AddStudent = true;
            obj_RF.DeleteStudent = true;
            obj_RF.UpdateStudent = true;
            obj_RF.updateclassfee = true;



            if (ModelState.IsValid)
            {
                using (testdbEntiies objx = new testdbEntiies())
                {
                    objx.role_funcdata.Add(obj_RF);
                    objx.SaveChanges();

                }
                ModelState.Clear();
            }

            TempData["adminNoti"] = Nofi;
            TempData["adminNoti2"] = Nofi;

            //if ((bool)TempData["St_Portal"] == true)
            //{
            //    return RedirectToAction("StudFunctionlities", "Creator");
            //}
            //if ((bool)TempData["Acc_Portal"] == true)
            //{
            //    return RedirectToAction("AccFunctionlities", "Creator");
            //}
            //if ((bool)TempData["Hr_Portal"] == true)
            //{
            //    return RedirectToAction("HrFunctionlities", "Creator");
            //}
            //if ((bool)TempData["Teacher_Portal"] == true)
            //{
            //    return RedirectToAction("TeacherFucntionalities", "Creator");
            //}


            return RedirectToAction("ChooseTheme", "Creator");
        }


        [HttpPost]
        public ActionResult StudFunctionlities(bool Nofi = false)
        {
            role_funcdata obj_RF = new role_funcdata();

            obj_RF.ViewNotification = Nofi;
            obj_RF.Role_ID = (int)TempData["Stud_Role_ID"];

            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.role_funcdata.Add(obj_RF);
                    obj.SaveChanges();

                }
                ModelState.Clear();
            }

            if ((bool)TempData["Hr_Portal"] == true)
            {
                return RedirectToAction("HrFunctionlities", "Creator");
            }
            if ((bool)TempData["Teacher_Portal"] == true)
            {
                return RedirectToAction("TeacherFucntionalities", "Creator");
            }

            return RedirectToAction("Continue", "Creator");
        }


        [HttpPost]
        public ActionResult HrFunctionlities(bool addStudent = false, bool deleteStudent = false, bool addTeacher = false, bool deleteTeacher = false)
        {
            role_funcdata obj_RF = new role_funcdata();

            obj_RF.AddStudent = true;
            obj_RF.DeleteStudent = true;



            obj_RF.Role_ID = (int)TempData["Hr_Role_ID"];


            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.role_funcdata.Add(obj_RF);
                    obj.SaveChanges();

                }
                ModelState.Clear();
            }
            if ((bool)TempData["Teacher_Portal"] == true)
            {
                return RedirectToAction("TeacherFucntionalities", "Creator");
            }


            return RedirectToAction("Continue", "Creator");
        }


        [HttpPost]
        public ActionResult TeacherFucntionalities(bool M = false, bool A = false)
        {
            role_funcdata obj_RF = new role_funcdata();

            obj_RF.Marks = M;
            obj_RF.Attendance = A;

            obj_RF.Role_ID = (int)TempData["Teacher_Role_ID"];
            TempData.Keep();

            if (ModelState.IsValid)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.role_funcdata.Add(obj_RF);
                    obj.SaveChanges();
                }
                ModelState.Clear();
            }

            return RedirectToAction("Continue", "Creator");
        }

        [HttpPost]
        public ActionResult ChooseTheme(theme themeVar)
        {
            int msid = (int)Session["MS_ID"];
            themeVar.MS_primekeyid = msid;
            string checktheme = themeVar.theme1;

            if (checktheme != null)
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    obj.themes.Add(themeVar);
                    obj.SaveChanges();
                }
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("theme1", "Choose A Theme Please");
                return View();
            }

            return RedirectToAction("successfullyCreated", "Creator");
        }


        [HttpPost]
        public ActionResult EditMS(String name, String branch, string address, string number, marksHelper theme)
        {
            int msid = (int)Session["MS_ID"];
            Editor editor = new Editor(msid);
            if (name != "")
            {
                editor.updateName(name);
            }
            if (branch != "")
            {
                editor.updateBranch(branch);
            }
            if (address != "")
            {
                editor.updateAddress(address);
            }
            if (number != "")
            {
                editor.updateNumber(number);
            }
            string my = theme.Choose;
            editor.updateTheme(theme.Choose);

            int x = 0;
            return RedirectToAction("editAddClasses", "Creator");
        }

        [HttpPost]
        public ActionResult editAddClasses(string ClassName, Nullable<double> Fee, string subjects)
        {


            int mid = (int)TempData["MS_ID"];
            TempData.Keep();
            Session["MS_ID"] = mid;
            @class class_obj = new @class();
            class_obj.Class_Fee = Fee;
            class_obj.Class_Name = ClassName;
            //	int x= ;

            using (testdbEntiies objj = new testdbEntiies())
            {
                try
                {
                    // ek anagement system ki specific id k  class name k sath compare krna ha newly entered class name 
                    var usr = objj.classes.Single(u => u.Class_Name == class_obj.Class_Name && u.MS_id == mid);
                    if (usr != null)
                    {
                        ViewBag.doublesub = "Class Already Exists";
                        return View();
                    }
                }
                catch (Exception ex)
                {

                    if (ModelState.IsValid)
                    {
                        List<string> subs = subjects.Split(',').Reverse().ToList();

                        for (int i = 0; i < subs.Count; i++)
                        {
                            for (int j = 0; j < subs.Count; j++)
                            {
                                if (subs[i] == subs[j])
                                {
                                    if (i != j)
                                    {
                                        ViewBag.doublesub = "Subjects Cannot be dublicated";
                                        return View();
                                    }
                                }
                            }
                        }


                        using (testdbEntiies obj = new testdbEntiies())
                        {
                            class_obj.MS_id = (int)TempData["MS_ID"];
                            TempData.Keep();
                            obj.classes.Add(class_obj);
                            obj.SaveChanges();
                        }

                        var ClassId = objj.classes.Where(u => u.Class_Name == class_obj.Class_Name).Select(u => u.Class_ID).FirstOrDefault();   //// getting MS from database using MS_Name

                        TempData["Class_ID"] = ClassId;

                        ////////////////////////   inserting subjects in classes


                        subject Subb_model = new subject();


                        for (int i = 0; i < subs.Count; i++)
                        {
                            using (testdbEntiies obj_sub = new testdbEntiies())
                            {
                                Subb_model = new subject();
                                Subb_model.Cls_id = (int)ClassId;

                                Subb_model.Subj_name = subs[i];
                                obj_sub.subjects.Add(Subb_model);
                                obj_sub.SaveChanges();
                            }
                        }

                        //////////////////////////////

                        ModelState.Clear();
                        TempData["NoClassesCheck"] = 1;
                        return RedirectToAction("ManageClass", "Creator");
                    }
                    else
                    {
                        int x = class_obj.Class_ID;
                        string y = class_obj.Class_Name;
                        Nullable<double> fee = class_obj.Class_Fee;

                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult missingRoles(bool AccountManager = false, bool HRmanager = false, bool Teacher = false)
        {
            bool hr = false;
            bool teacher = false;
            bool acc = false;
           
            List<string> rols = (List<string>)Session["roles"];

            foreach (string st in rols)
            {
                if (st == "HR")
                { hr = true; }

                if (st == "Accountant")
                { acc = true; }

                if (st == "Teacher")
                { teacher = true; }

            }

            //Editor editor = new Editor();
            //if (hr == true && HRmanager == true)
            //{
            //    int msid = (int)Session["MS_ID"];
            //    editor.updaterole("HR",msid);
            //}

            return View();
        }

        }
}