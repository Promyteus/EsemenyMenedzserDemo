# Demo Application

This is a full-stack demo application built with a modern tech stack, featuring an Angular frontend, a .NET backend, and an MSSQL database.

---

## 🛠️ Tech Stack & Prerequisites

Before running the application, ensure you have the following installed:

* **Backend:** .NET 10.0
* **Frontend:** Angular 22.0.5 (Node.js v24.18+)
* **Database:** Microsoft SQL Server Express (configured for Localhost with Windows Authentication)

---

## 🚀 Getting Started

### 1. Database Setup
The application connects to a local MSSQL Express instance using Windows Authentication.
* **Server Name:** `localhost\SQLEXPRESS`
* Ensure that your SQL Server is running and Windows Authentication is enabled for your current user.

### 2. Backend Server (.NET)
Navigate to the backend project directory and run the following commands to start the server:

dotnet restore
dotnet run

* **Base URL:** `https://localhost:7087`
* **API Documentation (Scalar):** `https://localhost:7087/scalar/v1`

### 3. Frontend Client (Angular)
Navigate to the frontend project directory and run the following commands to start the development server:

npm install
ng serve

* **Application URL:** `http://localhost:4200/`

---

## 🔐 Demo Credentials

To log in and test the application's functionality, you can use the following technical user account:

* **Email / Username:** `admin@admin.local`
* **Password:** `Asdf123.`

> [!NOTE]
> This account has administrative privileges to showcase all available features in the demo.