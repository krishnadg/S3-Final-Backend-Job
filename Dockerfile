
FROM microsoft/dotnet:1.1.2-sdk
LABEL Name=s3job Version=0.0.1 
ARG source=.
COPY . /usr/share/dotnet/sdk/foo
WORKDIR /usr/share/dotnet/sdk/fooEXPOSE 8080
RUN dotnet restore
RUN dotnet build
ENTRYPOINT dotnet test S3Tests/S3Tests.csproj
