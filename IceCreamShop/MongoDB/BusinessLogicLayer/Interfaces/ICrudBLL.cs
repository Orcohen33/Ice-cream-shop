namespace IceCreamShop.MongoDB.BusinessLogicLayer.Interfaces
{
    interface ICrudBLL<T>
    {
        public void CreateDocument(T sale);
        public ICollection<T> ReadAll();
        public T ReadDocument(int id);
        public void UpdateDocument(T sale);

        public void DeleteDocument(T sale);
    }
}
