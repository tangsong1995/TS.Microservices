#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.config","NuGet.config"]
COPY ["App/TS.Microservices.WebApi/TS.Microservices.WebApi.csproj", "App/TS.Microservices.WebApi/"]
COPY ["Infrastructure/TS.Microservices.Infrastructure/TS.Microservices.Infrastructure.csproj", "Infrastructure/TS.Microservices.Infrastructure/"]
COPY ["Shared/TS.Microservices.Infrastructure.Core/TS.Microservices.Infrastructure.Core.csproj", "Shared/TS.Microservices.Infrastructure.Core/"]
COPY ["Shared/TS.Microservices.Domain.Abstractions/TS.Microservices.Domain.Abstractions.csproj", "Shared/TS.Microservices.Domain.Abstractions/"]
COPY ["Domain/TS.Microservices.Domain/TS.Microservices.Domain.csproj", "Domain/TS.Microservices.Domain/"]
RUN dotnet restore "App/TS.Microservices.WebApi/TS.Microservices.WebApi.csproj"
COPY . .
WORKDIR "/src/App/TS.Microservices.WebApi"
RUN dotnet build "TS.Microservices.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TS.Microservices.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime  #需要时设置时区
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TS.Microservices.WebApi.dll"]