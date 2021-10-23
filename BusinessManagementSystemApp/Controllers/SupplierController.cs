using AutoMapper;
using BusinessManagementSystemApp.BLL.BLL;
using BusinessManagementSystemApp.BLL.Manager;
using BusinessManagementSystemApp.Models;
using BusinessManagementSystemApp.Models.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BusinessManagementSystemApp.Controllers
{
    public class SupplierController : Controller
    {
        SupplierManager _supplierManager = new SupplierManager();
        SupplierVM _supplierVM = new SupplierVM();
        private Supplier _supplier = new Supplier();
        string _fileName = string.Empty;
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        // GET: Suppliers save
        [HttpPost]
        public ActionResult Add(SupplierVM supplierVM, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                _fileName = Path.GetFileNameWithoutExtension(image.FileName);
                string extension = Path.GetExtension(image.FileName);
                _fileName = _fileName + DateTime.Now.ToString("yymmssfff") + extension;
                supplierVM.Image = "~/images/SupplierImage/" + _fileName;
                var supplier = Mapper.Map<Supplier>(supplierVM);
                if (_supplierManager.Add(supplier))
                {
                    _fileName = Path.Combine(Server.MapPath("~/images/SupplierImage/"), _fileName);
                    image.SaveAs(_fileName);
                    ViewBag.SuccessMsg = "Saved";
                    TempData["SuccessMessage"] = "Saved Successfully";
                    return RedirectToAction("Show");
                }
                else
                {
                    ViewBag.FailMsg = "Save Failed!!";
                }
            }
            else
            {
                ViewBag.FailMsg = "An error occurred, please try again later!";
            }

            return View(supplierVM);
        }
        //Customer Delete
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            _supplier.Id = Id;
            var supplier = _supplierManager.GetByID(_supplier);
            _supplierVM = Mapper.Map<SupplierVM>(supplier);
            return View(_supplierVM);
        }
        [HttpPost]
        public ActionResult Delete(SupplierVM supplierVM)
        {

            if (ModelState.IsValid)
            {
                var supplier = Mapper.Map<Supplier>(supplierVM);
                if (_supplierManager.Delete(supplier))
                {
                    TempData["SuccessDeleteMessage"] = "Record Delete Successfully";
                    return RedirectToAction("Show");
                }
                else
                {
                    ViewBag.FailMsg = "Failed";
                }

            }
            else
            {
                ViewBag.FailMsg = "Validation Error";
            }

            return View(supplierVM);
        }
        //Customer Update
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            _supplier.Id = Id;
            var supplier = _supplierManager.GetByID(_supplier);
            _supplierVM = Mapper.Map<SupplierVM>(supplier);
            return View(_supplierVM);
        }
        [HttpPost]
        public ActionResult Edit(SupplierVM supplierVM, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var supplier = Mapper.Map<Supplier>(supplierVM);
                _fileName = Path.GetFileNameWithoutExtension(image.FileName);
                string extension = Path.GetExtension(image.FileName);
                _fileName = _fileName + DateTime.Now.ToString("yymmssfff") + extension;
                supplierVM.Image = "~/images/SupplierImage/" + _fileName;
                _fileName = Path.Combine(Server.MapPath("~/images/SupplierImage/"), _fileName);
                image.SaveAs(_fileName);
                if (_supplierManager.Update(supplier))
                {

                    ViewBag.SuccessMsg = "Updated";
                    TempData["SuccessUpdateMessage"] = "Record Updated SuccessFully!";
                    return RedirectToAction("Show");


                }
                else
                {
                    ViewBag.FailMsg = "Failed";
                }
            }
            else
            {
                ViewBag.FailMsg = "Validation Error";
            }

            return View(supplierVM);
        }
        //Customer Details
        [HttpGet]
        public ActionResult Details(int Id)
        {
             _supplier.Id = Id;
            var supplier = _supplierManager.GetByID(_supplier);
            _supplierVM = Mapper.Map<SupplierVM>(supplier);
            return View(_supplierVM);
        }
        [HttpPost]
        public ActionResult Details(SupplierVM supplierVM, HttpPostedFileBase image)
        {
            Supplier supplier = new Supplier();
            supplier = Mapper.Map<Supplier>(supplierVM);
            var suppliers = _supplierManager.GetAll();

            return View(supplierVM);
        }
        [HttpGet]
        public ActionResult Show()
        {
            _supplierVM.Suppliers = _supplierManager.GetAll();
            return View(_supplierVM);
        }
        [HttpPost]
        public ActionResult Show(SupplierVM supplierVM )
        {
            Supplier supplier = new Supplier();
            supplier = Mapper.Map<Supplier>(supplierVM);
            var suppliers = _supplierManager.GetAll();

            if (supplierVM.Name != null)
            {

                suppliers = suppliers.Where(c => c.Name.ToLower().Contains(supplierVM.Name.ToLower())).ToList();
            }

            supplierVM.Suppliers = suppliers;
            return View(supplierVM);
        }
        public JsonResult IsExistSupplierName(string supplierCode)
        {
            bool isExist = false;
            Supplier supplier = new Supplier();
            supplier.Code = supplierCode;
            isExist = _supplierManager.IsExistSupplier(supplier);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}