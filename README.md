# Tool for running sql scripts on MSSQL server

# How to use:
## Update appsettings.json, fill:
- ConnectionString: db connection string ex: `Server=localhost,1433;User ID=sa;Password=myStrongPassword1;Connection Timeout=120;MultipleActiveResultSets=true`
- Files: array of sql files to run, ex: `./my-awesome-script.sql`