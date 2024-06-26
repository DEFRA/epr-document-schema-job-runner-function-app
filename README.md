# EPR Document Schema Job Runner Function

## Overview

The Document Schema Job Runner Function is an Azure Function app that runs on a timer trigger. Its primary function is to execute a set of tasks, retrieving and updating records from various databases within the EPR system.

## How To Run

### Prerequisites
In order to run the service you will need the following dependencies

- .NET 8
- Azure CLI

### Run
Go to `EPR.DocumentSchemaJobRunner/EPR.DocumentSchemaJobRunner.Function` directory and execute:

```
func start
```

### Docker
Run in terminal at the solution source root:

```
docker build -t document-schema-job-runner -f EPR.DocumentSchemaJobRunner.Function/Dockerfile .
```

Fill out the environment variables and run the following command:

```
docker run -e AzureWebJobsStorage="X" -e AccountsDatabase:ConnectionString="X" -e SubmissionsDatabase:ConnectionString="X" -e SubmissionsDatabase:AccountKey="X" -e SubmissionsDatabase:Name="X" -e ScheduleExpression="X" document-schema-job-runner
```

## How To Test

### Unit tests

On root directory `EPR.DocumentSchemaJobRunner`, execute:

```
dotnet test
```

### Pact tests

N/A

### Integration tests

N/A

## How To Debug

## Environment Variables - deployed environments

The structure of the app-settings can be found in the repository. Example configurations for the different environments can be found in [epr-app-config-settings](https://dev.azure.com/defragovuk/RWD-CPR-EPR4P-ADO/_git/epr-app-config-settings).

| Variable Name                         | Description                                                |
|---------------------------------------|------------------------------------------------------------|
| AzureWebJobsStorage                   | The connection string for the Azure Web Jobs Storage       |
| AccountsDatabase__ConnectionString    | The connection string for the Accounts SQL database        |
| SubmissionsDatabase__ConnectionString | The connection string for the Submissions Cosmos database  |
| SubmissionsDatabase__AccountKey       | The account key for the Submissions Cosmos database        |
| SubmissionsDatabase__Name             | The name of the Submissions Cosmos database                |
| ScheduleExpression                    | The schedule expression on which the function will execute |

## Additional Information

See [ADR-059: CosmosDB Schema Migrations](https://eaflood.atlassian.net/wiki/spaces/MWR/pages/4521427100/ADR-059+CosmosDB+Schema+Migrations)


### Monitoring and Health Check

Enable Health Check in the Azure portal and set the URL path to `TimerTrigger`

## Directory Structure

### Source files

- `EPR.DocumentSchemaJobRunner.Application` - Application .NET source files
- `EPR.DocumentSchemaJobRunner.Data` - Data .NET source files
- `EPR.DocumentSchemaJobRunner.Function` - Function .NET source files
- `EPR.DocumentSchemaJobRunner.UnitTests` - .NET unit test files

## Contributing to this project

Please read the [contribution guidelines](CONTRIBUTING.md) before submitting a pull request.

## Licence

[Licence information](LICENCE.md).