# ClayAssignment
This repository is based on Clay Technical Assignment. It consists of 5 projects a .Net Core Web API project, two simple webapp, one NUnitTest project (for unit testing) and one xUintTest Project (for Integration testing).

## Whats Including in This Repository
I have implemented below features over the **ClayAssignment** repository:

#### AccessControl.Api which Includes: 
* ASP.NET Core Web API application 
* REST API principles, CRUD operations
* MSSQL database connection and configuration
* InMemory database connection and configuration
* SOLID Pattern Implementation
* Repository Pattern Implementation
* Swagger Open API implementation
* JWT Authentication And Authorization with Identity Framework 

#### UnitTests which includes: 
* Using NUnit and Moq framework for unit testing

#### IntegrationTests which includes: 
* Using xUnit framework for integration testing

#### WebApp.Admin which includes: 
* A simple ASP.NET Core MVC Web Application with Bootstrap 5 and Razor template as Admin Manegement UI

#### WebApp.Accessor which includes: 
* A simple ASP.NET Core MVC Web Application with Bootstrap 5 and Razor template for Opening and Closing Doors

#### CICD: 
* Using yaml file configuration for continuous integration and automation testing in GitHub Action

## Run The Project
You will need the following tools:

* [Visual Studio 2019 or later](https://visualstudio.microsoft.com/downloads/)
* [.Net Core 6 or later](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Notes

By default in this project InMemory Database was used. If you want to use MSSQL, in **appsettings.Development.json** file of **AccessControl.Api** project, please change the value of **"UseInMemoryDb"** to **"no"**:
```
"Settings": {
  "UseInMemoryDb": "no"
}
```

Microservices can be launched as below urls:
* **AccessControl.Api**
    - https://localhost:6050/swagger/index.html
    - http://localhost:5000/swagger/index.html
* **WebApp.Admin**
    - https://localhost:6051
    - http://localhost:5001
* **WebApp.Accessor**
    - https://localhost:6052
    - http://localhost:5002

You can use these preconfigured Usernames and Passwords:
* For Admin Role:
    - Username: **admin**
    - Password: **Admin*123**
* For Accessor Role:
    - Accessor1
      - Username: **accessor1**
      - Password: **Accessor1*123**
    - Accessor2
      - Username: **accessor2**
      - Password: **Accessor2*123**
 
 You can use download Postman json collection from link below:
 * [Postman json collection file](https://drive.google.com/file/d/1AY2fTYEbdPEX2QE0ztYOjeIw-tYBsK2o/view?usp=sharing)
 
 This is my 5-minute explanation video about the **AccessControl.Api**:
 * [5-minute explanation video](https://localhost:6051)

> Happy Coding! :thumbsup: :smile: :sunglasses:
