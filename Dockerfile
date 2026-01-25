# Runtime image (kiçik olur)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080   # sadəcə metadata, Render üçün vacib deyil

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CodeDungeonAPI.csproj", "."]
RUN dotnet restore "CodeDungeonAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet publish "CodeDungeonAPI.csproj" -c Release -o /app/publish --no-restore

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render-in default portu üçün (10000) dinamik etmək
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

ENTRYPOINT ["dotnet", "CodeDungeonAPI.dll"]
