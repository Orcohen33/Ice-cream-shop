namespace IceCreamShop.MySQL.DataAccessLayer.Interfaces
{
    interface ICrudDAL<T>
    {
        int createRecord(T obj);
        IList<T> readAll();
        int updateRecord(int pk, T obj);
        int updateRecord(T obj);
        int deleteRecord(T obj);
        int deleteRecord(int pk);
    }
}
