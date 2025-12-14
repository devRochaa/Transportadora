# Stage 1: Build do React
FROM node:24-bookworm AS build

WORKDIR /app

# Copia todo o código do frontend
COPY . .

# Variáveis de build
ARG NODE_ENV=production
ARG NODE_BUILD_SCRIPT=build
ARG VITE_API_URL

# Instala dependências
RUN npm install

# Copia apenas arquivos de dependência do frontend
COPY src/Transportadora.Web/package*.json ./

# Build
RUN npm run $NODE_BUILD_SCRIPT

# Ajuste automático para CRA ou Vite
RUN if [ -d "dist" ]; then \
    echo "Build Vite detectado"; \
    elif [ -d "build" ]; then \
    echo "Build CRA detectado"; \
    mv build dist; \
    else \
    echo "Erro: pasta de build não encontrada" && exit 1; \
    fi

# Stage 2: Nginx
FROM nginx:alpine

# Copia build para Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Expose porta 80
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
