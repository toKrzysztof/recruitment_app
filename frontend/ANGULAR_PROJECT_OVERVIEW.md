# Angular Contacts Application - High-Level Overview

## Project Purpose
This Angular application is a **Contacts Management System** that allows users to:
- Register and authenticate users
- Create, read, update, and delete contacts
- Manage contact categories and subcategories
- Search and filter contacts with pagination
- View detailed contact information

The application follows modern Angular best practices with standalone components, reactive forms, and a clean modular architecture.

## Technology Stack
- **Angular 20.3.0** (Latest version with standalone components)
- **PrimeNG 20.2.0** (UI component library)
- **PrimeFlex** (CSS utility framework)
- **RxJS 7.8.0** (Reactive programming)
- **JWT Decode** (Token handling)
- **SCSS** (Styling)
- **TypeScript 5.9.2**

## Application Architecture

### Core Configuration
- **Standalone Components**: All components are standalone (no NgModules)
- **Zoneless Change Detection**: Uses Angular's new zoneless change detection
- **Reactive Forms**: All forms use Angular Reactive Forms
- **HTTP Interceptors**: Automatic token injection for API calls
- **Lazy Loading**: Contact form and details components are lazy-loaded

## Module Structure

### 1. **Auth Module** (`/modules/auth/`)
**Purpose**: Handles user authentication and registration

**Components**:
- `LoginFormComponent`: User login with email/password validation
- `RegisterFormComponent`: User registration with password complexity validation

**Key Features**:
- JWT token-based authentication
- Password complexity validation (8+ chars, numbers, special characters)
- Email uniqueness validation
- Form validation with custom error messages

**Models**:
- `LoginRequestDto`, `LoginResponseDto`
- `RegisterRequestDto`, `RegisterResponseDto`
- `JwtDecoded` (JWT token structure)

### 2. **Contacts Module** (`/modules/contacts/`)
**Purpose**: Core business logic for contact management

**Components**:
- `ContactListComponent`: Paginated table with search/filter capabilities
- `ContactFormComponent`: Create/edit contacts with category selection
- `ContactDetailsComponent`: View detailed contact information

**Services**:
- `ContactService`: CRUD operations for contacts
- `CategoryService`: Manages contact categories and subcategories

**Resolvers**:
- `contactDetailsResolver`: Pre-loads contact data for detail/edit views

**Models**:
- `ContactDto`, `ContactDetailsDto`, `ContactForm`
- `CategoryDto`, `SubcategoryDto`

**Key Features**:
- Lazy loading for form and details components
- Server-side pagination, sorting, and filtering
- Category/subcategory hierarchy (with "Other" category support)
- Form validation

### 3. **Core Module** (`/modules/core/`)
**Purpose**: Shared application infrastructure

**Services**:
- `AuthService`: Centralized authentication management
  - Token storage and validation
  - User email extraction from JWT
  - Login/logout functionality
  - Token expiration checking

**Guards**:
- `loggedInGuard`: Protects routes requiring authentication
  - Redirects to login if not authenticated
  - Clears invalid tokens

**Interceptors**:
- `tokenInterceptor`: Automatically adds JWT tokens to API requests
  - Only applies to application API calls
  - Excludes third-party API calls

**Models**:
- `JwtDecoded`: JWT token structure
- Authentication DTOs

### 4. **Public Module** (`/modules/public/`)
**Purpose**: Public pages not requiring authentication

**Components**:
- `NotFound`: 404 error page

### 5. **Shared Module** (`/modules/shared/`)
**Purpose**: Reusable components and utilities

**Components**:
- `ValidationErrorsComponent`: Displays form validation errors
  - Supports multiple error types (required, email, minlength, etc.)
  - Computed properties for reactive error display

**Validators**:
- `passwordComplexityValidator`: Custom password validation
  - Minimum 8 characters
  - Requires numbers and special characters

**Models**:
- `FormErrors`: Type definitions for form validation errors

## Routing Architecture

### Route Structure
```typescript
/ → redirects to /login
/login → LoginFormComponent
/register → RegisterFormComponent
/contacts → ContactListComponent
/contacts/new → ContactFormComponent (lazy-loaded, protected)
/contacts/:id/edit → ContactFormComponent (lazy-loaded, protected, resolved)
/contacts/:id → ContactDetailsComponent (lazy-loaded, protected, resolved)
/404 → NotFound
/** → redirects to /404
```

### Route Protection
- **Public Routes**: `/login`, `/register`, `/404`
- **Protected Routes**: All `/contacts/*` routes use `loggedInGuard`
- **Lazy Loading**: Contact form and details components
- **Data Resolution**: Contact details pre-loaded via resolver

### Navigation Flow
1. **Unauthenticated**: Redirected to login
2. **Login Success**: Navigate to contacts list
3. **Contact Management**: CRUD operations with form validation
4. **Error Handling**: 404 for invalid routes/contacts

## Services Architecture

### 1. **AuthService**
- **State Management**: BehaviorSubject for user email
- **Token Management**: localStorage-based token storage
- **Authentication**: Login/register API calls
- **Authorization**: Token validation and expiration checking

### 2. **ContactService**
- **CRUD Operations**: Full contact lifecycle management
- **Pagination**: Server-side pagination with HTTP params
- **Filtering**: Multi-column filtering support
- **Sorting**: Dynamic sorting capabilities

### 3. **CategoryService**
- **Category Management**: Fetch all categories
- **Subcategory Hierarchy**: Load subcategories by category ID
- **Dynamic Loading**: Categories loaded on form initialization

## Data Flow

### Authentication Flow
1. User submits login/register form
2. AuthService makes API call
3. JWT token stored in localStorage
4. Token interceptor adds Bearer token to subsequent requests
5. User email extracted and stored in BehaviorSubject

### Contact Management Flow
1. Contact list loads with pagination
2. User can search/filter/sort contacts
3. Create/Edit forms load categories dynamically
4. Form validation ensures data integrity
5. Success/error messages via PrimeNG Toast

### Error Handling
- **Form Validation**: Real-time validation with custom error messages
- **API Errors**: Toast notifications for user feedback
- **Route Errors**: 404 page for invalid routes/contacts
- **Token Expiration**: Automatic logout and redirect to login

## Key Features

### 1. **Modern Angular Patterns**
- Standalone components (no NgModules)
- Zoneless change detection
- Signal-based reactive programming
- Functional guards and interceptors

### 2. **User Experience**
- Real-time form validation
- Loading states and error handling
- Toast notifications for user feedback

### 3. **Performance**
- Lazy loading for heavy components
- Server-side pagination and filtering
- Efficient change detection
- Optimized bundle size

### 4. **Security**
- JWT-based authentication
- Route protection with guards
- Automatic token injection
- Token expiration handling

### 5. **Data Management**
- Reactive forms with validation
- (pseudo) Type-safe API communication
- Centralized state management
- Error boundary handling

## Development Setup
- **Proxy Configuration**: API calls proxied to backend
- **Environment Configuration**: Development/production settings
- **Build Configuration**: Optimized for production deployment
- **Testing**: Jasmine/Karma test setup

## Integration Points
- **Backend API**: RESTful API communication
- **JWT Authentication**: Token-based security
- **PrimeNG Components**: Rich UI components

This architecture provides a scalable, maintainable, and user-friendly contacts management system with modern Angular best practices.
