using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TsnEducation2024.Models
{

    public class MyTodoItem
    {
        public int Id { get; set; }  // ID プロパティを追加
        public DateTime Day { get; set; }

        public DateTime Time { get; set; }

        //public int time1 { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Result { get; set; }//bool型かも

        //public string Repeat { get; set; }
    }

    public class MyViewModel
    {
        public bool IsChecked { get; set; }
    }
}
