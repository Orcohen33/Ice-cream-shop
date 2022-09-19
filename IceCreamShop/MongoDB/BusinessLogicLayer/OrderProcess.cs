using IceCreamShop.MongoDB.BusinessLogicLayer.Analysis;
using IceCreamShop.MongoDB.DataAccessLayer.Imp;
using IceCreamShop.MongoDB.Entity;

namespace IceCreamShop.MongoDB.BusinessLogicLayer
{
    //  This class represents the logic of ordering a dish in a store.
    internal class OrderProcess
    {
        private int userInput = 0;
        private string userInputStr = "";
        bool chooseChocolate = false, chooseMekupelet = false, chooseVanilla = false;
        private Sale mongoSale;
        private SaleDAL mongoSaleDAL = new();
        private readonly List<Ingredient> IngredientCollection = (List<Ingredient>)new IngredientDAL().ReadAll();

        public OrderProcess()
        {
            main();
        }


        void main()
        {

            do
            {
                Console.WriteLine("[Main-menu]\n\t" +
                    "1.Start order.\n\t" +
                    "2.Analysis.\n\t" +
                    "-1. Exit");

                userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 1:
                        startOrder();
                        break;
                    case 2:
                        startAnalysis();
                        break;
                    case -1:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            } while (userInput != -1);
        }

