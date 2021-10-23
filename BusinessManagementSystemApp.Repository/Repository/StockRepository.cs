using BusinessManagementSystemApp.DatabaseContext.DatabaseContext;
using BusinessManagementSystemApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManagementSystemApp.Repository.Repository
{
    public class StockRepository
    {
        BusinessManagementDbContext db = new BusinessManagementDbContext();
        public List<Product> GetAll()
        {
            return db.Products.Include(p => p.Categories).ToList();
        }


        public PurchaseDetails PurchaseDetails(Product product, DateTime startDate, DateTime endDate)
        {
            var aProduct = db.PurchaseDetails.Include(c => c.Product).Where(c => c.Product.ProductName.ToLower() == product.ProductName.ToLower() && c.ExpireDate >= startDate && c.ExpireDate <= endDate).FirstOrDefault();

            //var aProduct = db.PurchaseDetails.Include(c => c.Product).Where(c => c.Product.Name.ToLower() == product.ProductName.ToLower()).FirstOrDefault();
            return aProduct;
        }

        public int GetProductAvailableQuantity(Product product)
        {
            int availableQuantity = 0;
            int purchaseQuantity = 0;
            int salesQuantity = 0;
            var purchaseProducts = db.PurchaseDetails.Where(c => c.ProductId == product.ProductId).ToList();
            foreach (var purchaseProduct in purchaseProducts)
            {
                purchaseQuantity += purchaseProduct.Quantity;
            }
            var salesProducts = db.SalesDetails.Where(c => c.ProductsId == product.ProductId).ToList();
            foreach (var salesProduct in salesProducts)
            {
                salesQuantity += salesProduct.Quantity;
            }
            availableQuantity = purchaseQuantity - salesQuantity;
            return availableQuantity;
        }
        public List<PurchaseDetails> SearchByExpiredDate(DateTime StartDate, DateTime EndDate, Product product)
        {

            var productList = db.PurchaseDetails.Where(c => c.ExpireDate >= StartDate && c.ExpireDate <= EndDate).ToList();

            return productList;
        }
        public List<Product> GetByProductDetails(int productName)
        {

            return db.Products.Include(c => c.Category).Where(c => c.ProductId == productName).ToList();

        }

    }
}
