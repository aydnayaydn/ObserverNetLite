# ObserverNetLite

A modern, scalable network monitoring and management system built with .NET 8 and Clean Architecture principles.

## 🚀 Features

- **User Management**: Complete CRUD operations with role-based access control
- **Role & Permission System**: Flexible permission-based authorization
- **Menu Management**: Hierarchical menu structure with dynamic routing
- **JWT Authentication**: Secure token-based authentication
- **Email Integration**: Password reset functionality with MailKit
- **Clean Architecture**: Separated concerns with clear boundaries
- **RESTful API**: Well-structured minimal API endpoints
- **PostgreSQL Database**: Robust data persistence with Entity Framework Core

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) (recommended)

## 🏗️ Project Structure

```
ObserverNetLite/
├── ObserverNetLite.API/          # Web API Layer (Minimal APIs)
│   ├── src/
│   │   ├── Endpoints/            # API endpoint definitions
│   │   ├── Extensions/           # Service extensions
│   │   ├── Filters/              # Action filters
│   │   └── Middlewares/          # Custom middlewares
│   ├── appsettings.json          # Configuration
│   └── Program.cs                # Application entry point
│
├── ObserverNetLite.Core/         # Domain Layer
│   └── src/
│       ├── Entities/             # Domain entities
│       ├── Abstractions/         # Interfaces
│       ├── Exceptions/           # Custom exceptions
│       └── Helpers/              # Utility classes
│
├── ObserverNetLite.Infrastructure/  # Data Access Layer
│   ├── src/
│   │   ├── Data/                 # DbContext and configurations
│   │   ├── Repositories/         # Repository implementations
│   │   └── SeedData.cs           # Database seeding
│   └── Migrations/               # EF Core migrations
│
└── ObserverNetLite.Service/      # Application Layer
    └── src/
        ├── Services/             # Business logic services
        ├── DTOs/                 # Data transfer objects
        ├── Mappings/             # AutoMapper profiles
        └── Settings/             # Configuration settings
```

## 🛠️ Technology Stack

### Backend
- **.NET 8.0**: Latest LTS version of .NET
- **ASP.NET Core**: Minimal APIs for lightweight endpoints
- **Entity Framework Core 9**: ORM for database operations
- **PostgreSQL**: Primary database
- **AutoMapper**: Object-to-object mapping
- **MailKit**: Email functionality (SMTP)
- **BCrypt.Net**: Password hashing

### Authentication & Security
- **JWT (JSON Web Tokens)**: Stateless authentication
- **Role-Based Access Control (RBAC)**: Fine-grained permissions
- **CORS**: Cross-origin resource sharing

## 📦 Installation

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/ObserverNetLite.git
cd ObserverNetLite
```

### 2. Configure Database
Update the connection string in `ObserverNetLite.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User ID=postgres;Password=yourpassword;Host=localhost;Port=5432;Database=ObserverNetLiteDb;"
  }
}
```

### 3. Apply Migrations
```bash
cd ObserverNetLite.Infrastructure
dotnet ef database update --startup-project ../ObserverNetLite.API
```

### 4. Configure Email (Optional)
For password reset functionality, configure email settings in `appsettings.json`:

```json
{
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "FromEmail": "your-email@gmail.com",
    "FromName": "ObserverNetLite",
    "EnableSsl": true
  }
}
```

**For Gmail:** Create an [App Password](https://support.google.com/accounts/answer/185833) and save it to `mailpassword.txt` in the root directory.

### 5. Run the Application
```bash
cd ObserverNetLite.API
dotnet run
```

The API will be available at: `http://localhost:5258`

Swagger documentation: `http://localhost:5258/swagger`

## 🔑 Default Credentials

After database seeding, you can login with:

- **Admin User**
  - Username: `admin`
  - Password: `Admin123!`

- **Guest User**
  - Username: `guest`
  - Password: `Guest123!`

## 📚 API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `POST /api/auth/forgot-password` - Request password reset
- `POST /api/auth/reset-password` - Reset password with token

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### Roles
- `GET /api/roles` - Get all roles
- `GET /api/roles/{id}` - Get role by ID
- `GET /api/roles/{id}/permissions` - Get role permissions
- `POST /api/roles` - Create role
- `PUT /api/roles/{id}` - Update role
- `DELETE /api/roles/{id}` - Delete role
- `POST /api/roles/{id}/assign-permissions` - Assign permissions to role

