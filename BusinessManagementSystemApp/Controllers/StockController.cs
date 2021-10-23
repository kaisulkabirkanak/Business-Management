using BusinessManagementSystemApp.BLL.BLL;
using BusinessManagementSystemApp.BLL.Manager;
using BusinessManagementSystemApp.Models;
using BusinessManagementSystemApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessManagementSystemApp.Controllers
{
    public class StockController : Controller
    {
        StockManager _stockManager = new StockManager();
        ProductManager _productManager = new ProductManager();
        CategoryManager _categoryManager = new CategoryManager();
        Product _product = new Product();


        // GET: Stock
        public ActionResult Search()
        {

            return View();
        }



        [HttpPost]
        public ActionResult Search(StockVM  stockVM, DateTime startDate, DateTime endDate)
        {
      

            _product.ProductName = stockVM.ProductName;
            _product.CategoryName = stockVM.CategoryName;
            try
            {
                List<StockVM> products = _productManager.GetProductsWithCatagory(_product).Select(c => new StockVM
                {
                    ProductId = c.ProductId,
                    ProductName = c.ProductName,
                    CategoryName = c.Category.CategoryName,
                    ReorderLevel = c.ReorderLevel,
                    Code = c.ProductCode,
                }).ToList();

                foreach (var product in products)
                {

                    Product pro = new Product();
                    pro.ProductId = product.ProductId;
                    pro.ProductName = product.ProductName;
                    var aProduct = _stockManager.GetProductAvailableQuantity(pro);
                    var expireDate = _productManager.PurchaseDetails(pro, startDate, endDate);
                    foreach (var item in products)
                    {

                        if (item.ProductName == pro.ProductName)
                        {
                            item.Quantity = aProduct;
                            item.ProductId = pro.ProductId;
                            item.ExpireDate = expireDate.ExpireDate;

                            TempData["SuccessMessage"] = "Search Successfully";
                        }


                    }

                }
                ViewBag.Products = products;
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "Search data not found";
            }


            return View();
        }
      
    }
}