FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
#USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["cuf-logger-app-service/cuf-logger-app-service.csproj", "cuf-logger-app-service/"]
RUN dotnet restore "cuf-logger-app-service/cuf-logger-app-service.csproj"
COPY . .
WORKDIR "/src/cuf-logger-app-service"
RUN dotnet build "cuf-logger-app-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cuf-logger-app-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#USER root
RUN mkdir -p /app/wwwroot/uploads
RUN mkdir -p /app/wwwroot/uploads/documents
#SSL
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
#USER appuser
VOLUME [ "/app/wwwroot/uploads" ]

#USER appuser
ENTRYPOINT ["dotnet", "cuf-logger-app-service.dll"]