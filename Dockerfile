FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Galaxi.Tickets.API/Galaxi.Tickets.API.csproj", "Galaxi.Tickets.API/"]
COPY ["Galaxi.Tickets.Domain/Galaxi.Tickets.Domain.csproj", "Galaxi.Tickets.Domain/"]
COPY ["Galaxi.Bus.Message/Galaxi.Bus.Message.csproj", "Galaxi.Bus.Message/"]
COPY ["Galaxi.Tickets.Persistence/Galaxi.Tickets.Persistence.csproj", "Galaxi.Tickets.Persistence/"]
COPY ["Galaxi.Tickets.Data/Galaxi.Tickets.Data.csproj", "Galaxi.Tickets.Data/"]
RUN dotnet restore "./Galaxi.Tickets.API/./Galaxi.Tickets.API.csproj"
COPY . .
WORKDIR "/src/Galaxi.Tickets.API"
RUN dotnet build "./Galaxi.Tickets.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Galaxi.Tickets.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Galaxi.Tickets.API.dll"]