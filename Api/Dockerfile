# syntax=docker/dockerfile:1

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG UBUNTU_VERSION="20.04"
ARG DOTNET_VERSION="8.0"

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src #Set current Docker working directory
COPY ["Api/Api.csproj", "Api/"] #Copy from local device to docker folder
RUN dotnet restore "./Api/Api.csproj" #Restoring the packages for our solution in the Docker image
COPY . .
WORKDIR "/src/Api" #Set another current working directory
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]