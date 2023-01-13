FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RollingUpgrade.Api/RollingUpgrade.Api.csproj", "RollingUpgrade.Api/"]
RUN dotnet restore "RollingUpgrade.Api/RollingUpgrade.Api.csproj"
COPY . .
WORKDIR "/src/RollingUpgrade.Api"
RUN dotnet build "RollingUpgrade.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RollingUpgrade.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RollingUpgrade.Api.dll"]
