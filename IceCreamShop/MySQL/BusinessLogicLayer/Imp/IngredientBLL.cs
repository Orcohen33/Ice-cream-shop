using IceCreamShop.MySQL.BusinessLogicLayer.Interfaces;
using IceCreamShop.MySQL.DataAccessLayer.Imp;
using IceCreamShop.MySQL.Entity;

namespace IceCreamShop.MySQL.BusinessLogicLayer.Imp
{
    internal class IngredientBLL : ICrudBLL<Ingredient>
    {
        private readonly IngredientDAL ingredientDAL = new IngredientDAL();

        internal IngredientDAL IngredientDAL => ingredientDAL;
        #region Crud operations
        public int createRecord(Ingredient obj)
        {
            return ingredientDAL.createRecord(obj);
        }

        public int deleteRecord(Ingredient obj)
        {
            return ingredientDAL.deleteRecord(obj);
        }



        public int deleteRecord(int pk)
        {
            return ingredientDAL.deleteRecord(pk);
        }

        public IList<Ingredient> readAll()
        {
            return ingredientDAL.readAll();
        }

        public int updateRecord(int pk, Ingredient obj)
        {
            return ingredientDAL.updateRecord(pk, obj);
        }

        public int updateRecord(Ingredient obj)
        {
            return ingredientDAL.updateRecord(obj);
        }
        #endregion

        public Ingredient GetIngredientByID(int id)
        {
            return ingredientDAL.GetIngredientByID(id);
        }
        public int GetIngredientID(string name, string type)
        {
            return ingredientDAL.GetIngredientID(name, type);
        }
        public IList<Ingredient> GetIngredientsListByType(string type)
        {
            return ingredientDAL.GetIngredientsListByType(type);
        }
    }
}
