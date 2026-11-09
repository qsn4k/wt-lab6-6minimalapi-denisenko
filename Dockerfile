# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY wt-lab6-6minimalapi-denisenko.csproj ./
RUN dotnet restore ./wt-lab6-6minimalapi-denisenko.csproj
COPY . .
RUN dotnet publish ./wt-lab6-6minimalapi-denisenko.csproj -c Release -o /app/publish /p:UseAppHost=false

# Small runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000
ENTRYPOINT ["dotnet", "wt-lab6-6minimalapi-denisenko.dll"]
