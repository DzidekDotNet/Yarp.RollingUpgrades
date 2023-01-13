FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RollingUpgrade.Proxy/RollingUpgrade.Proxy.csproj", "RollingUpgrade.Proxy/"]
RUN dotnet restore "RollingUpgrade.Proxy/RollingUpgrade.Proxy.csproj"
COPY . .
WORKDIR "/src/RollingUpgrade.Proxy"
RUN dotnet build "RollingUpgrade.Proxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RollingUpgrade.Proxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RollingUpgrade.Proxy.dll"]
