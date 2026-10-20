FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BTL_QuanLyKhachSan/APIAdmin.csproj", "BTL_QuanLyKhachSan/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Model/Model.csproj", "Model/"]
RUN dotnet restore "BTL_QuanLyKhachSan/APIAdmin.csproj"

COPY . .
RUN dotnet publish "BTL_QuanLyKhachSan/APIAdmin.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "APIAdmin.dll"]