# Hierarchical Directory API

RESTful API for managing hierarchical data structure (categories) with versioning, validation, and CRUD operations.

## Projects
- HierarchicalDirectory.Domain: Data models, interfaces
- HierarchicalDirectory.Application: Business logic, DTO, validation
- HierarchicalDirectory.Infrastructure: EF Core, repositories, Serilog
- HierarchicalDirectory.Api: Console app, EmbedIO, DI, HTTP endpoints

## Tech stack
- .NET 8, C#
- SQLite, Entity Framework Core
- EmbedIO (REST API)
- Serilog (logging)
- Microsoft.Extensions.DependencyInjection

## Quick start
1. Update connection string in `appsettings.json` if needed
2. Build and run HierarchicalDirectory.Api
3. API available at http://localhost:9696/api

## Endpoints
See full specification in the main README.md
