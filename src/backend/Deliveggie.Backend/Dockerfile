#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 35575

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Deliveggie.Backend/Deliveggie.Backend.csproj", "Deliveggie.Backend/"]
COPY ["Deliveggie.Shared.Models/Deliveggie.Shared.Models.csproj", "Deliveggie.Shared.Models/"]
RUN dotnet restore "Deliveggie.Backend/Deliveggie.Backend.csproj"
COPY . .
WORKDIR "/src/Deliveggie.Backend"
RUN dotnet build "Deliveggie.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Deliveggie.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Deliveggie.Backend.dll"]