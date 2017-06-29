properties([
	parameters([
		
		string(description: 'environment', name: 'ENVIRONMENT')
	])
])


stage ('Deploy') {
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
				sh 'echo "hello"'
			}
		}
	}
}