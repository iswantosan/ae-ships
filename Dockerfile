FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AE.Ships.Api/AE.Ships.Api.csproj", "src/AE.Ships.Api/"]
COPY ["src/AE.Ships.Application/AE.Ships.Application.csproj", "src/AE.Ships.Application/"]
COPY ["src/AE.Ships.Domain/AE.Ships.Domain.csproj", "src/AE.Ships.Domain/"]
COPY ["src/AE.Ships.Infrastructure/AE.Ships.Infrastructure.csproj", "src/AE.Ships.Infrastructure/"]
RUN dotnet restore "./src/AE.Ships.Api/AE.Ships.Api.csproj"
COPY . .
WORKDIR "/src/src/AE.Ships.Api"
RUN dotnet build "./AE.Ships.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AE.Ships.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AE.Ships.Api.dll"]

