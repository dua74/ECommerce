# 🛍️ ECommerce Project

A complete E-Commerce web application built with ASP.NET Core for the backend and Angular for the frontend.

## 🧱 Project Structure

Backend (ASP.NET Core):
- Ecom.API: The main Web API project.
- Ecom.Core: Contains core business entities and interfaces.
- Ecom.Infrastructure: Implements data access logic using Entity Framework Core.

Frontend (Angular):
- EcomClient: Angular application for the user interface.

## 🚀 Getting Started

### Prerequisites

- .NET 7 SDK: https://dotnet.microsoft.com/en-us/download
- Node.js: https://nodejs.org/
- Angular CLI: npm install -g @angular/cli
- SQL Server or any other configured database

### Backend Setup

1. Open the solution `Ecom.sln` in Visual Studio.
2. Update the `appsettings.json` file with your local database connection string.
3. Apply migrations and update the database (if using EF Core):  
   Update-Database
4. Set `Ecom.API` as the startup project and run it.

### Frontend Setup

cd EcomClient  
npm install  
ng serve --open

The Angular app will be available at http://localhost:4200.

## 📂 Folder Structure

ECommerce/  
├── Ecom.API/                - ASP.NET Core Web API  
├── Ecom.Core/               - Business logic and entities  
├── Ecom.Infrastructure/     - Database and persistence logic  
├── EcomClient/              - Angular frontend  
└── Ecom.sln                 - Visual Studio solution file

## ✨ Features

- User Authentication  
- Product Browsing & Filtering  
- Category Management  
- Responsive UI with Angular & Bootstrap  
- Clean architecture using Repository pattern

## 👤 Author

Dua Helal  
GitHub: https://github.com/dua74

## 📜 License

This project is open source and available under the MIT License.
