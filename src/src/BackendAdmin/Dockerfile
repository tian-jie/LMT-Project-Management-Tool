#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/BackendAdmin/BackendAdmin.csproj", "src/BackendAdmin/"]
COPY ["src/BusinessCore/BusinessCore.csproj", "src/BusinessCore/"]
COPY ["src/ApplicationCore/ApplicationCore.csproj", "src/ApplicationCore/"]
COPY ["src/BlazorShared/BlazorShared.csproj", "src/BlazorShared/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/BlazorAdmin/BlazorAdmin.csproj", "src/BlazorAdmin/"]
RUN dotnet restore "src/BackendAdmin/BackendAdmin.csproj"
COPY . .
WORKDIR "/src/src/BackendAdmin"
RUN dotnet build "BackendAdmin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendAdmin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendAdmin.dll"]