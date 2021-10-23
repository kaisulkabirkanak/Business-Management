using BusinessManagementSystemApp.BLL.Manager;
using BusinessManagementSystemApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessManagementSystemApp.Controllers
{
    public class CustomerInfoController : Controller
    {
        CustomerManager _customerManager = new CustomerManager();
        private CustomerModel _customerModel = new CustomerModel();
        // GET: CustomerInfo save
        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(CustomerModel customerModel)
        {
            
            if (ModelState.IsValid)
            {
                if (_customerManager.Insert(customerModel))
                {
                    TempData["SuccessMessage"] = "Saved Successfully";
                    return RedirectToAction("FindAll");
                } else {
                    ViewBag.FailMsg = "Data Saved Fail!";
                } 
            } else {
                ViewBag.FailMsg = "Data Validation Fail!";
            }
           
            return View();
        }
        //customer Update
        [HttpGet]
        public ActionResult Update(int Id)
        {
            _customerModel.Id = Id;
            var customer = _customerManager.FindById(_customerModel);
            return View(customer);
        }

        [HttpPost]
        public ActionResult Update(CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                if (_customerManager.Update(customerModel))
                {
                    ViewBag.SuccessMsg = "Data Updated SuccessFully!";
                    TempData["SuccessUpdateMessage"] = "Record Updated SuccessFully!";
                    return RedirectToAction("FindAll");
                }
                else
                {
                    ViewBag.FailMsg = "Data Update Fail!";
                    TempData["SuccessMessage"] = "Data Update Fail!";
                    return RedirectToAction("FindAll");
                    //Response.Redirect("FindAll");
                }
            }
            else
            {
                ViewBag.FailMsg = "Data Validation Fail!";
                Response.Redirect("FindAll");
            }

            return View(customerModel);
        }
        //Customer Delete
        public ActionResult Delete(int Id)
        {
            _customerModel.Id = Id;
            
            if (_customerManager.Delete(_customerModel))
            {
                TempData["SuccessDeleteMessage"] = "Record Delete Successfully";
                return RedirectToAction("FindAll");
            }
            else
            {
                ViewBag.FailMsg = "Data Delete Fail!";
                Response.Redirect("FindAll");
            }
           
            return View();
        }
        //Customer Details
        [HttpGet]
        public ActionResult FindAll()
        {
            _customerModel.Customers = _customerManager.FindAll();
            return View(_customerModel);
        }
        [HttpPost]
        public ActionResult FindAll(CustomerModel customerModel)
        {
            var customers = _customerManager.FindAll();
            if (customerModel.CustName != null)
            {
                customers = customers.Where(c => c.CustName.ToLower().Contains(customerModel.CustName.ToLower())).ToList();
            }
            customerModel.Customers = customers;
            return View(customerModel);
        }
        public JsonResult IsExist(int CustCode)
        {
            var customer = _customerManager.IsExist(CustCode);

            return Json(customer, JsonRequestBehavior.AllowGet);
        }

    }

}