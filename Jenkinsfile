properties([
	parameters([
		
		string(description: 'environment', name: 'ENVIRONMENT')
	])
])


stage ('Test') {
	podTemplate(
		label: 'dotnet-core-pod',
		containers: [
			containerTemplate(
				name: 'dotnet-core',
				image: 'microsoft/dotnet:1.1.2-sdk',
				ttyEnabled: true,
				command: 'cat',
				
			)
		]
	) {
		node('dotnet-core-pod') {
			container('dotnet-core') {

				checkout scm
				sh 'dotnet restore && dotnet test S3Tests/S3Tests.csproj --filter Category!=Integration'
			}
		}
	}
}

stage ('Build-Docker-Image') {
	podTemplate(
		label: 'docker-build-pod',
		containers: [
			containerTemplate(
				name: 'docker-image',
				image: 'microsoft/dotnet:1.1.2-sdk',
				ttyEnabled: true,
				command: 'cat',
				
			)
		]
	) {
		node('docker-build-pod') {
			container('docker-image') {

				checkout scm
				sh 'docker build -f Dockerfile -t s3jobfinal:latest .'
			}
		}
	}
}