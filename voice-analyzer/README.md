#Запуск локально из voice-analyzer\app
uvicorn main:app --reload --host 0.0.0.0 --port 8000

#Запуск сваггера
http://localhost:8000/docs