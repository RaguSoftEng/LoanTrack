#!/bin/bash

# Fix permissions for PostgreSQL volume on Linux
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
  echo "Setting up permissions for Linux..."
  sudo mkdir -p /var/lib/docker/volumes/postgres_data
  sudo chown -R 1001:1001 /var/lib/docker/volumes/postgres_data
  sudo mkdir -p /var/lib/docker/volumes/pgadmin_data
  sudo chown -R 1001:1001 /var/lib/docker/volumes/pgadmin_data
fi

echo "Starting Docker Compose..."
docker-compose up --build

