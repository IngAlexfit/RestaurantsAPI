#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Restaurants From Colombia/Restaurants From Colombia.csproj", "Restaurants From Colombia/"]
RUN dotnet restore "Restaurants From Colombia/Restaurants From Colombia.csproj"
COPY . .
WORKDIR "/src/Restaurants From Colombia"
RUN dotnet build "Restaurants From Colombia.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Restaurants From Colombia.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Restaurants From Colombia.dll"]