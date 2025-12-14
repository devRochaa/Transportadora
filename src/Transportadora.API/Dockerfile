# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /Code

# Copia solution e projetos
COPY *.slnx ./
COPY src/ ./src/

# Restaura dependências da solution inteira
RUN dotnet restore *.slnx

# Publica a API
RUN dotnet publish src/Transportadora.API/Transportadora.API.csproj -c Release -o build

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runner-env
WORKDIR /App
EXPOSE 8080

COPY --from=build-env /Code/build .
ENTRYPOINT ["dotnet", "Transportadora.API.dll"]
