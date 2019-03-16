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

		public ActionResult HRAttributes()
		{
			return View();
		}

		public ActionResult AccountantAttributes()
		{
			return View();
		}



		public ActionResult adminFunctionlities()
		{
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

							ModelState.Clear();
							return RedirectToAction("ManageClass", "Creator");
					   }
				}

			}
			return View();
		}


		// POST method for adding Class data + subjects + fee in database ( in 2 separate tables)
		[HttpPost]
		public ActionResult ManageClass(@class class_obj, string subjects)
		{

			

		//	int x= ;
			using (testdbEntiies objj = new testdbEntiies())
				{
					try
					{
				
					var usr = objj.classes.Single(u => u.Class_Name == class_obj.Class_Name);
					if (usr != null)
					{
						ModelState.AddModelError("Class_Name", "Class Already Exists");
						ModelState.Clear();
					}


					}
			
			catch (Exception ex)
			{

			//	if (ModelState.IsValid)
				{
					using (testdbEntiies obj = new testdbEntiies())
					{
						class_obj.MS_id = (int)TempData["MS_ID"];
						obj.classes.Add(class_obj);
						obj.SaveChanges();
					}

					var ClassId = objj.classes.Where(u => u.Class_Name == class_obj.Class_Name).Select(u => u.Class_ID).FirstOrDefault();   //// getting MS from database using MS_Name

					TempData["Class_ID"] = ClassId;

					ModelState.Clear();

					////////////////////////   inserting subjects in classes
					List<string> subs = subjects.Split(',').Reverse().ToList();

					subject Subb_model= new subject();
					Subb_model.Cls_id = (int)ClassId;
					


					for (int i = 0; i < subs.Count; i++)
					{
						using (testdbEntiies obj_sub = new testdbEntiies())
						{

							Subb_model.Subj_name=subs[i];
							obj_sub.subjects.Add(Subb_model);
							obj_sub.SaveChanges();
						}
					}

					//////////////////////////////


					return RedirectToAction("ManageClass", "Creator");
				}
			  }
			}

			return View();
		}

		[HttpPost]
		public ActionResult ManageRoles(bool AccountManager = false ,bool HRmanager = false)
		{

			TempData["Acc_Manager"] = AccountManager;
			TempData["HR_Manager"] = HRmanager;
			TempData["HR_Manager5"] = HRmanager;
			TempData["Hr_Portal"] = false;
			TempData["St_Portal"] = false;
			

			return RedirectToAction("StudentAttributes", "Creator");
		}

		[HttpPost]
		public ActionResult StudentAttributes(roledata obj_R ,bool DOB = false, bool Address = false, bool CNIC = false, bool Gender = false, bool pic = false, bool FatherCNIC = false, bool GuardianContact = false, bool email = false, bool StudPortal = false)
		{

			
			obj_R.MS_iid=(int)TempData["MS_ID"];
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
				var role_Id = objj.roledatas.Where((u => u.MS_iid == obj_R.MS_iid  &&  u.Role_Name == obj_R.Role_Name)).Select(u => u.Role_ID).FirstOrDefault();   //// getting MS from database using MS_Name
				TempData["Stud_Role_ID"] = (int)role_Id;
	 
			}
			
			if ((bool)TempData["Acc_Manager"] == true)
			{
			   return RedirectToAction("AccountantAttributes","Creator");


			}
			if ((bool)TempData["HR_Manager"] == true)
			{
				return RedirectToAction("HRAttributes", "Creator");
			}


			return RedirectToAction("adminFunctionlities", "Creator");
		}


		[HttpPost]
		public ActionResult AccountantAttributes(bool DOB = false, bool Address = false, bool CNIC = false, bool Gender = false, bool pic = false, bool Qualification = false, bool Portal = false)
		{
			roledata obj_R = new roledata();
			obj_R.MS_iid=(int)TempData["MS_ID"];
		//	obj_R.MS_iid = 5;
			obj_R.Role_Name = "Accountant";

			obj_R.Role_Qualif = Qualification;
			obj_R.Role_Pic = pic;
			obj_R.Role_Dob = DOB;
			obj_R.Role_Address = Address;
			obj_R.Role_Gender = Gender;
			obj_R.Role_CNIC = CNIC;



			obj_R.Role_Fname = true;
			obj_R.Role_Lname = true;
			obj_R.Role_Phone = true;
			obj_R.Role_FthrName = true;


			obj_R.Role_Portal = Portal; 

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

			TempData["Hr_Portal"] = false;
			return RedirectToAction("adminFunctionlities", "Creator");
		}


		[HttpPost]
		public ActionResult HRAttributes(bool DOB = false, bool Address = false, bool CNIC = false, bool Gender = false, bool pic = false, bool Qualification = false, bool Portal = false)
		{
			roledata obj_R = new roledata();
				obj_R.MS_iid=(int)TempData["MS_ID"];
		//	obj_R.MS_iid = 5;
			obj_R.Role_Name = "HR";

			obj_R.Role_Qualif = Qualification;
			obj_R.Role_Pic = pic;
			obj_R.Role_Dob = DOB;
			obj_R.Role_Address = Address;
			obj_R.Role_Gender = Gender;
			obj_R.Role_CNIC = CNIC;


			obj_R.Role_Portal = Portal;
			TempData["Hr_Portal"] = Portal;



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
			
			return RedirectToAction("adminFunctionlities", "Creator");
		}


		[HttpPost]
		public ActionResult adminFunctionlities(bool Nofi = false)
		{
			/////////////// Saving Admin TO Role Data
			roledata obj_R = new roledata();
			obj_R.MS_iid=(int)TempData["MS_ID"];

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



			obj_RF.Role_ID = (int)TempData["Admin_Role_ID"];

			obj_RF.GiveNotification = true;
			obj_RF.AddStudent = true;
			obj_RF.DeleteStudent = true;
			obj_RF.UpdateStudent = true;
			obj_RF.updateHR = true;

			if ((bool)TempData["HR_Manager5"] == true)
			obj_RF.updateHRpay = true;
			else
				obj_RF.updateHRpay = false;
			

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
				if ((bool)TempData["St_Portal"] == true)
				{
					return RedirectToAction("StudFunctionlities", "Creator");
				}

				if ((bool)TempData["Hr_Portal"] == true)
				{
					return RedirectToAction("HrFunctionlities", "Creator");
				}

			return RedirectToAction("Continue", "Creator");
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


			return RedirectToAction("Continue", "Creator");
		}


		[HttpPost]
		public ActionResult HrFunctionlities(bool Nofi = false)
		{
			role_funcdata obj_RF = new role_funcdata();

			obj_RF.AddStudent = true;
			obj_RF.DeleteStudent = true;
			obj_RF.UpdateStudent = true;
		  
			obj_RF.ViewNotification = Nofi;
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


			return RedirectToAction("Continue", "Creator");
		}




	}
}