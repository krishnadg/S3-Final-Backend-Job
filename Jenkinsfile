// 	properties([
// 	parameters([
// 		string(description: 'IAM Role', name: 'Role')
// 	])
// ])
	
	
podTemplate(
	label: 'dotnet-core-pod',
	inheritFrom: 'test',
	containers: [
		containerTemplate(			
			name: 'dotnet-core',
			image: 'microsoft/dotnet:1.1.2-sdk',
			ttyEnabled: true,
			command: 'cat'
		)
	],
	annotations: [
		podAnnotation(key: "kube2iam.beta.nordstrom.net/role", value: "arn:aws:iam::543369334115:role/datalens/k8s/platform")]		
)
{
	node('dotnet-core-pod') {

		stage ('Test') 
		{
			container('dotnet-core') {

				checkout scm
				sh 'dotnet restore && dotnet test S3Tests/S3Tests.csproj --filter Category!=Integration'
			}		
		}

		stage ('PushImage') {
			container('test') {
				checkout scm
					sh '''
						$(aws ecr get-login --no-include-email --region us-west-2)
						docker build -f Dockerfile -t s3-backend-job:latest .
						docker tag s3-backend-job:latest 543369334115.dkr.ecr.us-west-2.amazonaws.com/s3-backend-job:latest
						docker push 543369334115.dkr.ecr.us-west-2.amazonaws.com/s3-backend-job:latest
					'''
			}
		}
	}
}
