using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2C2P.Models;

namespace Assignment2C2P.Services
{
    public class DBService
    {
        public AssignmentContext db { get; set; } 
        public string conn { get; set; }
        public DBService()
        {
            db = new AssignmentContext();
            conn = db.Database.GetDbConnection().ConnectionString;
        }
    }
}