        #region Order process
        //step 1
        private void startOrder()
        {

            Console.Clear();
            Console.WriteLine("[startOrder]");
            // initialize sale
            mongoSale = new()
            {
                order_date = DateTime.Now,
                ingredients = new List<Ingredient>()
            };
            mongoSaleDAL.CreateDocument(mongoSale);
            chooseCup(ref mongoSale); // step 1 -> step 2

        }
        //step 2
        private void chooseCup(ref Sale mongoSale)
        {
            Console.WriteLine("[chooseCup]");
            Console.WriteLine("Choose which kind of cup you want to order:\n\t" +
                  "1.Regular (0 nis)\n\t" +
                  "2.Special (2 nis)\n\t" +
                  "3.Box     (5 nis)\n\t" +
                  "-1.Exit");
            userInput = int.Parse(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    mongoSale.ingredients.Add(IngredientCollection.Find(x => x.name == "RegularCup" && x.type == "Box"));
                    userInputStr = "Cup";
                    break;
                case 2:
                    mongoSale.ingredients.Add(IngredientCollection.Find(x => x.name == "SpecialCup" && x.type == "Box"));
                    userInputStr = "Cup";
                    break;
                case 3:
                    mongoSale.ingredients.Add(IngredientCollection.Find(x => x.name == "Box" && x.type == "Box"));
                    userInputStr = "Box";
                    break;
                case -1:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            howManyBalls(); //step 2 -> step 3
        }
        //step 3
        private void howManyBalls()
        {
            int numOfBalls = 0;
            switch (userInputStr)
            {
                case "Cup":
                    Console.Write("(you allowed to choose at most 3 balls)\nChoose how many balls: ");
                    break;
                case "Box":
                    Console.Write("(unlimited balls)\nChoose how many balls:");
                    break;
                default:
                    break;
            }
            do
            {
                numOfBalls = int.Parse(Console.ReadLine());
            } while (validNumOfBalls(numOfBalls));
            chooseFlavor(numOfBalls); //step 3 -> step 4
        }
        //step 4
        private void chooseFlavor(int numOfBalls)
        {
            Console.WriteLine("[chooseFlavor]");
            Console.WriteLine("Choose which flavor you want to order:\n\t");
            var index = 1;
            IngredientCollection.FindAll(x => x.type == "IceCream")
                                .ForEach(x => Console.WriteLine($"{index++}. {x.name} ({x.price} nis)"));

            var iceCreamList = IngredientCollection.FindAll(x => x.type == "IceCream");
            for (int i = 0; i < numOfBalls; i++)
            {
                Console.Write("Choose flavor: ");
                userInput = int.Parse(Console.ReadLine());
                if (userInput > 0 && userInput <= iceCreamList.Count())
                {
                    mongoSale.ingredients.Add(iceCreamList[userInput - 1]);

                    if (iceCreamList[userInput - 1].name == "Chocolate")
                        chooseChocolate = true;
                    else if (iceCreamList[userInput - 1].name == "Mekupelet")
                        chooseMekupelet = true;
                    else if (iceCreamList[userInput - 1].name == "Vanilla")
                        chooseVanilla = true;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }

            }
            chooseHowManyToppings(numOfBalls); //step 4 -> step 5
        }
        //step 5
        private void chooseHowManyToppings(int numOfBalls)
        {
            if (mongoSale.ingredients.Any(x => x.name == "RegularCup")
                && numOfBalls == 1)
            { mongoSale.price += 1; finalStepOrder(ref mongoSale); return; }
            if (numOfBalls == 1) { mongoSale.price += 1; }
            Console.WriteLine("Choose how many toppings you want to order: ");
            chooseToppings(int.Parse(Console.ReadLine())); //step 5 -> step 6
        }
        //step 6
        private void chooseToppings(int numOfToppings)
        {
            // print list of toppings from mongoingredientdal
            int index = 1;
            var toppingCollection = IngredientCollection.Where(x => x.type == "Topping")
                                                        .Where(x => !((chooseChocolate || chooseMekupelet) && x.name == "Chocolate"))
                                                        .Where(x => !(chooseVanilla && x.name == "Maple")).ToList();
            toppingCollection.ForEach(x => Console.WriteLine($"{index++}: {x.name} ({x.price} nis)"));

            do
            {
                Console.WriteLine("Choose topping: ");
                userInput = int.Parse(Console.ReadLine());
                if (userInput > 0 && userInput <= toppingCollection.Count)
                {
                    mongoSale.ingredients.Add(toppingCollection[userInput - 1]);
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            } while (--numOfToppings > 0);
            finalStepOrder(ref mongoSale); //step 6 -> step 7
        }
        //step 7
        private void finalStepOrder(ref Sale mongoSale)
        {
            Console.WriteLine("[finalStepOrder]");
            Console.WriteLine("1.Complete an order and start a new one.\n" +
                  "2.Complete and back to start menu.\n" +
                  "3.Cancel order.");
            chooseVanilla = false;
            chooseMekupelet = false;
            chooseChocolate = false;
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    break;
                case 2:
                    completeOrder(ref mongoSale);
                    break;
                case 3: break;
                default: break;

            }
        }

        private void completeOrder(ref Sale mongoSale)
        {
            if (mongoSale.ingredients != null)
                foreach (Ingredient ingredient in mongoSale.ingredients)
                {
                    if (mongoSale.price == null) mongoSale.price = 0;
                    mongoSale.price += ingredient.price;
                } // Fix this
            Console.WriteLine("Your order is ready!");
            Console.WriteLine($"Your order price is: {mongoSale.price}");
            mongoSaleDAL.UpdateDocument(mongoSale);
        }
        #endregion
        #region Analysis
        private void startAnalysis()
        {
            Console.Clear();
            AnalysisImpBLL analysisImpBLL = new();
            do
            {
                Console.WriteLine($"Choose what you want to analyze:\n" +
                                $"1. Customer invoice (Enter sale id): \n" +
                                $"2. End of day report: (Enter date in format: yyyy-mm-dd): \n" +
                                $"3. All end of day reports: \n" +
                                $"4. Show all incomplete sales: \n" +
                                $"5. Most common ingredient and taste:");
                userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 1:
                        Console.WriteLine("Enter sale id: ");
                        userInput = int.Parse(Console.ReadLine());
                        Console.WriteLine(analysisImpBLL.customerInvoice(userInput));
                        break;
                    case 2:
                        Console.WriteLine("Enter date in format: yyyy-mm-dd: ");
                        userInputStr = Console.ReadLine();
                        Console.WriteLine(analysisImpBLL.endOfDayReport(userInputStr));
                        break;
                    case 3:
                        Console.WriteLine(analysisImpBLL.allEndOfDayReports());
                        break;
                    case 4:
                        Console.WriteLine(analysisImpBLL.showAllIncompleteSales());
                        break;
                    case 5:
                        Console.WriteLine(analysisImpBLL.mostCommonIngredientNTaste());
                        break;
                    default:
                        break;
                }
            } while (userInput != -1);
        }
        #endregion
        #region help functions
        private bool validNumOfBalls(int numOfBalls)
        {
            if (userInputStr == "Cup" && numOfBalls > 3)
            {
                Console.WriteLine("Invalid input");
                return true;
            }
            else if (userInputStr == "Box" && numOfBalls < 1)
            {
                Console.WriteLine("Invalid input");
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