### Permissions
- `GET /api/permissions` - Get all permissions
- `GET /api/permissions/category/{category}` - Get permissions by category

### Menus
- `GET /api/menus` - Get all menus
- `GET /api/menus/{id}` - Get menu by ID
- `GET /api/menus/hierarchy` - Get hierarchical menu structure
- `GET /api/menus/role/{roleId}` - Get menus for role
- `POST /api/menus` - Create menu
- `PUT /api/menus/{id}` - Update menu
- `DELETE /api/menus/{id}` - Delete menu

## 🔐 Permission System

The application uses a flexible permission-based access control system. See [PERMISSION_SYSTEM.md](PERMISSION_SYSTEM.md) for detailed information.

### Permission Categories

- **Dashboard**: `view_dashboard`
- **Users**: `view_users`, `create_user`, `edit_user`, `delete_user`
- **Roles**: `view_roles`, `create_role`, `edit_role`, `delete_role`
- **Menus**: `view_menus`, `create_menu`, `edit_menu`, `delete_menu`
- **Special**: `assign_permissions`

### Role Examples

**Admin Role** (All permissions)
```
✓ All view permissions
✓ All create permissions
✓ All edit permissions
✓ All delete permissions
✓ assign_permissions
```

**Guest Role** (Read-only)
```
✓ view_dashboard
✓ view_users
✓ view_roles
✓ view_menus
```

## 🗄️ Database Schema

### Core Tables
- **Users**: User accounts
- **Roles**: User roles (Admin, Guest, etc.)
- **Permissions**: Available permissions
- **UserRoles**: Many-to-many relationship between users and roles
- **RolePermissions**: Many-to-many relationship between roles and permissions
- **Menus**: Navigation menu items

### Relationships
```
Users ────< UserRoles >──── Roles
                              │
                              └──< RolePermissions >──── Permissions
```

## 🧪 Development

### Build
```bash
dotnet build
```

### Run Tests (if available)
```bash
dotnet test
```

### Watch Mode (Auto-rebuild on changes)
```bash
dotnet watch run --project ObserverNetLite.API
```

### Create New Migration
```bash
cd ObserverNetLite.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../ObserverNetLite.API
```

## 📝 Configuration

### JWT Settings
```json
{
  "JwtSettings": {
    "Key": "YourSuperSecretKeyHereAtLeast32Characters",
    "Issuer": "ObserverNetLite",
    "Audience": "ObserverNetLiteClients",
    "DurationInMinutes": 60
  }
}
```

### Email Settings
```json
{
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "FromEmail": "your-email@gmail.com",
    "FromName": "ObserverNetLite",
    "EnableSsl": true
  },
  "PasswordResetSettings": {
    "ResetUrl": "http://localhost:3000/reset-password"
  }
}
```

## 🔧 Troubleshooting

### Database Connection Issues
- Ensure PostgreSQL is running
- Verify connection string in `appsettings.json`
- Check PostgreSQL user permissions

### Email Issues
- For Gmail: Enable 2-factor authentication and create an App Password
- Save the App Password in `mailpassword.txt` (no spaces)
- Ensure port 587 is not blocked by firewall

### Migration Issues
```bash
# Reset database (WARNING: This will delete all data)
dotnet ef database drop --startup-project ../ObserverNetLite.API
dotnet ef database update --startup-project ../ObserverNetLite.API
```

## 🚀 Deployment

### Production Checklist
- [ ] Update `appsettings.Production.json` with production settings
- [ ] Change JWT secret key
- [ ] Configure production database connection
- [ ] Set up HTTPS/SSL certificates
- [ ] Configure email service credentials
- [ ] Enable application logging
- [ ] Set up reverse proxy (nginx/IIS)
- [ ] Configure CORS for production domains

### Publish
```bash
dotnet publish -c Release -o ./publish
```

## 📖 Additional Documentation

- [Permission System Guide](PERMISSION_SYSTEM.md) - Detailed permission system documentation

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Authors

- Your Name - Initial work

## 🙏 Acknowledgments

- Built with .NET 8
- Uses Clean Architecture principles
- Inspired by modern API design patterns
