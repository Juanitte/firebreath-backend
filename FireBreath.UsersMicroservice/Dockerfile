# == Build ==
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proyectos comunes en la misma posición relativa
COPY Common/ Common/
COPY RequestFiltering/ RequestFiltering/

# Copiamos TODO el proyecto UsersMicroservice en su carpeta
COPY FireBreath.UsersMicroservice/ FireBreath.UsersMicroservice/

# Nos situamos dentro y publicamos (restore+publish)
WORKDIR /src/FireBreath.UsersMicroservice
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# == Runtime ==
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "FireBreath.UsersMicroservice.dll"]