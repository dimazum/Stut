# frontend/Dockerfile
FROM node:18-alpine AS builder

WORKDIR /app

# Копируем package.json и package-lock.json
COPY package*.json ./

# Устанавливаем зависимости
RUN npm install

# Копируем весь фронт-код
COPY . .

# Сборка production и генерация manifest
RUN npx ng build --configuration production --output-hashing=all \
    && node scripts/generate-manifest.js
