using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Final_Project.Models;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace Final_Project.Models
{
    public class HomeDatabaseConnections
    {
        //public bool managementSystemPresent { get; set; }
        //public int creatorId { get; set; }
        //public int MsId { get; set; }
        //private string Encrypt(string clearText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}

        //private string Decrypt(string cipherText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";

        //    int len = cipherText.Length;

        //    //          cipherText = cipherText.Replace(" ", "+");
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}

        //public void Sendcode()
        //{
        //    creator s = (creator)Session["signupdata"];
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = "smtp.gmail.com";
        //    smtp.Port = 587;
        //    smtp.Credentials = new System.Net.NetworkCredential("f158034@nu.edu.pk", "64>JyKS=");
        //    smtp.EnableSsl = true;
        //    MailMessage msg = new MailMessage();
        //    msg.Subject = "NEXUM - Creator Email Verification Code";
        //    msg.Body = "Dear " + s.firstname + " " + s.lastname + "," + " Your Email Verification Code is " + activationcode + "\n\n\nThank You for being a Creator at NEXUM !";
        //    String toaddress = (String)s.email;
        //    msg.To.Add(toaddress);
        //    String fromaddress = "NEXUM <f158034@nu.edu.pk>";
        //    msg.From = new MailAddress(fromaddress);
        //    try
        //    {
        //        smtp.Send(msg);
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //}

        //public bool SelectMS(CreatorLogin MS_Name)
        //{

        //    using (testdbEntiies objj = new testdbEntiies())
        //    {
        //        try
        //        {
        //            {
        //                var MS_Present = objj.ms.Where(b => b.MS_InstName == MS_Name.Name).FirstOrDefault();
        //                //creatorname is used for MS name search

        //                if (MS_Present != null)
        //                {
        //                    managementSystemPresent = true;
        //                    var creatorID = objj.ms.Where(u => u.MS_InstName == MS_Name.Name).Select(u => u.C_ID).FirstOrDefault();

        //                    creatorId = (int)creatorID;


        //                    var systemID = objj.ms.Where(u => u.MS_InstName == MS_Name.Name).Select(u => u.MS_ID).FirstOrDefault();

        //                    MsId = systemID;

        //                    return true;
                            
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
                   
        //        }

        //    }




        //    return View();
        //}

    }
}