#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Deliveggie.Processor/Deliveggie.Processor.csproj", "Deliveggie.Processor/"]
COPY ["Deliveggie.Shared.Models/Deliveggie.Shared.Models.csproj", "Deliveggie.Shared.Models/"]
RUN dotnet restore "Deliveggie.Processor/Deliveggie.Processor.csproj"
COPY . .
WORKDIR "/src/Deliveggie.Processor"
RUN dotnet build "Deliveggie.Processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Deliveggie.Processor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Deliveggie.Processor.dll"]