#!/bin/bash
set -e

echo "=============================="
echo "🚀 Starting deployment"
echo "=============================="

echo "▶ Pulling latest changes..."
git pull origin develop --tags

COMMIT_HASH=$(git rev-parse --short HEAD)
export COMMIT_HASH
echo "▶ Commit hash: $COMMIT_HASH"

# =============================
# 1. Build Angular в контейнере
# =============================
echo "▶ Building Angular in Docker container..."
docker build -f frontend-builder.Dockerfile -t frontend-builder .

# Создаем временный контейнер и копируем dist
docker create --name tmp-frontend frontend-builder
rm -rf backend/stutvds/wwwroot/*
docker cp tmp-frontend:/app/dist/angular1/browser/. backend/stutvds/wwwroot/
docker rm tmp-frontend

# =============================
# 2. Сборка и поднятие Docker-compose
# =============================
docker-compose up -d --build

unset COMMIT_HASH

echo "=============================="
echo "✅ Deployment finished successfully"
echo "=============================="
