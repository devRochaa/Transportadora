FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /Code

# Copia todos csproj para o environment de build
COPY Transportadora.API/Transportadora.API.csproj                               Transportadora.API/

# Restaura os pacotes
RUN  dotnet restore Transportadora.API/Transportadora.API.csproj

# Copia os arquivos necessários para o projeto
COPY Transportadora.API                                                         Transportadora.API

# Executa build do projeto api
# TODO: verificar o porque --no-restore não está funcionando sendo que os pacotes já foram restaurados
RUN  dotnet publish -c Release -o build Transportadora.API/Transportadora.API.csproj
# -------------------------------------------------------------------------------------------------------------------- #

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runner-env

# Default dev se não específicado
ENV  ASPNETCORE_ENVIRONMENT=Development

WORKDIR /App

# Copia os arquivos compilados para o environment quer rodará o projeto
COPY --from=build-env --chown=app:app /Code/build .

# Default user da imagem mcr.microsoft.com/dotnet/aspnet:9.0
USER app

# Commando de inicialização do projeto
CMD  ["dotnet", "/App/Transportadora.API.dll"]
