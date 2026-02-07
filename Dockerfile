# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje ismini CodeDungeon olarak güncelledik
COPY ["CodeDungeon.csproj", "./"] 
RUN dotnet restore "./CodeDungeon.csproj"

COPY . .
RUN dotnet publish "CodeDungeon.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render dinamik port kullanır, ancak ASPNETCORE_URLS tanımlamak stabilite sağlar
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

# DLL adı artık CodeDungeon.dll olmalı
ENTRYPOINT ["dotnet", "CodeDungeon.dll"]
