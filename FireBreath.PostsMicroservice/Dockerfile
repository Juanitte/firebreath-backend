# == Build ==
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Common/ Common/
COPY RequestFiltering/ RequestFiltering/
COPY FireBreath.PostsMicroservice/ FireBreath.PostsMicroservice/

# En el Dockerfile de posts-ms, antes de dotnet restore/build, por ejemplo:
RUN apt-get update \
 && apt-get install -y ffmpeg \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /src/FireBreath.PostsMicroservice
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# == Runtime ==
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "FireBreath.PostsMicroservice.dll"]