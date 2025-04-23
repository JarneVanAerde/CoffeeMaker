# Setup

Run the following commands:
1.  `docker pull mcr.microsoft.com/mssql/server`
2.  `docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server`

If you don't have docker, you can also change the connection string to connect to a local SSMS.

Inside the [CoffeeMaker.Api/CoffeeMaker.Api.http](https://github.com/JarneVanAerde/CoffeeMaker/blob/a92d967f845a5c4cb50d741556eec902430ee344/CoffeeMaker.Api/CoffeeMaker.Api.http) file, call the seed endpoint to setup the database.
To test if it all works, call the brew-recommendation endpoint.

You're good to go 🎉
