# Contacts Management Application - Project Overview

## Project Description
This is a full-stack **Contacts Management System** built with modern web technologies. The application allows users to register, authenticate, and manage contacts with categories and subcategories, featuring server-side pagination, filtering, and sorting capabilities.

## Architecture Overview
- **Frontend**: Angular 20.3.0 with standalone components and PrimeNG UI
- **Backend**: ASP.NET Core 9.0 Web API with Clean Architecture
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer tokens with ASP.NET Core Identity
- **Containerization**: Docker Compose for multi-service orchestration

## Project Structure
```
contacts-app/
├── frontend/                 # Angular application
├── backend/                 # ASP.NET Core API
├── compose.yaml            # Root Docker Compose
└── PROJECT_OVERVIEW.md     # This file
```

## Quick Start Guide

### Prerequisites
- Docker and Docker Compose or Podman

### Running the Application

#### Option 1: Docker Compose (Recommended)
1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd contacts-app
   ```

2. **Create backend configuration file**
   ```bash
   # Navigate to backend directory
   cd backend/src
   
   # Create appsettings.json file (see required structure below)
   touch appsettings.json
   ```

3. **Add required configuration to appsettings.json**
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "ConnectionStrings": {
       "DefaultConnection": "Host=postgres;Port=5432;Database=<db_name>;Username=<db_user>;Password=<db_password>;Include Error Detail=true;"
     },
     "Jwt": {
       "Key": "<your-secret-jwt-key-here-must-be-at-least-32-characters-long>",
       "Issuer": "<reverse-proxy-url>",
       "Audience": "<reverse-proxy-url>"
     }
   }
   ```

4. **Start all services**
   ```bash
   # From project root
   docker-compose up --build
   ```

5. **Access the application**
   - Frontend: https://localhost
   - Backend API: https://localhost/api
   - Health Check: https://localhost/api/health

#### Option 2: Local Development
1. **Backend Setup**
   ```bash
   cd backend/src
   # Create appsettings.json with the structure above
   dotnet restore
   dotnet run
   ```

2. **Frontend Setup**
   ```bash
   cd frontend
   npm install
   npm start
   ```

3. **Database Setup**
   - Ensure PostgreSQL is running
   - Update connection string in appsettings.json
   - Migrations will run automatically on startup

## Required Configuration

### Backend appsettings.json Structure
The `backend/src/appsettings.json` file is **required** and contains sensitive configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Port=5432;Database=<db_name>;Username=<db_user>;Password=<db_password>;Include Error Detail=true;"
  },
  "Jwt": {
    "Key": "<your-secret-jwt-key-here-must-be-at-least-32-characters-long>",
    "Issuer": "<reverse-proxy-url>",
    "Audience": "<reverse-proxy-url>"
  }
}
```

**Important Notes**:
- This file is **gitignored** for security reasons
- The JWT Key must be at least 32 characters long
- For production, use strong, randomly generated keys
- Update Issuer/Audience URLs to match reverse proxy's location, e.g. "https://localhost"

## Detailed Documentation

### Frontend Documentation
📖 **[Angular Frontend Overview](./frontend/ANGULAR_PROJECT_OVERVIEW.md)**
- Complete Angular application architecture
- Module structure and components
- Services, guards, and interceptors
- Routing and navigation
- State management and data flow

### Backend Documentation
📖 **[ASP.NET Core Backend Overview](./backend/ASP_NET_CORE_BACKEND_OVERVIEW.md)**
- Clean Architecture implementation
- Feature-based organization
- Authentication and authorization
- Database design and Entity Framework
- API design and caching strategies
