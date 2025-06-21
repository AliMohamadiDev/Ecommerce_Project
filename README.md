# Ecommerce_Project

A modular, maintainable e-commerce application built with **ASP.NET Core Razor Pages**, applying **Onion Architecture** for clean separation of concerns.

## Architecture & Design

- **Onion Architecture**: Separation of concerns via Domain → Application → Infrastructure → Presentation layers.
- **Modular Design**: Each core functionality is developed as an independent module, making the project scalable and easy to maintain.
- Built with **.NET 6** and **Razor Pages**.
- Designed for learning and code demonstration purposes.

## Modules

- AccountManagement
- BlogManagement
- CommentManagement
- DiscountManagement
- InventoryManagement
- ShopManagement

## Features

- User registration and login (AccountManagement)
- Blog system with posts and categories (BlogManagement)
- Comment system for blog posts and products (CommentManagement)
- Discount codes and campaign management (DiscountManagement)
- Product inventory tracking and management (InventoryManagement)
- Full shop module: products, categories, product pictures, and more (ShopManagement)
- Admin panels for managing each module
- Strong folder structure and dependency injection

## Technologies Used

- ASP.NET Core Razor Pages
- C# (.NET 6)
- Entity Framework Core
- SQL Server
- Clean code practices
- Onion Architecture
- Modular project structure


## Getting Started

1. **Prerequisites**
   - .NET 6 SDK
   - SQL Server (LocalDB or full version)

2. **Restore & Build**

    ```bash
    dotnet restore
    dotnet build
    ```
3. **Apply Migrations**

    From the relevant Infrastructure project:
    ```
    dotnet ef database update
    ```

4. **Run the Application**
    ```
    cd src/ServiceHost
    dotnet run
    ```
