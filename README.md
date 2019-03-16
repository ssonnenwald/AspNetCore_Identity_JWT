# ASP.NET Core Identity - JWT Authentication

[![fork this repo](http://githubbadges.com/fork.svg?user=ssonnenwald&repo=AspNetCore_Identity_JWT&style=flat)](https://github.com/ssonnenwald/AspNetCore_Identity_JWT/fork)
![commit activity](https://img.shields.io/github/commit-activity/w/ssonnenwald/AspNetCore_Identity_JWT.svg)
![last commit](https://img.shields.io/github/last-commit/ssonnenwald/AspNetCore_Identity_JWT.svg)
![GitHub](https://img.shields.io/github/license/ssonnenwald/AspNetCore_Identity_JWT.svg)
![GitHub issues](https://img.shields.io/github/issues/ssonnenwald/AspNetCore_Identity_JWT.svg)
![GitHub contributors](https://img.shields.io/github/contributors/ssonnenwald/AspNetCore_Identity_JWT.svg)

## General

This is an implementation of using the ASP.NET Core Identity component to implement JWT Token authentication for a user signing into and out of a application.  There are two projects in this solution, the **AspNetCore.Identity.Data** and the **AspNetCore.Identity.Api**.
The **AspNetCore.Identity.Data** project is just creating the Identity database and extending the IdentityUser with two properties, FirstName and LastName, although it can be extended as needed for the consuming application scenario.
The **AspNetCore.Identity.Data** project uses migrations to create the database setup.  The **AspNetCore.Identity.Api** project is the web api that exposes login, signout, register and refreshtoken respectfully.

## Development

- Visual Studio 2017
- ASP.NET Core 2.2 SDK
- Microsft SQL Server LocalDB
- Entity Framework Core

## Startup Project

The **AspNetCore.Identity.Api** is set as the startup project and when run in the development environment will display the Swagger page for the API.

## Generating the Database

The database used in this implementation is a Microsoft SQL Server LocalDB.  The connection string to the database can be found in each of the projects appsettings.json file.  To generate the Identity database migrations are used.  What will need to be done before the solution is run is to create the database by running the migrations in the **AspNetCore.Identity.Data** project.  When this is done a migrations folder will be created in the project with the migrations.  Initially delete this folder from the project, if the database hasn't been generated.  Next step is to enable migrations and apply it to the database.  You can either use PowerShell or user Visual Studio 2017 NuGet Package Manager console.  Execute the following two commands for enabling and applying migrations to the target database.

```Powershell

- Add-Migration InitialCreate -Project AspNetCore.Identity.Data  -Context ApplicationUserDbContext  

- Update-Database -Project AspNetCore.Identity.Data -Context ApplicationUserDbContext

```

## Testing the API

To test out the API you can use **POSTMAN**.

### Register

#### HTTP Method:  POST

##### Address

https&#58;://localhost:44325/api/Account/register

##### Header

Content-Type:  application/json

##### Body

```Json
{
    "firstName": "John",
    "lastName": "Doe",
    "email": "jdoe@gmail.com",
    "passwordConfirmation": "Password123!",
    "userName": "jdoe",
    "password": "Password123!"
}
```

### Login

#### HTTP Method:  POST

##### Address

https&#58;://localhost:44325/api/Account/login

##### Header

Content-Type:  application/json

Authorization:  bearer '---JWT Token---'

##### Body

```Json
{
  "userName": "jdoe",
  "password": "Password123!"
}
```

### Refresh Token

#### HTTP Method:  POST

##### Address

https&#58;://localhost:44325/api/Account/refreshtoken

##### Header

Content-Type:  application/json

Authorization:  bearer '---JWT Token---'

### Signout

#### HTTP Method:  POST

##### Address

https&#58;://localhost:44325/api/Account/signout

##### Header

Content-Type:  application/json

Authorization:  bearer '---JWT Token---'
