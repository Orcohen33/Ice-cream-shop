using IceCreamShop.MySQL.BusinessLogicLayer.Analysis;
using IceCreamShop.MySQL.BusinessLogicLayer.Imp;
using IceCreamShop.MySQL.Entity;
#pragma warning disable
namespace IceCreamShop.MySQL.BusinessLogicLayer
{
    // This class represents the logic of ordering a dish in a store.
    public class OrderProcess
    {
        int userInput = 0;
        string userInputStr = "";
        bool chooseChocolate = false, chooseMekupelet = false, chooseVanilla = false;
        Sale sale;
        SaleBLL saleBLL = new();
        IngredientBLL ingredientBLL = new();
        OrderBLL orderBLL = new();
        public OrderProcess()
        {
            main();
        }
        void main()
        {
            do
            {
                //Console.Clear();
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
        /* step 1 */
        void startOrder()
        {
            Console.Clear();
            Console.WriteLine("[startOrder]");

            // initialize sale
            sale = new(DateTime.Now.ToShortDateString());
            saleBLL.createRecord(sale);
            sale.setId(saleBLL.GetPK());
            chooseCup(ref sale); // step 1 -> step 2
        }
        /* step 2 */
        void chooseCup(ref Sale sale)
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
                    sale.orders.Add(new(sale.Id, ingredientBLL.GetIngredientID("RegularCup", "Box")));
                    userInputStr = "Cup";
                    break;
                case 2:
                    sale.orders.Add(new(sale.Id, ingredientBLL.GetIngredientID("SpecialCup", "Box")));
                    userInputStr = "Cup";
                    break;
                case 3:
                    sale.orders.Add(new(sale.Id, ingredientBLL.GetIngredientID("Box", "Box")));
                    userInputStr = "Box";
                    break;
                case -1:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            howManyBalls(); // step 2 -> step 3
        }
        /* step 3 */
        void howManyBalls()
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

            chooseFlavor(numOfBalls); // step 3 -> step 4
        }



        /* step 4 */
        void chooseFlavor(int numOfBalls)
        {

            // list of all icecream tastes
            var kindOfBalls = ingredientBLL.GetIngredientsListByType("IceCream");
            Console.WriteLine("id : name");
            #region print list of icecream tastes
            foreach (Ingredient ing in kindOfBalls)
            {
                Console.WriteLine($"{ing.Id} : {ing.Name}");
            }
            #endregion


            if (numOfBalls > 1)
            {
                for (int i = 0; i < numOfBalls; i++)
                {
                    Console.Write("\tchoose id of taste: ");
                    userInput = int.Parse(Console.ReadLine());
                    sale.orders.Add(new(sale.Id, userInput));
                }
            }
            else
            {
                Console.Write("\tchoose id of taste: ");
                userInput = int.Parse(Console.ReadLine());
                sale.orders.Add(new(sale.Id, userInput));
                sale.setPrice(sale.Price + 1);  // 1 ball worth 7, 2 and more worth each 6
            }
            chooseHowManyToppings(numOfBalls); // step 4 -> step 5
        }
        /* step 5 */
        void chooseHowManyToppings(int numOfBalls)
        {
            if (numOfBalls == 1 &&
                ingredientBLL.GetIngredientByID(sale.orders.ElementAt(0).IngredientId)
                            .Name == "RegularCup") { finalStepOfOrder(); } // step 5 -> step 7
            Console.Write("Choose how many toppings: ");
            chooseTopping(int.Parse(Console.ReadLine())); // step 5 -> step 6            
        }

        /* step 6 */
        void chooseTopping(int userInput)
        {
            WhatAddInsCanBeAdded();
            var kindOfToppings = ingredientBLL.GetIngredientsListByType("Topping").Where(x => x.Type == "Topping")
                                                        .Where(x => !((chooseChocolate || chooseMekupelet) && x.Name == "Chocolate"))
                                                        .Where(x => !(chooseVanilla && x.Name == "Maple")).ToList();
            int index = 1;
            #region print list of toppings
            kindOfToppings.ForEach(x => Console.WriteLine($"{index++}. {x.Name} ({x.Price} nis)"));
            #endregion
            for (int i = 0; i < userInput; i++)
            {
                Console.Write("\tchoose id of topping: ");
                sale.orders.Add(new(sale.Id, kindOfToppings.ElementAt(int.Parse(Console.ReadLine()) - 1).Id));
            }
            finalStepOfOrder(); // step 6 -> step 7
        }


        /* step 7 */
        void finalStepOfOrder()
        {
            Console.WriteLine("[finalStepOfOrder]");
            Console.WriteLine("1.Complete an order and start a new one.\n" +
                              "2.Complete and back to start menu.\n" +
                              "3.Cancel order.");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    chooseVanilla = false;
                    chooseMekupelet = false;
                    chooseChocolate = false;
                    chooseCup(ref sale);
                    break;
                case 2:
                    completeOrder();
                    break;
                case 3:
                    break;
                default:
                    break;
            }

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
        // private method to check which topping can be served to client

        private void WhatAddInsCanBeAdded()
        {
            for (int i = 1; i < sale.orders.Count(); i++)
            {
                switch (ingredientBLL.GetIngredientByID(sale.orders.ElementAt(i).IngredientId)
                            .Name)
                {
                    case "Chocolate":
                        chooseChocolate = true;
                        break;
                    case "Mekupelet":
                        chooseMekupelet = true;
                        break;
                    case "Vanilla":
                        chooseVanilla = true;
                        break;
                }
            }
        }
        private bool validNumOfBalls(int numOfBalls)
        {
            switch (userInputStr)
            {
                case "Cup":
                    return !(0 < numOfBalls && numOfBalls < 4);
                case "Box":
                    return !(0 < numOfBalls);
            };
            return false;
        }
        private void completeOrder()
        {
            foreach (Order order in sale.orders)
            {
                orderBLL.createRecord(order);
            }
            sale.setPrice(sale.getPrice() + sale.orders.Sum(order => orderBLL.getProductPrice(order)));
            Console.WriteLine("============================ [completeOrder] ===============");
            Console.WriteLine(sale.Price);
            saleBLL.updateRecord(sale);
        }
        #endregion

    }
}
