dotnet restore --Rodar antes de qualquer comando scaffold
Microsoft.NETCore.App
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design


Npgsql.EntityFrameworkCore.PostgreSQL
dotnet ef dbcontext scaffold "string banco" Npgsql.EntityFrameworkCore.PostgreSQL -o Models

Oracle.ManagedDataAccess.Core
dotnet ef dbcontext scaffold "string banco" Oracle.EntityFrameworkCore -o ModelsEME4 

Microsoft.EntityFrameworkCore.SqlServer
dotnet ef dbcontext scaffold "string banco" Microsoft.EntityFrameworkCore.SqlServer -o Models




Para criar migration:
dotnet ef migrations add Initial -p KeepNotes.Persistence -s KeepNotes -c KeepNotesContext --verbose