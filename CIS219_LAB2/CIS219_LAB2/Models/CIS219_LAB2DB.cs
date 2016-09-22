using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CIS219_LAB2.Models
{
    public class CIS219_LAB2DB : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public CIS219_LAB2DB() : base("name=CIS219_LAB2DB")
        {
        }

        public System.Data.Entity.DbSet<CIS219_LAB2.Models.GamePlay> GamePlays { get; set; }
    
    }
}
