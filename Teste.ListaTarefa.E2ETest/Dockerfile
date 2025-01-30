FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Teste.ListaTarefa.E2ETest/Teste.ListaTarefa.E2ETest.csproj", "Teste.ListaTarefa.E2ETest/"]
RUN dotnet restore "Teste.ListaTarefa.E2ETest/Teste.ListaTarefa.E2ETest.csproj"
COPY . .
WORKDIR "/src/Teste.ListaTarefa.E2ETest"
RUN dotnet build "Teste.ListaTarefa.E2ETest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Teste.ListaTarefa.E2ETest.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teste.ListaTarefa.E2ETest.dll"]
