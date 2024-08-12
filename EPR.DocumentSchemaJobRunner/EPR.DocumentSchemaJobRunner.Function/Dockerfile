# Use the SDK image to build your application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS installer-env
USER root

# Copy your application source code
COPY EPR.DocumentSchemaJobRunner.Function/. ./EPR.DocumentSchemaJobRunner.Function/.
COPY EPR.DocumentSchemaJobRunner.Application/. ./EPR.DocumentSchemaJobRunner.Application/.
COPY EPR.DocumentSchemaJobRunner.Data/. ./EPR.DocumentSchemaJobRunner.Data/.
COPY stylecop.ruleset ./
COPY Directory.Build.props ./

# Build the application
RUN dotnet publish EPR.DocumentSchemaJobRunner.Function/*.csproj --output /home/site/wwwroot

# Use the Azure Functions image as the final image
FROM mcr.microsoft.com/azure-functions/dotnet:4

# Create a non-root user and set permissions
RUN groupadd -r dotnet && \
    useradd -r -g dotnet dotnet && \
    chown -R dotnet /home

# Switch to the non-root user for the final image
USER dotnet

# Set environment variables
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

# Copy the published application from the installer-env image
COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]