FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/TruthLens.Shared/TruthLens.Shared.csproj src/TruthLens.Shared/
COPY src/TruthLens.Api/TruthLens.Api.csproj src/TruthLens.Api/
RUN dotnet restore src/TruthLens.Api/TruthLens.Api.csproj

COPY src/TruthLens.Shared/ src/TruthLens.Shared/
COPY src/TruthLens.Api/ src/TruthLens.Api/
RUN dotnet publish src/TruthLens.Api/TruthLens.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TruthLens.Api.dll"]
