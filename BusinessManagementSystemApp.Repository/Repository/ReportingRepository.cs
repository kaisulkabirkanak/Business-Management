using BusinessManagementSystemApp.DatabaseContext.DatabaseContext;
using BusinessManagementSystemApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BusinessManagementSystemApp.Repository.Repository
{
    public class ReportingRepository
    {
        BusinessManagementDbContext db = new BusinessManagementDbContext();
        public List<SalesDetails> PeriodictIncomeReport(ProductViewModel productViewModel)
        {
            List<SalesDetails> salesDetails = new List<SalesDetails>();
            var saleProducts = db.Sales.Where(c => c.Date >= productViewModel.StartDate && c.Date <= productViewModel.EndDate).ToList();
            foreach(var product in saleProducts)
            {
                
                var saleProductList = db.SalesDetails.Include(c => c.Products).Where(c => c.SaleId == product.Id).ToList();
                foreach(var salesProduct in saleProductList)
                {
                    salesDetails.Add(salesProduct);
                }
            }
            return salesDetails;
        }
        public List<PurchaseDetails> PeriodictIncomeReportOnPurchase(ProductViewModel productViewModel)
        {
            List<PurchaseDetails> purchaseDetails= new List<PurchaseDetails>();
            var purchaseProducts = db.Purchases.Where(c => c.Date >= productViewModel.StartDate && c.Date <= productViewModel.EndDate).ToList();
            foreach (var product in purchaseProducts)
            {
                var purchaseProductList = db.PurchaseDetails.Include(c => c.Product).Where(c => c.PurchaseId == product.Id).ToList();
                foreach(var purchaseProduct in purchaseProductList)
                {
                    purchaseDetails.Add(purchaseProduct);
                }
            }
            return purchaseDetails;
        }
        public float GetSalesDiscountPercentByDate(int salesId)
        {
            float salesDiscountPercent = db.Sales.Where(c => c.Id == salesId).Select(c => c.Discount).FirstOrDefault();
            return salesDiscountPercent;
        }
    }
}
