#!/bin/bash
set -e

echo "=============================="
echo "Starting deployment prod"
echo "=============================="

echo "Updating repository..."

GIT_USER="dimazum"
GIT_TOKEN="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"

echo "Pulling latest changes..."
git pull https://${GIT_USER}:${GIT_TOKEN}@github.com/dimazum/Stut.git main --tags


export COMMIT_HASH=$(git rev-parse --short HEAD)
export APP_ENV=Production


echo "Cleaning docker build cache..."

docker image prune -f
docker builder prune -f

echo "Building Angular..."

docker compose \
-p prod \
-f docker-compose.yml \
-f docker-compose.prod.yml \
build frontend-builder

docker compose \
-p prod \
-f docker-compose.yml \
-f docker-compose.prod.yml \
run --rm frontend-builder

echo "Starting containers..."

docker compose \
-p prod \
-f docker-compose.yml \
-f docker-compose.prod.yml \
up -d --build

echo "=============================="
echo "✅ Deployment prod finished successfully"
echo "=============================="