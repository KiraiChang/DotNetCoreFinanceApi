#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Applications/WebApi/WebApi.csproj", "Applications/WebApi/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Cores/Cores.csproj", "Cores/"]
COPY ["Services/Services.csproj", "Services/"]
RUN dotnet restore "Applications/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/Applications/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
