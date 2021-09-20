#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BlockSearch.MVC/BlockSearch.MVC.csproj", "BlockSearch.MVC/"]
COPY ["BlockSearch.Application/BlockSearch.Application.csproj", "BlockSearch.Application/"]
COPY ["BlockSearch.Common/BlockSearch.Common.csproj", "BlockSearch.Common/"]
COPY ["BlockSearch.Infrastructure/BlockSearch.Infrastructure.csproj", "BlockSearch.Infrastructure/"]
RUN dotnet restore "BlockSearch.MVC/BlockSearch.MVC.csproj"
COPY . .
WORKDIR "/src/BlockSearch.MVC"
RUN dotnet build "BlockSearch.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlockSearch.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlockSearch.MVC.dll"]