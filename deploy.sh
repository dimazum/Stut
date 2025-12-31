#!/bin/bash
set -e

echo "=============================="
echo "Starting deployment"
echo "=============================="

# -----------------------------
# 1. Git pull
# -----------------------------
echo "Pulling latest changes..."
git pull origin develop --tags

COMMIT_HASH=$(git rev-parse --short HEAD)
export COMMIT_HASH
echo "Commit hash: $COMMIT_HASH"

# -----------------------------
# 2. Build Angular в контейнере
# -----------------------------
echo "Building Angular in Docker container..."
docker compose run --rm frontend-builder

# -----------------------------
# 3. Сборка и пересоздание всех сервисов
# -----------------------------
echo "Building and starting all services..."
docker compose up -d --build traefik backend voice-analyzer rabbitmq stut.database

unset COMMIT_HASH

echo "=============================="
echo "✅ Deployment finished successfully"
echo "=============================="
