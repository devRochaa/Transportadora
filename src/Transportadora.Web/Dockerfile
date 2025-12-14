FROM node:24-bookworm AS build

WORKDIR /app
COPY package*.json .
ARG NODE_ENV=development
ARG NODE_BUILD_SCRIPT=build
ARG VITE_API_URL
RUN npm install
COPY . .
RUN npm run $NODE_BUILD_SCRIPT

FROM nginx:alpine

COPY --from=build /app/dist /usr/share/nginx/html
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
