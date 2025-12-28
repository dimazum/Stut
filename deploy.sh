#!/bin/bash
set -e

echo "=============================="
echo "🚀 Starting deployment"
echo "=============================="

# -----------------------------
# 1. Git pull
# -----------------------------
echo "▶ Pulling latest changes..."
git pull origin develop --tags

COMMIT_HASH=$(git rev-parse --short HEAD)
export COMMIT_HASH
echo "▶ Commit hash: $COMMIT_HASH"

# -----------------------------
# 2. Build Angular в контейнере
# -----------------------------
echo "▶ Building Angular in Docker container..."
docker build -f frontend/Dockerfile -t frontend-builder ./frontend

# Создаем временный контейнер и копируем dist в backend/wwwroot
WWWROOT="backend/stutvds/wwwroot"
rm -rf "$WWWROOT"/*
mkdir -p "$WWWROOT"

docker create --name tmp-frontend frontend-builder
docker cp tmp-frontend:/app/dist/angular1/browser/. "$WWWROOT"/
docker rm tmp-frontend

echo "▶ Angular build copied to backend/wwwroot"

# -----------------------------
# 3. Сборка и поднятие Docker контейнеров
# -----------------------------
echo "▶ Building and starting Docker containers..."
docker-compose up -d --build

unset COMMIT_HASH

echo "=============================="
echo "✅ Deployment finished successfully"
echo "=============================="
