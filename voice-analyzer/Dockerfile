FROM python:3.11-slim

# Системные зависимости для аудио и Praat
RUN apt-get update && apt-get install -y \
    ffmpeg \
    libsndfile1 \
    gcc \
    g++ \
    && rm -rf /var/lib/apt/lists/*

# Рабочая директория
WORKDIR /app

# Python зависимости
COPY app/requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Код приложения
COPY app /app

# Порт FastAPI
EXPOSE 8000

# Запуск
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
