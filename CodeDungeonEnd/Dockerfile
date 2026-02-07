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

# Render'ın dinamik portunu kullanması için URL'i boş bırakıyoruz 
# veya Program.cs'deki ayara güveniyoruz.
EXPOSE 8080

ENTRYPOINT ["dotnet", "CodeDungeon.dll"]
