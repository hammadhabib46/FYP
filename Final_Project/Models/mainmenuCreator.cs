using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Sql;
using Final_Project.Models;

namespace Final_Project.Models
{
    public class mainmenuCreator
    {
        
        public List<string> getRecentProjects(int creatorId)
        {
            List<string> names = new List<string>();

            using (testdbEntiies objj = new testdbEntiies())
            {
                var systems = objj.ms.SqlQuery("Select * from ms where C_id ='" + creatorId + "'").ToList <m>();
                
                foreach (var x in systems)
                {
                   names.Add(x.MS_InstName);
                }
            }
            
                return names;
        }

    }
}