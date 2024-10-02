FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/Web.Api/Web.Api.csproj", "src/Web.Api/"]
RUN dotnet restore "src/Web.Api/Web.Api.csproj"
COPY . .
RUN rm -f /src/src/Web.Api/appsettings.Development.json
WORKDIR "/src/src/Web.Api"
RUN dotnet build "Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Web.Api.dll", "--urls", "http://+:80"]
