using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Final_Project.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Mvc;

namespace Final_Project.Models
{
    
    public class manageFee
    {
        public class ViewData
        {
            public int id;
            public string rollnumber;
            public string name;
            public string lastname;
            public string classname;
            public string feestatus;
            public double? pendingfee;
        }
       

        public string month { get; set; }

        public int year { get; set; }

        public string rollnumber { get; set; }

        // checking either the records are updated or not
        public bool RecordUpdated(int ms_id)
        {
            DateTime mytime;
            using (testdbEntiies objj = new testdbEntiies())
            {
                try
                {
                    var data = objj.studfees.Where(u => u.FeeMs_ID == ms_id).ToList();

                    foreach (var feedata in data)
                    {
                       mytime = (DateTime)feedata.Fee_date;
                       if (mytime.Month == DateTime.Now.Month && mytime.Year == DateTime.Now.Year)
                       {
                            return true;
                       }
                    }
                    UpdateFeeTable(ms_id);
                    return true; // there is management system but no updated current month

                }
                catch (Exception ex)
                {
                    UpdateFeeTable(ms_id);
                    return true; // managemnet system is new
                }
                // in both of the cases the system will update fee table with all studetns fee for curretn month --- UpdateFeeTable
            }
        }

        public int MonthToInt(string monthName)
        {
            switch (monthName)
            {
                case "Jan":
                    return 1;
                    break;
                case "Feb":
                    return 2;
                    break;
                case "Mar":
                    return 3;
                    break;
                case "Apr":
                    return 4;
                    break;
                case "May":
                    return 5;
                    break;
                case "Jun":
                    return 6;
                    break;
                case "Jul":
                    return 7;
                    break;
                case "Aug":
                    return 8;
                    break;
                case "Sep":
                    return 9;
                    break;
                case "Oct":
                    return 10;
                    break;
                case "Nov":
                    return 11;
                    break;
                case "Dec":
                    return 12;
                    break;

                default:
                    return 0;
                    break;
            }

        }

        public List<studfuctional> ListOfStudents(int ms_id)
        {
            using (testdbEntiies objj = new testdbEntiies())
            {

                var std = objj.studfuctionals.SqlQuery("Select * from studfuctional where studF_msid ='" + ms_id + "'").ToList<studfuctional>();
                return std;
            }
            
        }

