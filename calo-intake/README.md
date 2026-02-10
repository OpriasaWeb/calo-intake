# calo-intake

Small C# console app to track nutrition from a simple foods JSON database.

Usage
- Place `foods.json` next to the executable (the sample is included).
- Values in `foods.json` are **per 100 grams**.
- Run:

```powershell
dotnet run --project "calo-intake.csproj"
```

Commands in the app
- `list` — shows available foods
- `done` — finish and show totals

Files
- `Program.cs` — main app
- `Food.cs` — model
- `foods.json` — sample database (per 100g)
- `calo-intake.csproj` — project file
