# Stut
SpeechFlow

Архитектура:

SERVER
│
├── Traefik
│
├── PROD
│   ├── backend
│   ├── rabbitmq
│   └── sqlserver (volume prod_sql_prod_data)
│
└── PREPROD
    ├── backend
    ├── rabbitmq
    └── sqlserver (volume preprod_sql_preprod_data)

Сеть:
                traefik_proxy network
        ┌─────────────────────────────────┐
        │                                 │
        │   traefik                       │
        │      │                          │
        │      ├──────── backend_prod     │
        │      │                          │
        │      └──────── backend_preprod  │
        │                                 │
        └─────────────────────────────────┘

prod_default network         preprod_default network
backend + db + rabbit        backend + db + rabbit

#Команды в linux
- проверить что фронтенд скопировался в wwwroot
ls -l ./backend/stutvds/wwwroot/angular
- логи бэка
docker logs -f prod-backend-1

#Traefik
https://traefik.stut.mooo.com/
login:
admin
pass:
traefik
