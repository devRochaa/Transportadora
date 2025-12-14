# Stage 1: Build
FROM node:24-bookworm AS build

WORKDIR /app

# Copia apenas arquivos de dependência
COPY package*.json ./

# Variáveis de build
ARG NODE_ENV=development
ARG NODE_BUILD_SCRIPT=build
ARG VITE_API_URL

# Instala dependências
RUN npm install

# Copia o restante do código
COPY . .

# Build
RUN npm run $NODE_BUILD_SCRIPT

# Detecta se o build gerou 'dist' (Vite) ou 'build' (CRA)
RUN if [ -d "dist" ]; then \
        echo "Usando Vite build"; \
    elif [ -d "build" ]; then \
        echo "Usando CRA build"; \
        mv build dist; \
    else \
        echo "Erro: pasta de build não encontrada" && exit 1; \
    fi

# Stage 2: Nginx
FROM nginx:alpine

# Copia build para nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Opcional: se tiver nginx.conf dentro do frontend
# COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]