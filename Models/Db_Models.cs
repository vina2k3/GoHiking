using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace GoHiking.Models
{
    public class Db_Models
    {
        public class MyPark
        {
            [Key]
            public string parkCode { get; set; }
            public string id { get; set; }
            public string fullName { get; set; }
            public string states { get; set; }
            public string url { get; set; }
            public string weatherInfo { get; set; }
            public string description { get; set; }
            public string latitude { get; set; }
        }
    
    }

}