        public void UpdateFeeTable(int ms_id)
        {
            int studClas;
            double? classFee;

            studfee Fee_obj = new studfee();
            Fee_obj.FeeMs_ID = ms_id;
            Fee_obj.Fee_amount = 0;
            Fee_obj.Fee_date = DateTime.Now;
            Fee_obj.Fee_status = false;
            List<studfuctional> students_ids = ListOfStudents(ms_id);
            
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    foreach(var st in students_ids)
                    {
                        Fee_obj.FeeStud_ID = st.studF_ID;
                        studClas = Int32.Parse(st.studF_ClassName);

                        classFee = obj.classes.Where(u => u.MS_id == ms_id && u.Class_ID == studClas).Select(u => u.Class_Fee).FirstOrDefault();
                        
                        using (testdbEntiies objj = new testdbEntiies())
                        {
                            var Student = objj.studfuctionals.Where(u => u.studF_MSID == ms_id && u.studF_ID == st.studF_ID).FirstOrDefault();
                            Student.studF_PendingFee = Student.studF_PendingFee + classFee;
                            objj.SaveChanges();
                        }


                        obj.studfees.Add(Fee_obj);
                        obj.SaveChanges();
                    }
                    
                   
                }
            
            }


        }


        public List<string> UpdateStudentFee(manageFee obj_mF,int ms_id )
        {
            bool wrongdate = false;
            DateTime mytime;
            List<string> outputdata = new List<string>();

            using (testdbEntiies objj = new testdbEntiies())
            {

               try
                {
                    var data = objj.studfees.Where(u => u.FeeMs_ID == ms_id).ToList();

                    foreach (var feedata in data)
                    {
                        mytime = (DateTime)feedata.Fee_date;
                        if (mytime.Month == MonthToInt(obj_mF.month) && mytime.Year == obj_mF.year)
                        {
                            try
                            {
                                using (testdbEntiies obj = new testdbEntiies())
                                {
                                    //getting student classdi and studentID
                                    var stud_data = obj.studfuctionals.Where(u => u.studF_MSID == ms_id && u.studF_RollNO == obj_mF.rollnumber).FirstOrDefault();
                                    if (stud_data != null)
                                    {
                                        int calsID = Int32.Parse(stud_data.studF_ClassName);
                                        //getting classfee From ClassId
                                        var classFee = obj.classes.Where(u => u.MS_id == ms_id && u.Class_ID == calsID).FirstOrDefault();
                                        if (stud_data == null)
                                        {
                                            outputdata.Add("RollNumberError");
                                            return outputdata;
                                        }
                                        if (feedata.Fee_status == true && feedata.FeeStud_ID == stud_data.studF_ID)
                                        {
                                            outputdata.Add("AlreadySubmitedFee");
                                            return outputdata;
                                        }
                                        if (feedata.FeeStud_ID == stud_data.studF_ID)
                                        {
                                            feedata.FeeMs_ID = ms_id;
                                            feedata.Fee_date = DateTime.Now;
                                            feedata.Fee_amount = classFee.Class_Fee;
                                            feedata.FeeStud_ID = stud_data.studF_ID;
                                            feedata.Fee_status = true;
                                            outputdata.Add("Success");
                                            outputdata.Add(stud_data.studF_FName);
                                            outputdata.Add(stud_data.studF_LName);
                                            outputdata.Add(classFee.Class_Name);
                                            outputdata.Add(feedata.Fee_amount.ToString());
                                            outputdata.Add(stud_data.studF_PendingFee.ToString());
                                            using (testdbEntiies objx = new testdbEntiies())
                                            {
                                                var Student = objx.studfuctionals.Where(u => u.studF_MSID == ms_id && u.studF_ID == stud_data.studF_ID).FirstOrDefault();
                                                Student.studF_PendingFee = Student.studF_PendingFee - classFee.Class_Fee;
                                                objx.SaveChanges();
                                            }

                                            objj.SaveChanges();
                                            // removing From Pending Fee
                                            return outputdata;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                outputdata.Add("RollNumberError");
                                return outputdata;
                            }
                        }
                        else
                        {
                            //   outputdata.Add("Wrong Date");
                            //  return outputdata;
                            wrongdate = true;

                        }
                    }
                 /////////////
                }
                catch (Exception ex)
                {
                    outputdata.Add("MsidError");
                    return outputdata;
                }
            }
            outputdata.Add("Wrong Date");
            return outputdata;
        }

        public List<ViewData> viewFeeData(manageFee obj_F,int ms_id)
        {
            DateTime mytime;
            List<ViewData> output = new List<ViewData>();
            

            List<studfuctional> SList = ListOfStudents(ms_id);
            
            foreach(var Sdata in SList)
            {
                ViewData dataobj = new ViewData();
                dataobj.name = Sdata.studF_FName + Sdata.studF_LName;
                dataobj.pendingfee = Sdata.studF_PendingFee;
                dataobj.rollnumber = Sdata.studF_RollNO;
                
                using (testdbEntiies objj = new testdbEntiies())
                {
                    // geting Fee status
                    var data = objj.studfees.Where(u => u.FeeMs_ID == ms_id && u.FeeStud_ID == Sdata.studF_ID).ToList();
                    foreach (var feedata in data)
                    {
                        mytime = (DateTime)feedata.Fee_date;
                        if (mytime.Month == MonthToInt(obj_F.month) && mytime.Year == obj_F.year)
                        {
                            if (feedata.Fee_status == true)
                            {
                                dataobj.feestatus = "Paid";
                            }
                            else {
                                dataobj.feestatus = "Unpaid";
                            }
                        }
                    }
                   
                    //getting ClassName
                    int CLs = Int32.Parse(Sdata.studF_ClassName);
                    var classData = objj.classes.Where(u => u.MS_id == ms_id && u.Class_ID == CLs).FirstOrDefault();
                    dataobj.classname = classData.Class_Name;

                  
                }
                output.Add(dataobj);
            }


            return output;

        }
    }
}