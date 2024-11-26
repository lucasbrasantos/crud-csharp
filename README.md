# 📋 Simple CRUD Application with MySQL and Windows Forms

This project is a basic **CRUD** (Create, Read, Update, Delete) application built with **C#** and **Windows Forms**, using **MySQL** as the database. It demonstrates how to interact with a MySQL database to manage product data.

---

## 🚀 Features

- **Create**: Add new products with name and price.
- **Read**: View all products in a `ListView` control.
- **Update**: Modify selected product details.
- **Delete All**: Delete all records in the database.
- **Test Connection**: Check the connection to the MySQL database.

---

## 🛠️ Requirements

- **Visual Studio** (Any version supporting Windows Forms)
- **MySQL Server** installed locally
- **MySQL.Data** NuGet package for database connectivity

---

## 📂 Database Setup

1. Open your MySQL database management tool (e.g., MySQL Workbench).
2. Create a new database:

   ```sql
   CREATE DATABASE crud;
   ```

3. Create the `products` table:

   ```sql
   CREATE TABLE products (
       id INT AUTO_INCREMENT PRIMARY KEY,
       name VARCHAR(255) NOT NULL,
       price DOUBLE NOT NULL
   );
   ```

---

## 📝 How to Use

1. Clone the repository or copy the code into a new Visual Studio project.
2. Restore NuGet packages (specifically `MySql.Data`).
3. Update the database connection details in `DatabaseForm.cs`:

   ```csharp
   private string host = "localhost";
   private string user = "root";
   private string password = "";
   private string database = "crud";
   ```

4. Run the project and use the **UI buttons** for CRUD operations:
   - **Insert**: Add a new product.
   - **Select All**: Display all products.
   - **Update**: Update the selected product.
   - **Delete All**: Remove all records.
   - **Test Connection**: Verify the database connection.

---

## 🖥️ Application UI

The application has a simple interface:
- **Input Fields**: For product name and price.
- **ListView**: Displays all records from the `products` table.
- **Buttons**: Perform CRUD actions and test the database connection.

---

## 🤝 Contributing

Feel free to improve or expand the project. Create a pull request or fork the repository to get started.

---

## 🧾 License

This project is licensed under the MIT License. 

---

Enjoy coding! 😊
