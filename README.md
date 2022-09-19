# Ice-cream-shop
Implement a ice cream shop system with MySQL &amp; MongoDB

## About
<br>This project represents the customer and manager's side in an ice cream shop (for analysis purposes).</br>
<br>The customer has the option to order ice cream and there are a number of restrictions in the assembly of the ice cream.</br>
<br>The manager can see the following data: 
1. Invoice to the customer (including date, price, list of products).
2. End of day report: sales amount, sales volume, average selling price.
3. Incomplete sales.
4. What is the most common ingredient and what is the most common flavor.
</br>

## Requirement
`.NET 6.0`

`MySQL`

`MongoDB`

# MySQL

### Entity relationship diagram
![ERDiagram](https://user-images.githubusercontent.com/92351152/190480122-cc04283c-5ecd-4c6d-bf11-3a0ef8efd2db.png)


### Connection set-up
<br>At `DataAccessLayer/MySqlFactory/DbConnection`</br>
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
<br>At `DataAccessLayer/MongoDbFactory/MongoDBConnection`</br>
set up your connection details here:
```c#
...
MongoClientSettings settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
...
```
# Overview
### A simple video illustrating the console app:
