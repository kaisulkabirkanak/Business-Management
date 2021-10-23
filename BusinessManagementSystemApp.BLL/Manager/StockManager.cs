using BusinessManagementSystemApp.Models.Models;
using BusinessManagementSystemApp.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessManagementSystemApp.Models;

namespace BusinessManagementSystemApp.BLL.Manager
{
    public class StockManager
    {
        StockRepository _stockRepository = new StockRepository();

        public List<Product> GetAll()
        {
            return _stockRepository.GetAll();
        }
        public List<PurchaseDetails> SearchByExpiredDate(DateTime StartDate, DateTime EndDate, Product product)
        {
            return _stockRepository.SearchByExpiredDate(StartDate,EndDate,product);
        }
        public List<Product> GetByProductDetails(int productName)
        {

            return _stockRepository.GetByProductDetails(productName);

        }
        public int GetProductAvailableQuantity(Product product)
        {
            return _stockRepository.GetProductAvailableQuantity(product);
        }
    }
}
