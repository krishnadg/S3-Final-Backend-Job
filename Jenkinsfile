properties([
  parameters([string(description: 'IAM Role Account', name: 'ROLE_ACCT'),
	string(description: 'Repository Account', name: 'REPO_ACCT')])
])


podTemplate(
	name: 'dotnet-and-docker',
	label: 'dotnet-and-docker',
	containers: [
		containerTemplate(name: 'dotnet-core',image: 'microsoft/dotnet:1.1.2-sdk',ttyEnabled: true,command: 'cat'), 
		containerTemplate(name: 'awscli', image: 'teardrop/awscli', ttyEnabled: true, command: 'cat'),
		containerTemplate(name: 'docker', image: 'docker:stable-dind', ttyEnabled: true, command: 'cat', privileged: true),
	],
	annotations: [
		podAnnotation(key: "kube2iam.beta.nordstrom.net/role", value: "arn:aws:iam::543369334115:role/datalens/k8s/platform")
	], 
	volumes: [
		emptyDirVolume(mountPath: '/var/lib/docker', memory: false),
		hostPathVolume(hostPath: '/var/run/docker.sock', mountPath: '/var/run/docker.sock')
	]		
)
{
	node('dotnet-and-docker') {

		checkout scm

		stage ('Test S3 Job') 
		{
			container('dotnet-core') {
			  //sh 'dotnet restore && dotnet test S3Tests/S3Tests.csproj --filter Category!=Integration'
			}		
		}

		stage ('AWS Install')
		{
			container('awscli')
			{
				sh '''
				aws ecr get-login --no-include-email --region us-west-2 > ecr-login
				'''
			}
		}

		stage ('Docker build and push image')
		{
			container('docker')
			{
					sh '''
							set +x
							eval $(cat ecr-login)
					'''
					sh '''
						docker build -f Dockerfile -t s3-backend-job:latest .
						docker tag s3-backend-job:latest ${REPO_ACCT}.dkr.ecr.us-west-2.amazonaws.com/s3-backend-job:latest
						docker push ${REPO_ACCT}.dkr.ecr.us-west-2.amazonaws.com/s3-backend-job:latest
					'''
			}
		}
	}
}


