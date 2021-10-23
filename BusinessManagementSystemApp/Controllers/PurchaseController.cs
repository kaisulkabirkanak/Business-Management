using BusinessManagementSystemApp.BLL.BLL;
using BusinessManagementSystemApp.BLL.Manager;
using BusinessManagementSystemApp.Models;
using BusinessManagementSystemApp.Models.Models;
using System;
using Rotativa;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessManagementSystemApp.Controllers
{
    public class PurchaseController : Controller
    {
        SupplierManager _supplierManager = new SupplierManager();
        ProductManager _productManager = new ProductManager();
        PurchaseManager _purchaseManager = new PurchaseManager();
        Purchase _purchase = new Purchase();
        Product _product = new Product();
        //Get purchase
        public ActionResult Index()
        {
            List<PurchaseViewModel> models = new List<PurchaseViewModel>();
            var products = _purchaseManager.GetPurchaseProducts();
            
            foreach(var product in products)
            {                
                PurchaseViewModel model = new PurchaseViewModel();
                if (models.Count > 0)
                {
                    int count = 0;
                    int countValue = models.Count;
                    foreach(var p in models)
                    {
                        count++;
                        if (p.ProductId == product.ProductId)
                        {
                            p.Quantity += product.Quantity;
                            break;
                        }
                        else
                        {
                            if (count == countValue)
                            {
                                model.ProductId = product.ProductId;
                                model.ProductName = product.Product.ProductName;
                                model.Quantity = product.Quantity;
                                _product.ProductId = model.ProductId;
                                var latestProduct = _purchaseManager.LatestProduct(_product);
                                model.PreviousCostPrice = latestProduct.PreviousCostPrice;
                                model.PreviousMRP = latestProduct.PreviousMRP;

                                models.Add(model);
                                break;
                            }
                        }

                    }
                }
                else
                {
                    model.ProductId = product.ProductId;
                    model.ProductName = product.Product.ProductName;
                    model.Quantity = product.Quantity;
                    _product.ProductId = model.ProductId;
                    var latestProduct=_purchaseManager.LatestProduct(_product);
                    model.PreviousCostPrice = latestProduct.PreviousCostPrice;
                    model.PreviousMRP = latestProduct.PreviousMRP;

                    models.Add(model);
                }
            }
            foreach(var model in models)
            {
                _product.ProductId = model.ProductId;
                int salesQuantity = _purchaseManager.GetSalesProductQuantity(_product);
                model.Quantity -= salesQuantity;
            }
            PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
            purchaseViewModel.PurchaseViewModels = models;
            return View(purchaseViewModel);
        }
        [HttpPost]
        public ActionResult Index(string searchText)
        {
            if (ModelState.IsValid)
            {
                List<PurchaseViewModel> models = new List<PurchaseViewModel>();
                var products = _purchaseManager.GetPurchaseProducts();
                foreach (var product in products)
                {
                    PurchaseViewModel model = new PurchaseViewModel();
                    if (models.Count > 0)
                    {
                        int count = 0;
                        int countValue = models.Count;
                        foreach (var p in models)
                        {
                            count++;
                            if (p.ProductId == product.ProductId)
                            {
                                p.Quantity += product.Quantity;
                                break;
                            }
                            else
                            {
                                if (count == countValue)
                                {
                                    model.ProductId = product.ProductId;
                                    model.ProductName = product.Product.ProductName;
                                    model.Quantity = product.Quantity;
                                    _product.ProductId = model.ProductId;
                                    var latestProduct = _purchaseManager.LatestProduct(_product);
                                    model.PreviousCostPrice = latestProduct.PreviousCostPrice;
                                    model.PreviousMRP = latestProduct.PreviousMRP;

                                    models.Add(model);
                                    break;
                                }
                            }

                        }
                    }
                    else
                    {
                        model.ProductId = product.ProductId;
                        model.ProductName = product.Product.ProductName;
                        model.Quantity = product.Quantity;
                        _product.ProductId = model.ProductId;
                        var latestProduct = _purchaseManager.LatestProduct(_product);
                        model.PreviousCostPrice = latestProduct.PreviousCostPrice;
                        model.PreviousMRP = latestProduct.PreviousMRP;

                        models.Add(model);
                    }
                }
                PurchaseViewModel purchaseViewModel = new PurchaseViewModel();
                purchaseViewModel.PurchaseViewModels = models.Where(c => c.ProductName.ToLower().Contains(searchText.ToLower())).ToList();
                return View(purchaseViewModel);
            }
            
            return View();
        }
        //Select Supplier
        [HttpGet]
        public ActionResult InsertPurchaseProduct()
        {
            var suppliers = _supplierManager.GetAll();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            var products = _productManager.GetProducts();
            ViewBag.Products = new SelectList(products, "ProductId", "ProductName");
            return View();
        }
        [HttpPost]
        public ActionResult InsertPurchaseProduct(PurchaseViewModel purchaseViewModel)
        {
            var suppliers = _supplierManager.GetAll();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
            var products = _productManager.GetProducts();
            ViewBag.Products = new SelectList(products, "ProductId", "ProductName");
            if (ModelState.IsValid)
            {
                if (purchaseViewModel.SupplierId > 0)
                {
                    _purchase.InvoiceNo = purchaseViewModel.InvoiceNo;
                    _purchase.Date = purchaseViewModel.Date;
                    _purchase.SupplierId = purchaseViewModel.SupplierId;
                    _purchase.PurchaseDetails = purchaseViewModel.PurchaseDetails;
                    if (_purchaseManager.InsertPurchaseProduct(_purchase))
                    {
                        ViewBag.Message = "Saved";


                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "Failed!";
                    }
                }
                else
                {
                    ViewBag.Message = "Input validation error!";
                }
            }
            else
            {
                ViewBag.Message = "Input validation error!";
            }
            return View(purchaseViewModel);
        }

        public JsonResult GetProductHistory(int productId)
        {
            ProductViewModel model = new ProductViewModel();
            Product product = new Product();
            product.ProductId = productId;
            var aProduct = _productManager.GetProductById(product);
            string productCode = aProduct.ProductCode;
            model = _purchaseManager.LatestProduct(product);
            model.ProductCode = productCode;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportPDF()
        {
            return new ActionAsPdf("Index")
            {
                FileName = Server.MapPath("~/Content/ListAvailableQuentity.pdf")
            };
        }
    }
}