# ASP.NET Core Contacts API - High-Level Overview

## Project Purpose
This ASP.NET Core Web API is a **Contacts Management System Backend** that provides:
- User authentication and authorization with JWT tokens
- RESTful API endpoints for contact CRUD operations
- Contact categorization and subcategorization
- Server-side pagination, sorting, and filtering
- Data validation and business logic enforcement

The application follows Clean Architecture principles with feature-based organization, dependency injection, and modern .NET patterns.

## Technology Stack
- **.NET 9.0** (Latest LTS version)
- **ASP.NET Core Web API** (RESTful API framework)
- **Entity Framework Core 9.0.10** (ORM with PostgreSQL)
- **PostgreSQL** (Primary database)
- **AutoMapper 14.0.0** (Object-to-object mapping)
- **JWT Bearer Authentication** (Token-based security)
- **ASP.NET Core Identity** (User management)
- **Npgsql.EntityFrameworkCore.PostgreSQL** (PostgreSQL provider)

## Application Architecture

### Core Configuration
- **Vertically sliced, layered**: Feature-based organization with clear separation of technical concerns
- **Dependency Injection**: Built-in .NET DI container with scoped services
- **JWT Authentication**: Stateless token-based authentication
- **Database Migrations**: Automatic migration execution on startup
- **CORS Configuration**: Cross-origin resource sharing support
- **AutoMapper**: Automatic object mapping between DTOs and entities

## Feature Structure

### 1. **Authentication Feature** (`/Features/Authentication/`)
**Purpose**: Handles user authentication and JWT token management

**API Layer**:
- `AuthController`: RESTful endpoints for login and registration
  - `POST /api/auth/register`: User registration with validation
  - `POST /api/auth/login`: User authentication with JWT token response

**Application Layer**:
- `AuthService`: Core authentication business logic
  - User registration with password complexity validation
  - User login with credential verification
  - Email uniqueness validation
  - Password policy enforcement (8+ chars, numbers, special characters)

- `TokenService`: JWT token generation and management
  - JWT token creation with email claims
  - Configurable expiration (30 minutes)
  - Symmetric key signing with HMAC SHA256

**Data Layer**:
- `AuthDbContext`: Identity framework database context
  - Extends `IdentityDbContext` for user management
  - PostgreSQL integration for user storage

**DTOs**:
- `LoginRequestDto`, `LoginResponseDto`
- `RegisterRequestDto`, `RegisterResponseDto`

**Key Features**:
- ASP.NET Core Identity integration
- JWT token-based authentication
- Password complexity validation
- Email uniqueness enforcement
- Secure token generation with configurable settings

### 2. **Contacts Feature** (`/Features/Contacts/`)
**Purpose**: Core business logic for contact management

**API Layer**:
- `ContactsController`: RESTful CRUD endpoints
  - `GET /api/contacts`: Paginated contact list with filtering/sorting
  - `GET /api/contacts/{id}`: Individual contact details
  - `POST /api/contacts`: Create new contact
  - `PUT /api/contacts/{id}`: Update existing contact
  - `DELETE /api/contacts/{id}`: Delete contact

- `CategoriesController`: Category and subcategory management
  - `GET /api/categories`: Retrieve all available categories
  - `GET /api/categories/{id}/subcategories`: Get subcategories for a specific category

- `HealthController`: Application health monitoring
  - `GET /api/health`: Simple health check endpoint returning "Healthy"

**Application Layer**:
- `ContactService`: Contact business logic and validation
  - Contact creation with category/subcategory handling
  - Contact updates with email uniqueness validation
  - Password validation for contact passwords
  - Date of birth validation (no future dates)
  - Category existence validation

**Data Layer**:
- `ApplicationDbContext`: Main application database context
- `ContactRepository`: Specialized contact data access
  - Advanced filtering by multiple criteria
  - Server-side pagination with sorting
  - Email-based lookups
  - Category and subcategory inclusion

**Domain Models**:
- `Contact`: Core contact entity with validation attributes
- `Category`: Contact categorization
- `Subcategory`: Optional contact subcategorization

