#!/bin/bash
set -e

echo "=============================="
echo "Starting deployment preprod"
echo "=============================="

echo "Updating repository..."

GIT_USER="dimazum"
GIT_TOKEN="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"
git fetch https://${GIT_USER}:${GIT_TOKEN}@github.com/dimazum/Stut.git develop --tags
git reset --hard FETCH_HEAD


export COMMIT_HASH=$(git rev-parse --short HEAD)
export APP_ENV=Production

echo "Cleaning docker build cache..."

docker compose -p preprod -f docker-compose.yml -f docker-compose.preprod.yml down

docker image prune -f
docker builder prune -f

echo "Building Angular..."

docker compose \
-p preprod \
-f docker-compose.yml \
-f docker-compose.preprod.yml \
build frontend-builder

docker compose \
-p preprod \
-f docker-compose.yml \
-f docker-compose.preprod.yml \
run --rm frontend-builder

echo "Starting containers..."

docker compose \
-p preprod \
-f docker-compose.yml \
-f docker-compose.preprod.yml \
up -d --build

echo "=============================="
echo "✅ Deployment preprod finished successfully"
echo "=============================="