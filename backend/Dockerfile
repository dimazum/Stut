FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["stutvds/stutvds.csproj", "stutvds/"]
RUN dotnet restore "stutvds/stutvds.csproj"
COPY . .
WORKDIR "/src/stutvds"
RUN dotnet build "stutvds.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "stutvds.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS: http://+:80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "stutvds.dll"]
