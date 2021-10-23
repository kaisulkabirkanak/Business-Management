using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinessManagementSystemApp.Models
{
    public class gmail
    {
        [Required(ErrorMessage = "Please enter field!")]
        public string To { get; set; }
        [Required(ErrorMessage = "Please enter field!")]
        public string From { get; set; }
        [Required(ErrorMessage = "Please enter field!")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please enter field!")]
        public string Body { get; set; }
    }
}