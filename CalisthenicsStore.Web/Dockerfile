
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CalisthenicsStore.Web/CalisthenicsStore.Web.csproj", "CalisthenicsStore.Web/"]
COPY ["CalisthenicsStore.Data/CalisthenicsStore.Data.csproj", "CalisthenicsStore.Data/"]
COPY ["CalisthenicsStore.Common/CalisthenicsStore.Common.csproj", "CalisthenicsStore.Common/"]
COPY ["CalisthenicsStore.Services/CalisthenicsStore.Services.csproj", "CalisthenicsStore.Services/"]
COPY ["CalisthenicsStore.ViewModels/CalisthenicsStore.ViewModels.csproj", "CalisthenicsStore.ViewModels/"]
RUN dotnet restore "./CalisthenicsStore.Web/CalisthenicsStore.Web.csproj"
COPY . .
WORKDIR "/src/CalisthenicsStore.Web"
RUN dotnet build "./CalisthenicsStore.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CalisthenicsStore.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalisthenicsStore.Web.dll"]