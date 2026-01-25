# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CodeDungeonAPI.csproj", "."]
RUN dotnet restore "CodeDungeonAPI.csproj"
COPY . .
RUN dotnet publish "CodeDungeonAPI.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Portu göstər (format vacib: yalnız rəqəm, şərh yoxdur!)
EXPOSE 10000

# Render-in default portuna bind et
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

ENTRYPOINT ["dotnet", "CodeDungeonAPI.dll"]
