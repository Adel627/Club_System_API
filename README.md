
# Club_System_API

This project is a **Club Management Web API** built using **ASP.NET Core Web API**, designed to manage users, memberships, and club services. The API provides user authentication, role-based access control, and Stripe integration for membership payments.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Setup and Installation](#setup-and-installation)

---

## Features
- **User Authentication**: Users can register, log in, and access features based on their roles (admin, member).  
- **Role-based Access Control**: Admins can manage users, memberships, and club services. Members can view and purchase memberships.  
- **Membership Management**: Admins can create, update, and delete memberships.  
- **Service Management**: Admins can manage club services (classes, events, etc.).  
- **Order/Payment Management**: Members can purchase memberships and pay securely via Stripe Checkout.
- **Background Jobs**: Scheduled tasks run automatically for recurring tasks such as:
  - Sending membership expiry notifications
  - Generating reports
  - Updating membership statuses
- **DTO Mapping**: Clean separation between domain models and data transfer objects using Mapster/AutoMapper.

---

## Technologies Used
- **ASP.NET Core Web API**: Backend framework for building the API.  
- **Entity Framework Core**: ORM for database interactions.  
- **SQL Server**: Database to store users, memberships, and payments.  
- **C#**: Programming language used for server-side logic.  
- **Stripe API**: Payment processing.  
- **Swagger / OpenAPI**: API documentation and testing.

---

## Setup and Installation
1. **Clone the repository**:
   ```bash
     git clone https://github.com/Adel627/Club_System_API.git

2. **Navigate to the project folder**:
   ```bash
   cd  Club_System_API

3. **Install dependencies: Make sure you have the required .NET SDK installed. Run the following command to restore dependencies:**:
   ```bash
   dotnet restore

4.  **Database Setup: Update the appsettings.json file with your database connection string. Then, run migrations to set up the database schema:**
    ```bash
    dotnet ef database update

5.  **Run the API:**
    ```bash
    dotnet run

6.  **Access Swagger UI:**
    ```bash
    https://localhost:{port}/swagger

7.  **Testing Protected Routes:**
   
     Use Postman or Swagger UI.
     Include JWT token in the Authorization header for endpoints requiring authentication:
    ```bash
    Authorization: Bearer {your_token_here}
