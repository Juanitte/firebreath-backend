# == Build ==
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Common/ Common/
COPY RequestFiltering/ RequestFiltering/
COPY FireBreath.ApiGateway/ FireBreath.ApiGateway/

WORKDIR /src/FireBreath.ApiGateway
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# == Runtime ==
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "FireBreath.ApiGateway.dll"]