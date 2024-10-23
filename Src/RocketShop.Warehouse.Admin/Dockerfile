# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/RocketShop.Warehouse.Admin/RocketShop.Warehouse.Admin.csproj", "Src/RocketShop.Warehouse.Admin/"]
COPY ["Src/RocketShop.AuditService/RocketShop.AuditService.csproj", "Src/RocketShop.AuditService/"]
COPY ["Src/RocketShop.Database/RocketShop.Database.csproj", "Src/RocketShop.Database/"]
COPY ["Src/RocketShop.Shared/RocketShop.Shared.csproj", "Src/RocketShop.Shared/"]
COPY ["Src/RocketShop.Framework/RocketShop.Framework.csproj", "Src/RocketShop.Framework/"]
COPY ["Src/RocketShop.Retail/RocketShop.Retail.csproj", "Src/RocketShop.Retail/"]
COPY ["Src/RocketShop.SharedBlazor/RocketShop.SharedBlazor.csproj", "Src/RocketShop.SharedBlazor/"]
RUN dotnet restore "./Src/RocketShop.Warehouse.Admin/RocketShop.Warehouse.Admin.csproj"
COPY . .
WORKDIR "/src/Src/RocketShop.Warehouse.Admin"
RUN dotnet build "./RocketShop.Warehouse.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RocketShop.Warehouse.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RocketShop.Warehouse.Admin.dll"]