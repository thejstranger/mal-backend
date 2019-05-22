pipeline {
    agent {
        node {
            label 'docker'
        }
    }

    stages {
        stage('Build') {
            agent {
                docker {
                    reuseNode true
                    image 'mcr.microsoft.com/dotnet/core/sdk:2.2-alpine'
                }
            }
            steps {
                sh 'dotnet build MAL-Backend.sln -c Debug -r linux-musl-x64'
            }
        }
    }
}
