using IceCreamShop.MongoDB.BusinessLogicLayer.Interfaces;
using IceCreamShop.MongoDB.DataAccessLayer.Imp;
using IceCreamShop.MongoDB.Entity;

namespace IceCreamShop.MongoDB.BusinessLogicLayer.Imp
{
    internal class SaleBLL : ICrudBLL<Sale>
    {
        private readonly SaleDAL saleDAL = new SaleDAL();
        public void CreateDocument(Sale sale)
        {
            saleDAL.CreateDocument(sale);
        }

        public void DeleteDocument(Sale sale)
        {
            saleDAL.DeleteDocument(sale);
        }

        public ICollection<Sale> ReadAll()
        {
            return saleDAL.ReadAll();
        }

        public Sale ReadDocument(int id)
        {
            return saleDAL.ReadDocument(id);
        }

        public void UpdateDocument(Sale sale)
        {
            saleDAL.UpdateDocument(sale);
        }
    }
}
