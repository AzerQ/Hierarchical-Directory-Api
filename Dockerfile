# See https://aka.ms/devcontainer/dockerfile for more information.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base

# Install SQLite
RUN apt-get update && apt-get install -y sqlite3 libsqlite3-dev && rm -rf /var/lib/apt/lists/*

# Install Serilog log directory
RUN mkdir -p /workspaces/Hierarchical-Directory-Api/logs

# Set working directory
WORKDIR /workspaces/Hierarchical-Directory-Api

# Default command
CMD ["/bin/bash"]
