# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Nqey.Domain/Nqey.Domain.csproj Nqey.Domain/
COPY Nqey.DAL/Nqey.DAL.csproj Nqey.DAL/
COPY Nqey.Services/Nqey.Services.csproj Nqey.Services/
COPY Nqey.Api/Nqey.Api.csproj Nqey.Api/
COPY Nqey.sln .


RUN dotnet restore

COPY . .
WORKDIR /app/Nqey.Api
RUN dotnet publish Nqey.Api.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Nqey.Api.dll"]
