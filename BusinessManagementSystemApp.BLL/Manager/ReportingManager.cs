using BusinessManagementSystemApp.Models.Models;
using BusinessManagementSystemApp.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManagementSystemApp.BLL.Manager
{
    public class ReportingManager
    {
        ReportingRepository _reportingRepository = new ReportingRepository();
        public List<SalesDetails> PeriodictIncomeReport(ProductViewModel productViewModel)
        {
            return _reportingRepository.PeriodictIncomeReport(productViewModel);
        }
        public List<PurchaseDetails> PeriodictIncomeReportOnPurchase(ProductViewModel productViewModel)
        {
            return _reportingRepository.PeriodictIncomeReportOnPurchase(productViewModel);
        }
        public float GetSalesDiscountPercentByDate(int salesId)
        {
            return _reportingRepository.GetSalesDiscountPercentByDate(salesId);
        }
    }
}
