# Welcome to the Excella Vending Machine demo!
This demo shows a working example of a small project with unit tests at each level.

# How to Run this Application

## Prerequisites
To run the project, you'll need to have some basic items set up or installed.

* **Internet.** You'll need an internet connection so that you can restore nuget packages.
* **Visual Studio**. You'll need Visual Studio to run the examples
* **IIS**. Because the acceptance tests cannot run at the same time the app is running in IIS express, we have to deploy to IIS itself. Therefore, you'll need to have IIS Installed.
* **SQL Server**. The acceptance tests and web application use a database, which this example assumes is a SQL Server database.
* **Selenium Chrome Driver**. We'll need this to run instances of chrome on our machine.
 
## Getting Started

### Adding the Selenium Chrome Driver to the PATH variable.

### Creating the IIS Vrtual Directory
TODO.

### Running the Migration to Deploy the Database
TODO.

### Adding Permissions for the IIS Account to the Database 
TODO.

### Adding the Initial Payment row to the Database
TODO.

```
  SET IDENTITY_INSERT dbo.Payment ON
  INSERT INTO dbo.Payment
    (ID, Value)
  VALUES 
    (1, 0)
  SET IDENTITY_INSERT dbo.Payment OFF
```

## Running the tests
TODO.