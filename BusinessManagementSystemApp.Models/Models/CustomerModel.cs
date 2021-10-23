using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManagementSystemApp.Models.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        [Display(Name = "Customer Code")]
        [Required(ErrorMessage = "Please enter Code!")]
        public int CustCode { get; set; }
        [Required(ErrorMessage = "Please enter Name!")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Customer Name")]
        public string CustName { get; set; }
        [Required(ErrorMessage = "Please enter E-mail!")]
        [RegularExpression("^[a-z0-9_\\.-]+@([a-z0-9-]+\\.)+[a-z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Customer E-mail")]
        public string CustEmail { get; set; }

        [Required(ErrorMessage = "Please enter Address!")]
        [Display(Name = "Customer Address")]
        public string CustAddress { get; set; }

        [Required(ErrorMessage = "Please enter Contact Number!")]
        //[StringLength(14, MinimumLength = 6)]
        [Display(Name = "Customer Mobile")]
        public int CustContact { get; set; }
        [Display(Name = "Loyalty Points")]
        public int CustLoyaltyPoints { get; set; }
   
        [NotMapped]
        public List<CustomerModel> Customers { get; set; }
        [NotMapped]
        public List<SalesDetails> SalesSummary { get; set; }
    }
}