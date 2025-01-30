FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Teste.ListaTarefa.WebApi/Teste.ListaTarefa.WebApi.csproj", "WebApi/"]
COPY ["Teste.ListaTarefa.Domain/Teste.ListaTarefa.Domain.csproj", "Domain/"]
COPY ["Teste.ListaTarefa.Infrastructure/Teste.ListaTarefa.Infrastructure.csproj", "Infrastructure/"]
COPY ["Teste.ListaTarefa.Application/Teste.ListaTarefa.Application.csproj", "Application/"]
RUN dotnet restore "WebApi/Teste.ListaTarefa.WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "Teste.ListaTarefa.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Teste.ListaTarefa.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teste.ListaTarefa.WebApi.dll"]
