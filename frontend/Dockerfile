FROM node:18-alpine as build
WORKDIR /app
COPY . .
RUN npm install -g @angular/cli
RUN npm install
CMD ["sh", "-c", "ng serve --host 0.0.0.0 --configuration production"]