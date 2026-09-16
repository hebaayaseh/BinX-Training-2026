# Place this file at: Week9/Day2/CardioTrack/Dockerfile
# (same folder as CardioTrack.sln)

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the csproj first so Docker can cache the restore layer
COPY CardioTrack/CardioTrack.csproj CardioTrack/
RUN dotnet restore CardioTrack/CardioTrack.csproj

# Now copy the rest of the source and publish
COPY CardioTrack/ CardioTrack/
WORKDIR /src/CardioTrack
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Railway (and many other PaaS platforms) inject a $PORT env var at runtime
# and route traffic to it. Azure App Service for Containers instead expects
# a fixed port (set via the WEBSITES_PORT app setting). This entrypoint
# works with both: it listens on $PORT if set, otherwise falls back to 8080.
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet CardioTrack.dll"]
