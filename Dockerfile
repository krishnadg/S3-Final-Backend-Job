
FROM microsoft/dotnet:1.1.2-sdk
LABEL Name=s3job Version=0.0.1 
COPY . /usr/share/dotnet/sdk/s3job
WORKDIR /usr/share/dotnet/sdk/s3job
RUN dotnet restore
RUN dotnet build
ENTRYPOINT dotnet run --project S3ClassLib/S3ClassLib.csproj
