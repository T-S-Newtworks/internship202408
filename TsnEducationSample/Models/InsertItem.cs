using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TsnEducation2024.Models
{
    public class MyTodoInsertItem
    {
        public DateTime Day { get; set; }

        public DateTime Time { get; set; }

        //public int time1 { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Repeat { get; set; }

    }
}