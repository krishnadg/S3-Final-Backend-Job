node {
	stage ('Test')
	{
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
		)
	}

}


// stage ('Test') {
// 	podTemplate(
// 		label: 'dotnet-core-pod',
// 		containers: [
// 			containerTemplate(
// 				name: 'dotnet-core',
// 				image: 'microsoft/dotnet:1.1.2-sdk',
// 				ttyEnabled: true,
// 				command: 'cat',
				
// 			)
// 		]
// 	) {
// 		node('dotnet-core-pod') {
// 			container('dotnet-core') {

// 				checkout scm
// 				sh 'dotnet restore && dotnet test S3Tests/S3Tests.csproj --filter Category!=Integration'
// 			}
// 		}
// 	}
// }

// stage ('Dind') {
// 	podTemplate(
//     label: 'default',
//     inheritFrom: 'test',
//   ) {
//     node('default') {
//       container('test') {
//         checkout scm
//         sh '''
//           $(aws ecr get-login --no-include-email --region us-west-2)
//           docker build -f Dockerfile -t s3-backend-job:latest .
//           docker tag s3-backend-job:latest 543369334115.dkr.ecr.us-west-2.amazonaws.com/s3-backend-job:latest
//           docker push 543369334115.dkr.ecr.us-west-2.amazonaws.com/s3-backend-job:latest
//         '''
//       }
//     }
//   }
// }
