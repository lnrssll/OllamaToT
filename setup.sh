dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design

if [ -z "$1" ]; then
  echo "No argument supplied"
  echo "Using current directory name"
  PROJECT_NAME=$(basename "$(pwd)")
else
  PROJECT_NAME=$1
fi

# convert to lowercase
PROJECT_NAME=$(echo "$PROJECT_NAME" | tr '[:upper:]' '[:lower:]')

# DB_PASSWORD="password"
DB_PASSWORD=$(openssl rand -base64 32 | tr -d /=+) # | cut -c -16)

sudo psql -U postgres -c "CREATE USER $PROJECT_NAME WITH PASSWORD '$DB_PASSWORD';"
echo "POSTGRES_USER=$PROJECT_NAME" >> .env
echo "POSTGRES_PASSWORD=$DB_PASSWORD" >> .env

sudo psql -U postgres -c "CREATE DATABASE $PROJECT_NAME;"
echo "POSTGRES_DB=$PROJECT_NAME" >> .env

sudo psql -U postgres -c "GRANT ALL PRIVILEGES ON DATABASE $PROJECT_NAME TO $PROJECT_NAME;"
sudo psql -U postgres -c "GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO $PROJECT_NAME;"
sudo psql -U postgres -c "GRANT CREATE ON SCHEMA public TO $PROJECT_NAME;"

echo "Postgres setup complete"

if command -v xclip &> /dev/null; then
  echo "Host=localhost;Database=$PROJECT_NAME;Username=$PROJECT_NAME;Password=$DB_PASSWORD;" | xclip -sel clip
  echo "Connection string copied to clipboard"
  echo "You can now paste it into your appsettings.json file under 'ConnectionStrings' > 'YourProjectName'"
fi

echo "Credentials saved to .env file"
