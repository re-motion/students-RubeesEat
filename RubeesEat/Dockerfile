# to build: docker build -t <name> .
# to run: docker run -p <port(use port above 1024)>:8080 <name> .

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App
 
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out
 
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "RubeesEat.dll"]