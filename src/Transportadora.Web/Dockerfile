# Stage 1: Build do React
FROM node:24-bookworm AS build

WORKDIR /app

# Copia apenas os arquivos de dependência primeiro
COPY src/Transportadora.Web/package*.json ./

# Instala dependências
RUN npm install

# Copia o restante do código do frontend
COPY src/Transportadora.Web/ ./

# Variáveis de build
ARG NODE_ENV=production
ARG NODE_BUILD_SCRIPT=build
ARG VITE_API_URL

# Build do frontend
RUN npm run $NODE_BUILD_SCRIPT

# Ajuste automático para Vite ou CRA
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

COPY --from=build /app/dist /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf
ENV PORT 8080

EXPOSE 8080

# CMD ["nginx", "-g", "daemon off;"]
CMD ["sh", "-c", "envsubst '\$PORT' < /etc/nginx/conf.d/default.conf.template > /etc/nginx/conf.d/default.conf && nginx -g 'daemon off;'"]

# FROM node:24-bookworm AS build

# WORKDIR /app
# COPY package*.json .
# ARG NODE_ENV=development
# ARG NODE_BUILD_SCRIPT=build
# ARG VITE_API_URL
# RUN npm install
# COPY . .
# RUN npm run $NODE_BUILD_SCRIPT

# FROM nginx:alpine

# COPY --from=build /app/dist /usr/share/nginx/html
# COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

# EXPOSE 80
# CMD ["nginx", "-g", "daemon off;"]