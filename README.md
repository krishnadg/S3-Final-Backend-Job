Runs backend job on S3 to determine teams' total storage values and corresponding cost 



To run local tests from tests directory...

dotnet test --filter Category!=Integration



To run project from root directory... dotnet run --project S3ClassLib/S3ClassLib.csproj [bucketname] [folder prefix (prod, nonprod etc.)]