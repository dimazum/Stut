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

echo "▶ Building Angular (production)..."
cd frontend
ng build --configuration production
cd ..

ANGULAR_DIST="frontend/dist/angular1/browser"
WWWROOT="backend/stutvds/wwwroot"

echo "▶ Copying Angular build to backend wwwroot..."
if [ ! -d "$ANGULAR_DIST" ]; then
  echo "❌ Angular build not found: $ANGULAR_DIST"
  exit 1
fi

rm -rf "$WWWROOT"/*
mkdir -p "$WWWROOT"
cp -r "$ANGULAR_DIST"/* "$WWWROOT"/

echo "▶ Building and starting Docker containers..."
docker-compose up -d --build

unset COMMIT_HASH

echo "=============================="
echo "✅ Deployment finished successfully"
echo "=============================="
