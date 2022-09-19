using IceCreamShop.MySQL.BusinessLogicLayer.Interfaces;
using IceCreamShop.MySQL.Entity;

namespace IceCreamShop.MySQL.BusinessLogicLayer.Imp
{
    internal class SaleBLL : ICrudBLL<Sale>
    {
        private readonly DataAccessLayer.Imp.SaleDAL saleDAL = new DataAccessLayer.Imp.SaleDAL();
        public int createRecord(Sale obj)
        {
            return saleDAL.createRecord(obj);
        }
        public IList<Sale> readAll()
        {
            return saleDAL.readAll();
        }
        public int updateRecord(int pk, Sale obj)
        {
            return saleDAL.updateRecord(pk, obj);
        }
        public int updateRecord(Sale obj)
        {
            return saleDAL.updateRecord(obj);
        }
        public int deleteRecord(Sale obj)
        {
            return saleDAL.deleteRecord(obj);
        }
        public int deleteRecord(int pk)
        {
            return saleDAL.deleteRecord(pk);
        }

        public int GetPK()
        {
            int pk = saleDAL.GetPK();
            return pk;
        }

    }
}
