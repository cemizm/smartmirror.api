# Smart Mirror REST API

REST API interface for the Smart Mirror Database

## Prerequisites
* [.NET Core](https://www.microsoft.com/net/core)

## Run Tests
In order to run the tests, you have to restore the dependencies of the project. This can be done using the .NET cli.
```
cd WebApi.Test
dotnet restore
dotnet test
```

## Run Embedded Server
```
cd WebApi
dotnet restore
dotnet run
```