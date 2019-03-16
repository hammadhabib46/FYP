﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final_Project.Models;

namespace Final_Project.Controllers
{
	public class HomeController : Controller
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
			x = user.CreatorName;
			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{
					var usr = objj.creators.Single(u => u.email == user.CreatorName && u.c_password == user.Password);
					if (usr != null)
					{
						var userId = objj.creators.Where(u => u.email == user.CreatorName).Select(u => u.id).FirstOrDefault();

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


		//sign in user
		[HttpPost]
		public ActionResult UserLogin(CreatorLogin user)
		{
		
			string x = user.Password;
			x = user.CreatorName;// ............ ?

			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{
					var usr = objj.creators.Single(u => u.email == user.CreatorName && u.c_password == user.Password);
					if (usr != null)
					{

						var userId = objj.creators.Where(u => u.email == user.CreatorName).Select(u => u.id).FirstOrDefault();

						if ((int)TempData["Cr_ID"] == (int)userId) // checking if logging in userID = MS creatorID
						{
							return RedirectToAction("Index", "Admin");
						}
						else {
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
						var MS_Present = objj.ms.Where(b => b.MS_InstName == MS_Name.CreatorName).FirstOrDefault();
						//creatorname is used for MS name search

						if (MS_Present != null)
						{
							var creatorID = objj.ms.Where(u => u.MS_InstName == MS_Name.CreatorName).Select(u => u.C_ID).FirstOrDefault();
							
							TempData["Cr_ID"] = (int)creatorID;


							var systemID = objj.ms.Where(u => u.MS_InstName == MS_Name.CreatorName).Select(u => u.MS_ID).FirstOrDefault();

							TempData["M_ID"] = (int)systemID;


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
			x = user.CreatorName;
			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{
					var usr = objj.creators.Single(u => u.email == user.CreatorName && u.c_password == user.Password);
					if (usr != null)
					{
						var userId = objj.creators.Where(u => u.email == user.CreatorName).Select(u => u.id).FirstOrDefault();

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
			x = user.CreatorName;// ............ ?

			using (testdbEntiies objj = new testdbEntiies())
			{
				try
				{
					var usr = objj.creators.Single(u => u.email == user.CreatorName && u.c_password == user.Password);
					if (usr != null)
					{

						var userId = objj.creators.Where(u => u.email == user.CreatorName).Select(u => u.id).FirstOrDefault();

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
						var MS_Present = objj.ms.Where(b => b.MS_InstName == MS_Name.CreatorName).FirstOrDefault();
						//creatorname is used for MS name search

						if (MS_Present != null)
						{
							var creatorID = objj.ms.Where(u => u.MS_InstName == MS_Name.CreatorName).Select(u => u.C_ID).FirstOrDefault();

							TempData["Cr_ID"] = (int)creatorID;


							var systemID = objj.ms.Where(u => u.MS_InstName == MS_Name.CreatorName).Select(u => u.MS_ID).FirstOrDefault();

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