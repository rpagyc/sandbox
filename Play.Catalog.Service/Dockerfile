#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Play.Catalog.Service/Play.Catalog.Service.csproj", "Play.Catalog.Service/"]
RUN dotnet restore "Play.Catalog.Service/Play.Catalog.Service.csproj"
COPY . .
WORKDIR "/src/Play.Catalog.Service"
RUN dotnet build "Play.Catalog.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Play.Catalog.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Play.Catalog.Service.dll"]
