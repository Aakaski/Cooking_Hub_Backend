# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY dotnetapp.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . .
RUN dotnet publish dotnetapp.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000
ENTRYPOINT ["dotnet", "dotnetapp.dll"]
