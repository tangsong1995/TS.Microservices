#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.config","NuGet.config"]
COPY ["ApiGateways/TS.Microservices.Gateway/TS.Microservices.Gateway.csproj", "ApiGateways/TS.Microservices.Gateway/"]
RUN dotnet restore "ApiGateways/TS.Microservices.Gateway/TS.Microservices.Gateway.csproj"
COPY . .
WORKDIR "/src/ApiGateways/TS.Microservices.Gateway"
RUN dotnet build "TS.Microservices.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TS.Microservices.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TS.Microservices.Gateway.dll"]