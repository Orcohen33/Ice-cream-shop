using IceCreamShop.MySQL.DataAccessLayer.Analysis;

namespace IceCreamShop.MySQL.BusinessLogicLayer.Analysis
{
    internal class AnalysisImpBLL
    {
        AnalysisImpDAL analysisImpDAL = new();
        public string customerInvoice(int sid)
        {
            return analysisImpDAL.customerInvoice(sid);
        }
        public string endOfDayReport(string date)
        {
            return analysisImpDAL.endOfDayReport(date);
        }
        public string allEndOfDayReports()
        {
            return analysisImpDAL.allEndOfDayReports();
        }

        public string showAllIncompleteSales()
        {
            return analysisImpDAL.showAllIncompleteSales();
        }

        public string mostCommonIngredientNTaste()
        {
            return analysisImpDAL.mostCommonIngredientNTaste();
        }
    }
}
