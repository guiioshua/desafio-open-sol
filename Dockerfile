# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.slnx ./
COPY DesafioGerenciadorTarefas.Api/*.csproj DesafioGerenciadorTarefas.Api/
COPY DesafioGerenciadorTarefas.Core/*.csproj DesafioGerenciadorTarefas.Core/
COPY DesafioGerenciadorTarefas.Infrastructure/*.csproj DesafioGerenciadorTarefas.Infrastructure/
COPY DesafioGerenciadorTarefas.Tests/*.csproj DesafioGerenciadorTarefas.Tests/
RUN dotnet restore

COPY . .
RUN dotnet publish DesafioGerenciadorTarefas.Api -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "DesafioGerenciadorTarefas.Api.dll"]
