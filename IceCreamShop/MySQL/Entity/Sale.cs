namespace IceCreamShop.MySQL.Entity
{
    class Sale
    {
        private int _Id;
        private DateTime _Order_date;
        private int? _Price;

        // one to many
        public ICollection<Order> orders { get; set; }

        public void setId(int pk) { _Id = pk; }
        public void setPrice(int? price) { _Price = price; }
        public int getPrice() { return _Price ?? 0; }

        public int Id
        {
            get => _Id;
            set => _Id = Id;
        }

        public DateTime Order_date
        {
            get => _Order_date;
            set => _Order_date = Order_date;
        }
        public int? Price
        {
            get => _Price;
            set => _Price = Price;
        }
        public Sale() { _Order_date = DateTime.Now; orders = new List<Order>(); _Id = new(); }
        public Sale(string date) { _Order_date = DateTime.Parse(date); orders = new List<Order>(); _Id = new(); }
        public override string ToString()
        {
            return $"[{Id}: {Order_date.ToShortDateString()} price: {Price} ]";
        }

    }
}
