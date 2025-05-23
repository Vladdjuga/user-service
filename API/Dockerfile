# Базовый рантайм
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем решение и проекты (на уровень выше)
COPY ../DDD_Messenger.sln ./
COPY ../API/API.csproj ./API/
COPY ../Architechture/Infrastructure.csproj ./Architechture/
COPY ../Grpc/Grpc.csproj ./Grpc/

# Восстанавливаем зависимости
RUN dotnet restore "./API/API.csproj"

# Копируем остальной код (всё из репы)
COPY .. .

# Строим проект
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем
FROM build AS publish
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "API.dll"]
