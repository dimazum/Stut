#!/bin/bash
set -e  # cancel script if error

echo "Pulling latest changes..."
git pull origin develop --tags

# images
echo "Building Docker images..."
docker-compose build

# start
echo "Starting containers..."
docker-compose up -d

echo "Deployment finished successfully"