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

stage ('Dind') {
  podTemplate(
    label: 'default',
  ) {
    node('default') {
      container('default') {
        git url: 'https://github.com/krishnadg/S3-Final-Backend-Job.git', branch: 'master'
        sh '$(aws ecr get-login --no-include-email --region us-west-2)'
        sh 'docker build -f Dockerfile -t s3-backend-job:latest .'
        sh 'docker tag s3-backend-job:latest 543369334115.dkr.ecr.us-west-2.amazonaws.com/s3-backend-job:latest'
        sh 'docker push 543369334115.dkr.ecr.us-west-2.amazonaws.com/s3-backend-job:latest'
      }
    }
  }
}


/*stage ('Build-Docker-Image') {
	podTemplate(
		label: 'docker-build-pod',
		containers: [
			containerTemplate(
				name: 'docker-image',
				image: 'docker:latest',
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
*/