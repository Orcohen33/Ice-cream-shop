namespace IceCreamShop.MySQL.Entity
{
    public class Ingredient
    {
        private int _Id;
        private string _Name;
        private string _Type;
        private int _Price;


        public Ingredient(int id, string name, string type, int price)
        {
            _Id = id;
            _Name = name;
            _Type = type;
            _Price = price;
        }
        public int Id
        {
            get => _Id;
            set => _Id = Id;
        }
        public string Name
        {
            get => _Name;
            set => _Name = Name;
        }
        public string Type
        {
            get => _Type;
            set => _Type = Type;
        }
        public int Price
        {
            get => _Price;
            set => _Price = Price;
        }

        public override string? ToString()
        {
            return $"{_Id}\n{_Name}\n{_Type}\n{_Price}";
        }
    }
}
