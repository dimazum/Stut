FROM node:18-alpine as build

WORKDIR /app

# Копируем зависимости и устанавливаем
COPY package*.json ./
RUN npm install -g @angular/cli
RUN npm install

# Копируем весь проект
COPY . .

# Аргументы для продакшн-переменных
ARG API_URL
ENV API_URL=${API_URL}

# Сборка Angular для продакшн
RUN ng build --configuration=production
