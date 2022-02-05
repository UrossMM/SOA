#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CommandMicroservice/CommandMicroservice.API/CommandMicroservice.API.csproj", "CommandMicroservice/CommandMicroservice.API/"]
RUN dotnet restore "CommandMicroservice/CommandMicroservice.API/CommandMicroservice.API.csproj"
COPY . .
WORKDIR "/src/CommandMicroservice/CommandMicroservice.API"
RUN dotnet build "CommandMicroservice.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommandMicroservice.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommandMicroservice.API.dll"]
