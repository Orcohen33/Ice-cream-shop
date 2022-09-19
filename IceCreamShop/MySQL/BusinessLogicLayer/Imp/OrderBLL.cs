// See https://aka.ms/new-console-template for more information

using IceCreamShop.MySQL.BusinessLogicLayer.Interfaces;
using IceCreamShop.MySQL.DataAccessLayer.Imp;
using IceCreamShop.MySQL.Entity;

internal class OrderBLL : ICrudBLL<Order>
{
    private readonly OrderDAL orderDAL = new OrderDAL();
    #region ICrudBLL<Order> Members
    public int createRecord(Order obj)
    {
        return orderDAL.createRecord(obj);
    }

    public int deleteRecord(Order obj)
    {
        return orderDAL.deleteRecord(obj);
    }

    public int deleteRecord(int pk)
    {
        return orderDAL.deleteRecord(pk);
    }

    public IList<Order> readAll()
    {
        return orderDAL.readAll();
    }

    public int updateRecord(int pk, Order obj)
    {
        return orderDAL.updateRecord(pk, obj);
    }

    public int updateRecord(Order obj)
    {
        return orderDAL.updateRecord(obj);
    }

    #endregion

    public int getProductPrice(Order x)
    {
        return orderDAL.getProductPrice(x);
    }
}