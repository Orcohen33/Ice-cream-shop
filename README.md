# Ice-cream-shop
Implement a ice cream shop system with MySQL &amp; MongoDB using the three-layer architecture.

## About
<br>This project represents the customer and manager's side in an ice cream shop (for analysis purposes).</br>
<br>The customer has the option to order ice cream and there are a number of restrictions in the assembly of the ice cream.</br>
<br>The manager can see the following data: 
1. Invoice to the customer (including date, price, list of products).
2. End of day report: sales amount, sales volume, average selling price.
3. Incomplete sales.
4. What is the most common ingredient and what is the most common flavor.
</br>

# System Design
![SystemDesign](https://user-images.githubusercontent.com/92351152/191262207-a42195e5-704b-40ee-9ffe-109271b49e8a.png)


# MySQL

### Entity relationship diagram
![ERDiagram](https://user-images.githubusercontent.com/92351152/191253758-8a8b687d-b00d-4578-8895-e3fa27f3e30d.png)

### Connection set-up
<br>At `MySQL/DataAccessLayer/Factory/DbConnection`</br>
set up your connection details here:
```c#
...
const string connStr = "server=localhost;user=root;port=3306;password=root";
...
```
### How to run

# MongoDB

### Document example
#### Ingredient document:
![IngredientDocument](https://user-images.githubusercontent.com/92351152/190988063-8ca31405-416b-4bd5-a463-ab10201a7293.png)
#### Sale document:
![SaleDocument](https://user-images.githubusercontent.com/92351152/190988370-961e717b-a5a7-45d4-97ba-cd409595a968.png)


### Connection set-up
<br>At `MongoDB/DataAccessLayer/Factory/DBConnection`</br>
set up your connection details here:
```c#
...
MongoClientSettings settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
...
```

## Requirement
`.NET 6.0`

`MySQL`

`MongoDB`

# How to run

  - Open cmd and write the following commands: 
  - Clone this repository : `git clone https://github.com/Orcohen33/Ice-cream-shop.git`
  - Enter the foler 'IceCreamShop' : `cd Ice-cream-shop/IceCreamShop`
  - Run the program: `dotnet run Program.cs`


# Overview
### A simple video illustrating the console app:
#### MySQL:
https://user-images.githubusercontent.com/92351152/191263303-748b2262-b0b3-40e7-942e-5dc28f16249b.mp4







