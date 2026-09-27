# Fixly

## Project Overview

Fixly is a service marketplace web application developed as part of the **Tuwaiq Academy** program. It connects customers with local service providers such as plumbers, electricians, and cleaners.

Customers can browse and search for service providers by **service category and city**, submit service requests, and track their requests. Service providers can view and manage their incoming requests.

## Features

### Customer

* Browse available service providers.
* Search providers by **service category** and **city**.
* Submit a service request.
* Specify the **problem description, date, and time** for the requested service.
* View and track submitted requests through **My Requests**.

**Customer Home Page**

<img width="1600" height="900" alt="Customer page" src="https://github.com/user-attachments/assets/0190a716-e1cc-4730-aac5-d1bff45051e6" />

**Service Request**

<img width="1600" height="900" alt="Rquest a new service" src="https://github.com/user-attachments/assets/810fd655-edf4-4a6c-a452-7dcb60b31acb" />

**My Requests**

<img width="1600" height="900" alt="My Requests 2" src="https://github.com/user-attachments/assets/eacce59d-fc41-4ff6-98ad-ca3a0a5be41f" />

### Service Provider

* View incoming service requests.
* Review customer request details.
* Manage the status of service requests.

**Incoming Requests**

<img width="1600" height="900" alt="incomig requests" src="https://github.com/user-attachments/assets/eb28700e-8bfa-4c66-8b0c-6c5a9b8032ef" />

## Tech Stack

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* Bootstrap 5 (RTL)
* JavaScript

## Setup & Installation

1. Clone the repository:

```bash
git clone https://github.com/Reham2214/Fixly.git
cd Fixly
```

2. Update the connection string in `appsettings.json` to point to your SQL Server instance.

3. Apply database migrations:

```bash
dotnet ef database update
```

4. Run the application:

```bash
dotnet run
```

5. Open the application in your browser at the URL shown in the terminal.
