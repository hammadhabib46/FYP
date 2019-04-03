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
    public class NotificationModel
    {
        //student
        public bool sendNotifications(List<int> classids, string notificationText,int senderid)
        {
            notification noti_obj = new notification();
            List<int>  stud_id;
            string con_clas_id = "";
            TimeSpan ti = DateTime.UtcNow.TimeOfDay;
            try
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    foreach (int classid in classids)
                    {
                        noti_obj = new notification();
                        con_clas_id = classid.ToString();
                        stud_id = obj.studfuctionals.Where(u => u.studF_ClassName == con_clas_id).Select(u => u.studF_ID).ToList<int>();


                        foreach (int id in stud_id)
                        {
                            /// adding objects to the notifcation table
                            noti_obj.Message = notificationText;
                            noti_obj.Sender_ID = senderid;
                            noti_obj.Reciever_ID = id;
                            noti_obj.Time = ti;
                            noti_obj.Status = false;
                            noti_obj.type = "Student"; ;
                            obj.notifications.Add(noti_obj);
                            obj.SaveChanges();
                        }
                    }
                }
            }
            catch {
                return false;
            }
            return true;

        }

        //teacher
        public bool sendNotificationsTeacher(List<int> teachersid, string notificationText, int senderid)
        {
            notification noti_obj = new notification();
            TimeSpan ti = DateTime.UtcNow.TimeOfDay;
            DateTime da = DateTime.Now;
            try
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                        foreach (int id in teachersid)
                        {
                            /// adding objects to the notifcation table
                            noti_obj.Message = notificationText;
                            noti_obj.Sender_ID = senderid;
                            noti_obj.Reciever_ID = id;
                            noti_obj.Time = ti;
                            noti_obj.Status = false;
                            noti_obj.type = "Teacher";
                            noti_obj.Date = da;

                        obj.notifications.Add(noti_obj);
                            obj.SaveChanges();
                        }
                    
                }
            }
            catch
            {
                return false;
            }
            return true;

        }

        // hr
            public bool sendNotificationsHr(int Hrid, string notificationText, int senderid)
        {
            notification noti_obj = new notification();
            TimeSpan ti = DateTime.UtcNow.TimeOfDay;
            DateTime da = DateTime.Now;
            try
            {
                using (testdbEntiies obj = new testdbEntiies())
                {
                    
                    {
                        /// adding objects to the notifcation table
                        noti_obj.Message = notificationText;
                        noti_obj.Sender_ID = senderid;
                        noti_obj.Reciever_ID = Hrid;
                        noti_obj.Time = ti;
                        noti_obj.Status = false;
                        noti_obj.type = "Hr";
                        noti_obj.Date = da;

                        obj.notifications.Add(noti_obj);
                        obj.SaveChanges();
                    }

                }
            }
            catch
            {
                return false;
            }
            return true;

        }


    }
}