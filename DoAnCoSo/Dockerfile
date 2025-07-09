# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY DoAnCoSo/*.csproj ./DoAnCoSo/
RUN dotnet restore DoAnCoSo/DoAnCoSo.csproj

COPY . .
RUN dotnet publish DoAnCoSo/DoAnCoSo.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "DoAnCoSo.dll"]
