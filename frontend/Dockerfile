# frontend/Dockerfile
FROM node:18-bullseye

WORKDIR /app

# Только зависимости (кэшируются)
COPY package*.json ./
RUN npm ci

# Исходники
COPY . .

