// See https://aka.ms/new-console-template for more information

using IceCreamShop.MongoDB.PresentationLayer;
using IceCreamShop.MySQL.PresentationLayer;

Console.WriteLine("[Menu] \n" +
                  "1. MySQL \n" +
                  "2. MongoDB \n" +
                  "3. Exit \n");
string userInput = Console.ReadLine() ?? "";

do
{
    switch (userInput)
    {
        case "1":
            new MySQLMain();
            break;
        case "2":
            new MongoDBMain();
            break;
        default:
            Console.WriteLine("Wrong input, try again");
            break;
    }
} while (userInput != "");