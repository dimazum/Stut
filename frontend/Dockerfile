# --- Build stage ---
FROM node:18-alpine as build

WORKDIR /app
COPY package*.json ./
RUN npm install -g @angular/cli
RUN npm install

COPY . .

# Аргументы для продакшн переменных
ARG API_URL
ENV API_URL=${API_URL}

# Сборка Angular для продакшн
RUN ng build --configuration=production

# --- Serve stage ---
FROM nginx:alpine
COPY --from=build /app/dist/your-app-name /usr/share/nginx/html

# Копируем свой конфиг nginx, если нужен
# COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
