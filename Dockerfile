# Базовый рантайм
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 6001

# SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем решение и проекты (на уровень выше)
COPY user-service.sln ./
COPY UI/UI.csproj ./UI/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Grpc/Grpc.csproj ./Grpc/

# Восстанавливаем зависимости
RUN dotnet restore "./UI/UI.csproj"

# Копируем остальной код (всё из репы)
COPY .. .

# Строим проект
WORKDIR "/src/UI"
RUN dotnet build "UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем
FROM build AS publish
RUN dotnet publish "UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "UI.dll"]
