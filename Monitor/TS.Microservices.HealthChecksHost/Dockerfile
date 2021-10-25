#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.config","NuGet.config"]
COPY ["Monitor/TS.Microservices.HealthChecksHost/TS.Microservices.HealthChecksHost.csproj", "Monitor/TS.Microservices.HealthChecksHost/"]
RUN dotnet restore "Monitor/TS.Microservices.HealthChecksHost/TS.Microservices.HealthChecksHost.csproj"
COPY . .
WORKDIR "/src/Monitor/TS.Microservices.HealthChecksHost"
RUN dotnet build "TS.Microservices.HealthChecksHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TS.Microservices.HealthChecksHost.csproj" -c Release -o /app/publish

FROM base AS final
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime  #需要时设置时区
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TS.Microservices.HealthChecksHost.dll"]