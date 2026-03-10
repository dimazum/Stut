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
