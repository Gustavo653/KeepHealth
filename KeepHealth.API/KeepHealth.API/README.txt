dotnet restore --Rodar antes de qualquer comando scaffold
Microsoft.NETCore.App
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design


Para criar migration:
dotnet ef migrations add CreateDoctorPacient -p KeepHealth.Persistence -s KeepHealth.API -c KeepHealthContext --verbose