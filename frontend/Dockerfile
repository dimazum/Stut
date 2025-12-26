# Используем официальный Node
FROM node:18-alpine AS build

# Рабочая директория
WORKDIR /app

# Копируем только package.json и package-lock.json сначала
COPY package*.json ./

# Устанавливаем Angular CLI глобально
RUN npm install -g @angular/cli

# Устанавливаем зависимости с игнорированием peer-deps
RUN npm install --legacy-peer-deps

# Копируем остальной проект
COPY . .

# Собираем проект (production)
RUN ng build --configuration production

# Команда по умолчанию (для dev можно использовать ng serve)
CMD ["sh", "-c", "ng serve --host 0.0.0.0 --configuration production"]
