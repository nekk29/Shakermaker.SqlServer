# Shakermaker.SqlServer
Shakermaker is .Net database continuous integration tool for SqlServer.

Use the package [Shakermaker.Folders](https://github.com/nekk29/Shakermaker.Folders) to generate the folders structure.

## Usage

### Parameters

| Parameter | Description  |
| ------- | --- |
| **--source-directory** | The scripts directory |
| **--release** | Release id or release name |
| **--environment** | The environment (dev, test, prod...) |
| **--connection-string** | The database connection string to execute the changes |

### Execution

```sh
$ Shakermaker.SqlServer --source-directory "{SourceDirectory}" --release "{Release}" --environment "{Environment}" --connection-string "{ConnectionStrings}"
```