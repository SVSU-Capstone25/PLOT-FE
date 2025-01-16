# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy everything to the working directory
COPY . ./

# Restore dependencies
RUN dotnet restore

# Build and publish the app (for production)
RUN dotnet publish -c Release -o /publish

# Stage 2: Serve
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Set the working directory in the container
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build-env /publish .

# Expose the port on which the app will run
EXPOSE 8080

# Define the entry point for the container
ENTRYPOINT ["dotnet", "Plot.dll"]
