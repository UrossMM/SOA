#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DataMicroservice/DataMicroservice.API/DataMicroservice.API.csproj", "DataMicroservice/DataMicroservice.API/"]
RUN dotnet restore "DataMicroservice/DataMicroservice.API/DataMicroservice.API.csproj"
COPY . .
WORKDIR "/src/DataMicroservice/DataMicroservice.API"
RUN dotnet build "DataMicroservice.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataMicroservice.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataMicroservice.API.dll"]
