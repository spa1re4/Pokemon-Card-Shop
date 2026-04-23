#  Pokemon Card Shop (ASP.NET Core MVC)

##  Description

This project is a web application of an online Pokémon card store developed using **ASP.NET Core MVC** and **PostgreSQL**.

The application allows users to browse products, add items to a shopping cart, and place orders. It also includes authentication and role-based access control for administrators.

---

##  Features

###  User
- Browse product catalog
- View product details
- Add products to cart
- Place an order (only for authenticated users)

###  Admin
- Create new products
- Edit products
- Delete products
- View all orders

---

##  Technologies

- ASP.NET Core MVC
- C#
- Entity Framework Core
- PostgreSQL
- ASP.NET Identity
- Bootstrap

---

##  Architecture

The application follows the **MVC pattern**:

- **Model** — data and database structure
- **View** — user interface
- **Controller** — application logic

---

##  Database

Database: **PostgreSQL**

Main tables:
- `PokemonCards` — products
- `Orders` — orders
- `OrderItems` — items in orders
- `AspNetUsers` — users
- `AspNetRoles` — roles

Entity Framework Core is used with migrations to manage the database.

---

##  Authentication

Authentication is implemented using **ASP.NET Identity**.

Roles:
- **User** — can place orders
- **Admin** — can manage products and view orders

Default admin account:

---

##  Shopping Cart

- Implemented using **Session**
- Stores temporary data before checkout
- Calculates total price

---

##  Order Processing

- Only authenticated users can place orders
- Orders are saved to the database
- Each order contains multiple items

---

##  UI

- Built using Bootstrap
- Custom styles in `site.css`
- Responsive layout

---

##  Setup Instructions
1. Clone the repository
2. Configure PostgreSQL connection in `appsettings.json`
3. Run migrations:

```bash
Add-Migration InitialCreate
Update-Database
