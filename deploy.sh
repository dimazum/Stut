#!/bin/bash
set -e

echo "Pulling latest changes..."
git pull origin develop --tags

COMMIT_HASH=$(git rev-parse --short HEAD)
echo "Commit hash: $COMMIT_HASH"

export COMMIT_HASH=$COMMIT_HASH   # установить env

echo "Cleaning up old Docker resources..."
docker system prune -af --volumes   # -a для всех неиспользуемых образов, --volumes для томов, -f для подтверждения

echo "Building Docker images..."
docker-compose build              # теперь COMMIT_HASH доступен как build ARG

echo "Starting containers..."
docker-compose up -d              # COMMIT_HASH доступен как env

unset COMMIT_HASH                 # удалить переменную

echo "Deployment finished successfully"
