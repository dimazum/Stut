#Миграции:
перейти в backend\
-создание
    dotnet ef migrations add Test1 -p .\stutvds.DAL\stutvds.DAL.csproj -s .\stutvds\stutvds.csproj
-накат
    dotnet ef database update -p .\stutvds.DAL\stutvds.DAL.csproj -s .\stutvds\stutvds.csproj


#Локально обновить сертификаты:
dotnet dev-certs https --clean
dotnet dev-certs https --trust

#RabbitMQ
http://localhost:15672/
95.182.122.4:15672

#Обновить файл deploy.sh
Вызвать на VPS:
chmod +x deploy.sh

#Запустить деплой на VPS
dps
или
./deploy.sh
