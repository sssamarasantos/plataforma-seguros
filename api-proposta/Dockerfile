# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_HTTPS_PORTS=8081;8080
ENV AWS_REGION=us-east-1

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /
COPY ["src/SeguroProposta.Api/SeguroProposta.Api.csproj", "SeguroProposta.Api/"]
COPY ["src/SeguroProposta.Application/SeguroProposta.Application.csproj", "SeguroProposta.Application/"]
COPY ["src/SeguroProposta.Domain/SeguroProposta.Domain.csproj", "SeguroProposta.Domain/"]
COPY ["src/SeguroProposta.Infrastructure/SeguroProposta.Infrastructure.Data.csproj", "SeguroProposta.Infrastructure.Data/"]
RUN dotnet restore "./SeguroProposta.Api/SeguroProposta.Api.csproj"
COPY . .
WORKDIR "/src/SeguroProposta.Api"
RUN dotnet build "./SeguroProposta.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SeguroProposta.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeguroProposta.Api.dll"]