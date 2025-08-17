# --- 1. Сборка Angular ---
FROM node:18-alpine AS build
WORKDIR /app

# Копируем package.json и package-lock.json, чтобы ускорить кэширование
COPY package*.json ./
RUN npm install

# Копируем весь проект
COPY . .

# Устанавливаем Angular CLI
RUN npm install -g @angular/cli

# Сборка прод-билда (используется environment.prod.ts)
RUN ng build --configuration=production

# --- 2. Отдача через Nginx ---
FROM nginx:alpine
# Копируем собранный билд в Nginx
COPY --from=build /app/dist/имя_проекта /usr/share/nginx/html

# Опционально: свой конфиг Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
