#!/bin/bash

# Remove any existing container with the same name
docker rm -f sql_server_container 2>/dev/null

# Run SQL Server container with the same settings as Docker Compose
docker run -d \
  --name sql_server_container \
  -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=YourPassword123" \
  -p 8000:1433 \
  mcr.microsoft.com/mssql/server:2017-latest
