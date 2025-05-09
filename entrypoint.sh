#!/bin/bash

# Run EF migrations for Identity (if needed)
echo "Running Identity migrations..."
dotnet ef database update --project /src/Infrastructure/LoanTrack.Identity/LoanTrack.Identity.csproj --context LoanTrack.Identity.Database.LoanTrackIdentityDbContext --startup-project /src/Presentation/UI/Web/LoanTrack.Web/LoanTrack.Web.csproj

# Run EF migrations for Application (if needed)
echo "Running Application migrations..."
dotnet ef database update --project /src/Infrastructure/LoanTrack.Persistence/LoanTrack.Persistence.csproj --context LoanTrack.Persistence.Common.Database.ApplicationDbContext --startup-project /src/Presentation/UI/Web/LoanTrack.Web/LoanTrack.Web.csproj

# Now start the application
echo "Starting application..."
exec dotnet /app/LoanTrack.Web.dll
