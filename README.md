# NET Backend service template
This project is a template for the backend services of a web based application.

## Required Tools

- Visual Studio with .NET 5.0 SDK
- Make use of docker, for the following (see docker-compose)
  - [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite) (or an Azure storage account)
  - [Seq](https://getseq.net) (optional, but super handy)
  - [Papercut SMTP](https://github.com/ChangemakerStudios/Papercut-SMTP) SMTP server for email (don't accidentally send live emails!)

## Getting Started

1. Create `appsettings.Development.json` files in API and migrations projects (copy from `appsettings.json`)
2. Open solution 
3. Run the api project
