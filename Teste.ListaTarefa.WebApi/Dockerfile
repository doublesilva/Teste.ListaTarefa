# Usando a imagem base do SDK .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar csproj e restaurar dependências
COPY *.sln .
COPY ["Teste.ListaTarefa.WebApi/Teste.ListaTarefa.WebApi.csproj", "Teste.ListaTarefa.WebApi/"]
COPY ["Teste.ListaTarefa.Domain/Teste.ListaTarefa.Domain.csproj", "Teste.ListaTarefa.Domain/"]
COPY ["Teste.ListaTarefa.Infrastructure/Teste.ListaTarefa.Infrastructure.csproj", "Teste.ListaTarefa.Infrastructure/"]
COPY ["Teste.ListaTarefa.Application/Teste.ListaTarefa.Application.csproj", "Teste.ListaTarefa.Application/"]
COPY ["Teste.ListaTarefa.UnitTest/Teste.ListaTarefa.UnitTest.csproj", "Teste.ListaTarefa.UnitTest/"]
COPY ["Teste.ListaTarefa.IntegrationTest/Teste.ListaTarefa.IntegrationTest.csproj", "Teste.ListaTarefa.IntegrationTest/"]
RUN dotnet restore 

# Copiar todo o código fonte e compilar
COPY . ./
RUN dotnet build --no-restore

# Rodar testes unitários
RUN dotnet test Teste.ListaTarefa.UnitTest/Teste.ListaTarefa.UnitTest.csproj --no-build --logger:trx


# Publicar artefato
RUN dotnet publish Teste.ListaTarefa.WebApi/Teste.ListaTarefa.WebApi.csproj -c Release -o out

# Construir imagem runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "Teste.ListaTarefa.WebApi.dll"]
EXPOSE 80
