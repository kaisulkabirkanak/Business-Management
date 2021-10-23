using AutoMapper;
using BusinessManagementSystemApp.BLL.BLL;
using BusinessManagementSystemApp.BLL.Manager;
using BusinessManagementSystemApp.Models;
using BusinessManagementSystemApp.Models.Models;
using Rotativa;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessManagementSystemApp.Controllers
{
    public class SalesController : Controller
    {
        SalesManager _salesManager = new SalesManager();
        private SalesDetails _salesDetails = new SalesDetails();
        private Sale sale = new Sale();
        CustomerManager _customerManager = new CustomerManager();
        ProductManager _productManager = new ProductManager();
        // GET: Sales
        [HttpGet]
        public ActionResult Save()
        {
            SalesSaveViewModel salesModelVm = new SalesSaveViewModel();
            salesModelVm.CustomerList = _customerManager.FindAll().Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.CustName
            });
            return View(salesModelVm);
        }
        [HttpPost]
        public ActionResult Save(SalesSaveViewModel salesModelVm)
        {
            if (ModelState.IsValid)
            {
                SalesDetails salesModel = new SalesDetails();
                salesModel = Mapper.Map<SalesDetails>(salesModelVm);
                if (_salesManager.Save(salesModel))
                {
                    ViewBag.SuccessMsg = "Data Saved SuccessFully!";
                }
                else
                {
                    ViewBag.FailMsg = "Data Saved Fail!";
                }
            }
            else
            {
                ViewBag.FailMsg = "Data Validtion Fail!";
            }
            salesModelVm.CustomerList = _customerManager.FindAll().Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.CustName
            });
            return View(salesModelVm);
        }
        public ActionResult Delete(Sale sale)
        {
            sale.Id = sale.Id;

            if (_salesManager.Delete(sale))
            {
                ViewBag.SuccessMsg = "Data Delete SuccessFully!";
                TempData["SuccessMessage"] = "Data Delete SuccessFully!";
                return RedirectToAction("FindAll");
            }
            else
            {
                ViewBag.FailMsg = "Data Delete Fail!";
                TempData["SuccessMessage"] = "Data Delete Fail!";
                return RedirectToAction("FindAll");
            }

            return View();
        }
        [HttpGet]
        public ActionResult FindAll(SalesSaveViewModel salesSaveViewModel)
        {
            SalesSaveViewModel salesSaveVM = new SalesSaveViewModel();

            salesSaveVM.SalesList = _salesManager.FindAll();

            salesSaveVM.CustomerList = _customerManager.FindAll()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.CustName
                });
            return View(salesSaveVM);
        }
        [HttpPost]
        public ActionResult FindAll(CustomerModel customerModel)
        {
            SalesSaveViewModel salesSaveVM = new SalesSaveViewModel();
            var customers = _customerManager.FindAll();
            if (customerModel.CustName != null)
            {
                customers = customers.Where(c => c.CustName.ToLower().Contains(customerModel.CustName.ToLower())).ToList();
            }

            foreach (var v in customers)
            {
                customerModel.Id = v.Id;
            }
            var salesList = _salesManager.FindAll();
            if (customerModel.Id != null)
            {
                salesList = salesList.Where(c => c.CustomerModelsId == customerModel.Id).ToList();
            }

            salesSaveVM.SalesList = salesList;

            salesSaveVM.CustomerList = _customerManager.FindAll()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.CustName
                });
            return View(salesSaveVM);
        }

        public ActionResult BatchSalesAdd()
        {
            var customers = _customerManager.FindAll();
            ViewBag.Customers = new SelectList(customers, "Id", "CustName");

            var products = _productManager.GetProducts();
            ViewBag.Products = new SelectList(products, "ProductId", "ProductName");

            return View();
        }
        
        //Loyality point
        [HttpPost]
        public ActionResult BatchSalesAdd(SalesSaveViewModel Model)
        {
            try
            {
                sale.CustomerModelsId = Model.CustomerModelsId;
                int loyaltyPoint = (Convert.ToInt32(Model.CustomerPayment) / 1000);
                CustomerModel customer = new CustomerModel();
                customer.Id = sale.CustomerModelsId;
                var aCustomer = _customerManager.FindById(customer);
                if (Model.Discount > 0)
                {
                    aCustomer.CustLoyaltyPoints = aCustomer.CustLoyaltyPoints - (Convert.ToInt32(Model.Discount * 10));
                }
                else
                {
                    aCustomer.CustLoyaltyPoints = aCustomer.CustLoyaltyPoints + loyaltyPoint;
                }


                sale.Date = Model.Date;
                sale.Comments = Model.Comments;
                sale.CustomerPayment = Model.CustomerPayment;
                sale.DiscountAmt = Model.DiscountAmt;
                sale.Discount = Model.Discount;
                sale.SalesDetailsList = Model.SalesDetailsList;
                if (_salesManager.SaveSalesProduct(sale))
                {
                    if (_customerManager.Update(aCustomer))
                    {
                        var customersList = _customerManager.FindAll();
                        ViewBag.Customers = new SelectList(customersList, "Id", "CustName");

                        var products = _productManager.GetProducts();
                        ViewBag.Products = new SelectList(products, "ProductId", "ProductName");
                        ViewBag.SuccessMsg = "Data Saved SuccessFully!";
                        return View();
                    }

                }
                else
                {
                    ViewBag.FailMsg = "Data Saved Fail!";
                }

            }
            catch(Exception EX)
            {
                ViewBag.FailMsg = "Something went wrong";
            }
         

            return View(Model);
        }

        public JsonResult GetCustLoyaltyPoints(int CustId)
        {
            var customer = _salesManager.GetCustLoyaltyPoints(CustId);

            return Json(customer, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProductDetails(int productId)
        {
            ProductViewModel model = new ProductViewModel();
            Product product = new Product();
            product.ProductId = productId;
            var aProduct = _productManager.GetProductById(product);
            string productCode = aProduct.ProductCode;
            model = _salesManager.ProductDetails(product);
            model.ProductCode = productCode;

            return Json(model, JsonRequestBehavior.AllowGet);
        }


       
        [HttpGet]
        public ActionResult VoucherDetails(SalesSaveViewModel salesSaveViewModel)
        {
            SalesSaveViewModel salesSaveVM = new SalesSaveViewModel();

            salesSaveVM.SalesList = _salesManager.FindAll();

            salesSaveVM.CustomerList = _customerManager.FindAll()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.CustName
                });
            return View(salesSaveVM);
        }
        [HttpPost]
        public ActionResult VoucherDetails(SalesDetails salesDetails)
        {
            SalesSaveViewModel salesSaveVM = new SalesSaveViewModel();
            var customers = _customerManager.FindAll();

            foreach (var v in customers)
            {
                salesDetails.Id = v.Id;
            }
            var salesDetailsList = _salesManager.FindAll();

            salesSaveVM.CustomerList = _customerManager.FindAll()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.CustName
                });
            return View(salesSaveVM);
        }

        public ActionResult ExportPDF()
        {
            return new ActionAsPdf("FindAll")
            {
                FileName = Server.MapPath("~/Content/ListSales.pdf")
            };
        }



    }
}