using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessManagementSystemApp.Models;
using System.Net;
using System.Net.Mail;

namespace BusinessManagementSystemApp.Controllers
{
    public class EmailSetupController : Controller
    {
        // GET: EmailSetup
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(BusinessManagementSystemApp.Models.gmail model)
        {
            MailMessage mm = new MailMessage("hasan.authorize@gmail.com", model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 993;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("hasan.authorize@gmail.com", "kanak243kabir@gmail.com");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
            ViewBag.Message = "Mail has been sent Successfully";

            return View();
        }
    }
}