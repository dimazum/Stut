# --- Базовый runtime (ASP.NET Core 10) ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Принудительно указываем порт, который будет слушать Kestrel
ENV ASPNETCORE_URLS=http://+:80

# --- SDK для сборки ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["stutvds/stutvds.csproj", "stutvds/"]
RUN dotnet restore "stutvds/stutvds.csproj"

COPY . .
WORKDIR "/src/stutvds"
RUN dotnet build "stutvds.csproj" -c Release -o /app/build

# --- Публикация ---
FROM build AS publish
RUN dotnet publish "stutvds.csproj" -c Release -o /app/publish

# --- Финальный образ ---
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "stutvds.dll"]
