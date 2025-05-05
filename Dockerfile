# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln .
COPY Nqey.Domain/ Nqey.Domain/
COPY Nqey.DAL/ Nqey.DAL/
COPY Nqey.Services/ Nqey.Services/
COPY Nqey.Api/ Nqey.Api/

# Restore dependencies
WORKDIR /app/Nqey.Api
RUN dotnet restore

# Copy source code
WORKDIR /app
COPY Nqey.Domain/ Nqey.Domain/
COPY Nqey.DAL/ Nqey.DAL/
COPY Nqey.Services/ Nqey.Services/
COPY Nqey.Api/ Nqey.Api/

# Ensure no leftover build artifacts
WORKDIR /app/Nqey.Api
RUN rm -rf bin obj

# Publish to a clean folder
RUN dotnet publish Nqey.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
# Make sure the app listens on port 80 inside the container
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Nqey.Api.dll"]

