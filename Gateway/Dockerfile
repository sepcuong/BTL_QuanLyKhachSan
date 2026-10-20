FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Gateway/Gateway.csproj", "Gateway/"]
RUN dotnet restore "Gateway/Gateway.csproj"

COPY . .
RUN dotnet publish "Gateway/Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]