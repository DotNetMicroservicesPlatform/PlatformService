FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5231

ENV ASPNETCORE_URLS=http://+:5231

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["src/PlatformContracts/PlatformContracts.csproj", "src/PlatformContracts/"]
COPY ["src/PlatformService/PlatformService.csproj", "src/PlatformService/"]
RUN dotnet nuget add source --name baget "http://host.docker.internal:5555/v3/index.json"
RUN dotnet restore "src/PlatformService/PlatformService.csproj"
COPY ./src ./src
WORKDIR "/src/PlatformService"
RUN dotnet publish "PlatformService.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PlatformService.dll"]