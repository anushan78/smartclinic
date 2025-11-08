🩺 SmartClinic – Cloud-Based Appointment & Notification System

SmartClinic is a cloud-native, multi-tenant appointment management system built with .NET 8, integrated with Azure services for scalability, reliability, and automation.

🌐 Features

- Multi-tenant support (each clinic as a tenant)
- Secure authentication via Azure AD
- Appointment management API with Azure SQL
- Background notifications via Service Bus and Azure Functions
- Secrets managed with Key Vault
- Observability via Application Insights
- Automated deployment via GitHub Actions CI/CD

🏗️ Architecture Overview

graph TD
A[React Frontend] --> B[.NET 8 Web API]
B --> C[Azure SQL Database]
B --> D[Azure Service Bus]
D --> E[Azure Function (Notification Sender)]
B --> F[Azure Key Vault]
B --> G[Azure App Insights]
F --> B

☁️ Azure Services Used

| Service              | Purpose                                     |
| -------------------- | ------------------------------------------- |
| Azure App Service    | Host .NET API & frontend                    |
| Azure SQL Database   | Store clinic, patient, and appointment data |
| Azure Service Bus    | Handle async messaging                      |
| Azure Functions      | Send reminders/emails asynchronously        |
| Azure Key Vault      | Store secrets and connection strings        |
| Azure AD             | Identity & authentication                   |
| Application Insights | Monitoring and logging                      |
| GitHub Actions       | CI/CD automation                            |

🧰 Tech Stack

- .NET 8 (C#) – Web API + Functions
- EF Core 8 – ORM
- React – Frontend
- Azure Bicep – Infrastructure as Code
- GitHub Actions – DevOps automation

🚀 Getting Started

Run locally:

git clone https://github.com/<your-username>/smartclinic-azure-dotnet.git
cd smartclinic-azure-dotnet/src
dotnet build
dotnet run --project SmartClinic.Api

Deploy to Azure:

az login
az deployment group create -g SmartClinicRG -f infra/main.bicep

🧪 CI/CD Pipeline

- Every push triggers build & test workflows.
- Main branch automatically deploys to Azure App Service using GitHub Actions.

📸 Demo Video / Screenshots

Tobe Added

📄 License

This project is licensed under the MIT License.
