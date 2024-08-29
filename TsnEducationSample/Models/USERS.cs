namespace TsnEducation2024.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [Key]
        [StringLength(10)]
        public string ID { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(100)]
        public string LOGIN_PASSWORD { get; set; }

        [StringLength(100)]
        [DisplayName("メールアドレス")]
        public string EMAIL { get; set; }

        [DisplayName("生年月日")]
        public DateTime? BIRTHDAY { get; set; }

        [StringLength(2000)]
        public string REMARKS { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_DELETE { get; set; }

        [StringLength(10)]
        public string INSERT_ID { get; set; }

        public DateTime? INSERT_DATE { get; set; }

        [StringLength(10)]
        public string UPDATE_ID { get; set; }

        public DateTime? UPDATE_DATE { get; set; }
    }
}
