# frontend/Dockerfile
FROM node:18-alpine AS builder

WORKDIR /app

# Копируем package.json и package-lock.json
COPY package*.json ./

# Устанавливаем зависимости
RUN npm install

# Копируем весь фронт
COPY . .

# Собираем production билд
RUN npx ng build --configuration production
