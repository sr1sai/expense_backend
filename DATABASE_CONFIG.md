# Database Configuration Guide

## Overview
The application uses a templated connection string approach that supports both development and production environments.

## Configuration Files

### DatabaseSettings.json (Development Template)
Contains the connection string template and default settings for development.

### DatabaseSettings.prod.json (Production Template)
Contains the connection string template optimized for production with connection pooling enabled.

### secret.database.json (Development Only)
**?? This file should be added to .gitignore and never committed to version control.**

Contains actual database credentials for development:
```json
{
  "Host": "your-dev-database-host",
  "Port": 5432,
  "Database": "your_database_name",
  "Username": "your_username",
  "Password": "your_password",
  "SSLMode": "Require",
  "CommandTimeout": 30,
  "ApplicationName": "expense_backend"
}
```

## Environment Variables (Production)

In production, the following environment variables must be set:

- `HOST` - Database host address
- `PORT_DB` - Database port (default: 5432)
- `DATABASE` - Database name
- `USERNAME` - Database username
- `PASSWORD` - Database password
- `SSL_MODE` - SSL mode (default: Require)
- `COMMAND_TIMEOUT` - Command timeout in seconds (default: 30)
- `APPLICATION_NAME` - Application name for connection tracking (default: expense_backend)

### Setting Environment Variables in Render

1. Go to your service in the Render Dashboard
2. Navigate to the "Environment" tab
3. Add each environment variable with its corresponding value
4. For sensitive values (PASSWORD, USERNAME), mark them as secret

## How It Works

1. **Development Mode**:
   - Loads `DatabaseSettings.json` for the connection template
   - Loads `secret.database.json` for actual database credentials
   - Replaces template placeholders with values from `secret.database.json`
   - Creates the final connection string

2. **Production Mode**:
   - Loads `DatabaseSettings.prod.json` for the connection template
   - Reads environment variables for database credentials
   - Replaces template placeholders with environment variable values
   - Creates the final connection string

## Connection String Template

The template uses placeholders that get replaced at runtime:

```
Host={HOST};Port={PORT};Database={DATABASE};Username={USERNAME};Password={PASSWORD};SSLMode={SSL_MODE};CommandTimeout={COMMAND_TIMEOUT};ApplicationName={APPLICATION_NAME}
```

## Testing the Configuration

### Development
Ensure `secret.database.json` exists with valid credentials, then run:
```bash
dotnet run --project API
```

### Production
Set all required environment variables and run:
```bash
ASPNETCORE_ENVIRONMENT=Production dotnet run --project API
```

## Troubleshooting

- **"Failed to establish database connection"**: Check that all credentials are correct
- **Missing configuration file**: Ensure `secret.database.json` exists in development
- **Empty connection string**: Verify environment variables are set correctly in production