**DTOs**:
- `ContactDto`: Basic contact information
- `ContactDetailsDto`: Full contact details with relationships
- `CategoryDto`, `SubcategoryDto`: Category representations

**Key Features**:
- Server-side pagination and filtering
- Multi-column sorting capabilities
- Category/subcategory hierarchy management
- Email uniqueness enforcement
- Comprehensive validation rules
- Category and subcategory API endpoints for frontend consumption
- Health check endpoint for monitoring and load balancer integration

## Shared Infrastructure

### 1. **Data Layer** (`/Shared/Data/`)
**Purpose**: Common data access patterns and abstractions

**Components**:
- `UnitOfWork`: Transaction management and repository coordination
  - Centralized database operations
  - Repository pattern implementation
  - Transaction boundary management

- `RepositoryBase<T>`: Generic repository with common operations
  - CRUD operations for any entity
  - Async/await pattern throughout
  - Expression-based querying

- `PagedList<T>`: Pagination result wrapper
  - Total count and page information
  - Current page and page size tracking
  - Total pages calculation

**Extensions**:
- `ToPagedListAsync`: LINQ extension for pagination
- Database query optimization

### 2. **Application Layer** (`/Shared/Application/`)
**Purpose**: Common application patterns and utilities

**Components**:
- `ServiceResult<T>`: Standardized service response pattern
  - Success/failure indication
  - Error collection management
  - Type-safe result handling

### 3. **API Layer** (`/Shared/Api/`)
**Purpose**: Common API infrastructure and utilities

**Components**:
- `ApiControllerBase`: Base controller with common configuration
  - Standard API routing (`/api/[controller]`)
  - Common controller attributes

- `HttpService`: HTTP response utilities
  - Pagination header management
  - Response standardization

- `PaginationHeader`: Pagination metadata for client consumption

### 4. **Configuration** (`/Configuration/`)
**Purpose**: Application configuration and mapping

**Components**:
- `AutoMapperProfiles`: Object mapping configuration
  - Entity to DTO mappings
  - Bidirectional mapping support
  - Complex relationship handling

## Database Architecture

### Entity Relationships
```
IdentityUser (ASP.NET Identity)
    ↓ (separate context)
Contact
    ├── Category (required)
    └── Subcategory (optional)
```

### Database Contexts
1. **AuthDbContext**: User authentication and identity
   - Extends `IdentityDbContext`
   - Manages user accounts and authentication

2. **ApplicationDbContext**: Business data
   - Contact management
   - Category and subcategory data
   - Business logic entities

### Key Constraints
- **Unique Email**: Contact emails must be unique
- **Unique Category Names**: Category names must be unique
- **Required Relationships**: Contacts must have a category
- **Optional Subcategories**: Subcategories are optional

## Security Architecture

### Authentication Flow
1. User submits credentials to `/api/auth/login`
2. `AuthService` validates credentials against Identity
3. `TokenService` generates JWT token with email claim
4. Client stores token for subsequent requests
5. JWT middleware validates tokens on protected endpoints

### Authorization
- **JWT Bearer Authentication**: Stateless token authentication reduces server-side storage, simplifies authentication and authorization handling
- **Protected Endpoints**: Most contact operations require authentication
- **Token Validation**: Issuer, audience, lifetime, and signing key validation
- **Configurable Expiration**: 30-minute token lifetime

### Password Security
- **Complexity Requirements**: 8+ characters, numbers, special characters
- **Identity Framework**: Built-in password hashing and validation
- **Contact Passwords**: Separate password field for contact records

## API Design

### RESTful Endpoints
```
Authentication:
POST /api/auth/register
POST /api/auth/login

Contacts:
GET    /api/contacts?pageNumber=1&pageSize=10&sortBy=lastname&sort=asc
GET    /api/contacts/{id}
POST   /api/contacts
PUT    /api/contacts/{id}
DELETE /api/contacts/{id}

Categories:
GET    /api/categories
GET    /api/categories/{id}/subcategories

Health:
GET    /api/health
```

### Query Parameters
- **Pagination**: `pageNumber`, `pageSize`
- **Sorting**: `sortBy`, `sort` (asc/desc)
- **Filtering**: `firstName`, `lastName`, `email`, `phone`, `category`, `subcategory`

