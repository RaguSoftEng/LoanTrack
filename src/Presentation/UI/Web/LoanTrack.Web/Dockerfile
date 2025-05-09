# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Presentation/UI/Web/LoanTrack.Web/LoanTrack.Web.csproj", "src/Presentation/UI/Web/LoanTrack.Web/"]
COPY ["aspire/LoanTrack.ServiceDefaults/LoanTrack.ServiceDefaults.csproj", "aspire/LoanTrack.ServiceDefaults/"]
COPY ["src/Infrastructure/LoanTrack.Identity/LoanTrack.Identity.csproj", "src/Infrastructure/LoanTrack.Identity/"]
COPY ["src/Core/LoanTrack.Application/LoanTrack.Application.csproj", "src/Core/LoanTrack.Application/"]
COPY ["src/Core/LoanTrack.Domain/LoanTrack.Domain.csproj", "src/Core/LoanTrack.Domain/"]
COPY ["src/Infrastructure/LoanTrack.Persistence/LoanTrack.Persistence.csproj", "src/Infrastructure/LoanTrack.Persistence/"]
COPY ["src/Presentation/LoanTrack.Presentation/LoanTrack.Presentation.csproj", "src/Presentation/LoanTrack.Presentation/"]
COPY ["src/Presentation/UI/Web/LoanTrack.Web.Client/LoanTrack.Web.Client.csproj", "src/Presentation/UI/Web/LoanTrack.Web.Client/"]
COPY ["src/Presentation/UI/Web/LoanTrack.Web.Shared/LoanTrack.Web.Shared.csproj", "src/Presentation/UI/Web/LoanTrack.Web.Shared/"]
RUN dotnet restore "./src/Presentation/UI/Web/LoanTrack.Web/LoanTrack.Web.csproj"
COPY . .
WORKDIR "/src/src/Presentation/UI/Web/LoanTrack.Web"
RUN dotnet build "./LoanTrack.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LoanTrack.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoanTrack.Web.dll"]