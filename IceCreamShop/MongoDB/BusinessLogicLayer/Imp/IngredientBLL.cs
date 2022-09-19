using IceCreamShop.MongoDB.BusinessLogicLayer.Interfaces;
using IceCreamShop.MongoDB.DataAccessLayer.Imp;
using IceCreamShop.MongoDB.Entity;

namespace IceCreamShop.MongoDB.BusinessLogicLayer.Imp
{
    internal class IngredientBLL : ICrudBLL<Ingredient>
    {
        private readonly IngredientDAL ingredientDAL = new();
        public void CreateDocument(Ingredient ingredient)
        {
            ingredientDAL.CreateDocument(ingredient);
        }

        public void DeleteDocument(Ingredient ingredient)
        {
            ingredientDAL.DeleteDocument(ingredient);
        }

        public ICollection<Ingredient> ReadAll()
        {
            return ingredientDAL.ReadAll();
        }

        public Ingredient ReadDocument(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocument(Ingredient ingredient)
        {
            ingredientDAL.UpdateDocument(ingredient);
        }
    }
}