### Response Patterns
- **Success Responses**: HTTP 200/201 with data and 204 without content
- **Error Responses**: HTTP 400/404 with error details
- **Pagination Headers**: Custom pagination metadata
- **Validation Errors**: Detailed field-level error messages

## Data Flow

### Contact Creation Flow
1. Client sends `ContactDetailsDto` to `POST /api/contacts`
2. `ContactService.AddContact()` validates input
3. Category lookup and validation
4. Subcategory creation/lookup if provided
5. Contact entity creation with relationships
6. Database persistence via UnitOfWork
7. Success response with created contact data

### Contact Retrieval Flow
1. Client requests contacts with query parameters
2. `ContactRepository.GetAllAsync()` applies filters and sorting
3. Server-side pagination calculation
4. Database query execution with includes
5. AutoMapper converts entities to DTOs
6. Pagination headers added to response
7. Paginated contact list returned

### Authentication Flow
1. Client submits login credentials
2. `AuthService.Login()` validates against Identity
3. `TokenService.CreateJwtToken()` generates JWT
4. Token returned in response
5. Client includes token in Authorization header
6. JWT middleware validates token on protected routes

## Error Handling

### Validation Errors
- **Model Validation**: Automatic model state validation
- **Business Logic Validation**: Custom validation in services
- **Database Constraints**: Entity framework constraint validation
- **Error Response Format**: Consistent error message structure

### Exception Handling
- **Global Exception Handling**: Development vs production error pages
- **Database Exceptions**: Transaction rollback and error reporting
- **Authentication Errors**: Secure error messages without information leakage

## Performance Considerations

### Database Optimization

- **Indexed Fields**: Database indexes on frequently queried fields

### Caching Strategy
**Potential Caching Implementations**:

1. **Controller-Level Caching**:
   - `[ResponseCache]` attributes for HTTP response caching
   - `MemoryCache` for in-memory data caching
   - `IDistributedCache` for distributed caching scenarios
   - Cache invalidation strategies for data consistency

2. **API Response Caching**:
   - HTTP `Cache-Control` headers for client-side caching
   - `ETag` headers for conditional requests and cache validation
   - `Last-Modified` headers for time-based cache invalidation
   - Vary headers for cache key differentiation

3. **Database Query Caching**:
   - Entity Framework query result caching
   - Repository-level caching for frequently accessed data
   - Category and subcategory data caching (relatively static)
   - Contact list pagination result caching

4. **Distributed Caching**:
   - Redis integration for multi-instance deployments
   - SQL Server distributed cache for enterprise scenarios
   - Cache-aside pattern implementation
   - Write-through and write-behind caching strategies

5. **Client-Side Caching**:
   - HTTP caching headers for static resources
   - API response caching directives
   - Conditional request support (If-None-Match, If-Modified-Since)
   - Cache busting strategies for data updates

## Development Setup

### Configuration
- **Connection Strings**: PostgreSQL database configuration
- **JWT Settings**: Token generation and validation parameters
- **Identity Options**: Password policy configuration

### Database Management
- **Entity Framework Migrations**: Version-controlled database schema
- **Automatic Migration**: Database updates on application startup
- **Seed Data**: Initial category and user data

### Docker Integration
- **PostgreSQL Container**: Database service containerization
- **Application Container**: API service containerization
- **Docker Compose**: Multi-service orchestration

## Integration Points

### Frontend Integration
- **CORS Configuration**: The request handling happens on a single origin by design (thanks to the nginx serving as a reverse proxy) so no CORS configuration required - enhanced security againt CSRF
- **JWT Token Exchange**: Secure authentication flow
- **RESTful API**: Standard HTTP methods and status codes
- **Pagination Support**: Server-side pagination handling

### External Dependencies
- **PostgreSQL Database**: Primary data storage
- **ASP.NET Core Identity**: User management framework
- **JWT Libraries**: Token generation and validation
- **AutoMapper**: Object mapping and transformation

This architecture provides a robust, scalable, and maintainable contacts management API with modern .NET best practices, comprehensive security, and efficient data handling.
