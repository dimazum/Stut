FROM node:18-alpine as build
WORKDIR /app
COPY . .
RUN npm install -g @angular/cli
RUN ng build --configuration=production