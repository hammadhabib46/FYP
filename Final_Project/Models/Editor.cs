using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Final_Project.Models;

namespace Final_Project.Models
{
    public class Editor
    {
        public int Choose { set; get; }
        public int MsId { set; get; }
        public int CreatorId { set; get; }
        public string MsName { set; get; }
        public string Address { set; get; }
        public string BranchName { set; get; }
        public string PhoneNumber { set; get; }
        public string theme { set; get; }
        public int totalClasses { set; get; }
        public List<string> ClassesIds { set; get; }
        public List<string> ClassesNames { set; get; }
        public List<string> RolesMissing { set; get; }

        public Editor()
        {

        }

        public Editor(int msid)
        {
            MsId = msid;
            using (testdbEntiies objj = new testdbEntiies())
            {
                var msData = objj.ms.Where(u => u.MS_ID == MsId ).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                MsName = msData.MS_InstName;
                BranchName = msData.MS_InstBranch;
                PhoneNumber = msData.MS_InstPhone;
                Address = msData.MS_InstAddress;
                ////////////////// Baiic Info Gathering Ending
                var themeData = objj.themes.Where(u => u.MS_primekeyid == MsId).FirstOrDefault();
                theme = themeData.theme1;
                ///////////////// Themes Data gathered
                //List<@class> classData = objj.classes.Where(u => u.MS_id == MsId).ToList<@class>();

                //int count = classData.Count;
                
                //foreach (@class item in classData)
                //{
                //    ClassesNames.Add(item.Class_Name);
                    
                //}
                //totalClasses = count;
                //////////////////////// Classes Data Gathered
            }

        }

        public void updateName(string newName)
        {
            using (testdbEntiies objj = new testdbEntiies())
            {
                var msData = objj.ms.Where(u => u.MS_ID == MsId).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                msData.MS_InstName = newName;
                objj.SaveChanges();
            }
        }
        public void updateBranch(string newBrach)
        {
            using (testdbEntiies objj = new testdbEntiies())
            {
                var msData = objj.ms.Where(u => u.MS_ID == MsId).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                msData.MS_InstBranch = newBrach;
                objj.SaveChanges();
            }
        }
        public void updateAddress(string newAddress)
        {
            using (testdbEntiies objj = new testdbEntiies())
            {
                var msData = objj.ms.Where(u => u.MS_ID == MsId).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                msData.MS_InstAddress = newAddress;
                objj.SaveChanges();
            }
        }
        public void updateNumber(string newNumber)
        {
            using (testdbEntiies objj = new testdbEntiies())
            {
                var msData = objj.ms.Where(u => u.MS_ID == MsId).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                msData.MS_InstPhone = newNumber;
                objj.SaveChanges();
            }
        }

        public void updateTheme(string Theme)
        {
            using (testdbEntiies objj = new testdbEntiies())
            {
                var msData = objj.themes.Where(u => u.MS_primekeyid == MsId).FirstOrDefault();   //// getting RoleID from database(ms) using Role name + ms id
                msData.theme1 = Theme;
                objj.SaveChanges();
            }
        }

        public List<string> getRoles(int msid)
        {
            bool accountantCheck = true;
            bool teacherCheck = true;
            bool hrCheck = true;

            List<string> roles = new List<string>();
            using (testdbEntiies objj = new testdbEntiies())
            {
                List<roledata>  msData = objj.roledatas.Where((u => u.MS_iid == msid)).ToList<roledata>();   //// getting RoleID from database(ms) using Role name + ms id
                foreach (roledata rd in msData)
                {
                   
                    if (rd.Role_Name == "Accountant")
                    {
                        accountantCheck = false;
                    }
                    if (rd.Role_Name == "HR")
                    {
                        hrCheck = false;
                    }
                    if (rd.Role_Name == "Teacher")
                    {
                        teacherCheck = false;
                    }
                }

                if (accountantCheck == true)
                {
                    roles.Add("Accountant");
                }
                if (hrCheck == true)
                {
                    roles.Add("HR");
                }
                if (teacherCheck == true)
                {
                    roles.Add("Teacher");
                }

            }


            return roles;
        }
    }
}