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
                    image 'mcr.microsoft.com/dotnet/core/sdk:2.2'
                }
            }     
            steps {
                echo 'Building MAL Backend'
                sh 'dotnet restore MAL-Backend.sln'
                sh 'dotnet build MAL-Backend.sln -c Debug '
            }
        }
        stage('Test') {
            agent {
                docker {
                    reuseNode true
                    image 'mcr.microsoft.com/dotnet/core/sdk:2.2'
                }
            } 
            steps {
                echo 'Running MAL Auth Tests'
                sh 'dotnet test MAL.TEst.Tests -c Debug'
                echo 'Running MAL WebAPI Tests'
                sh 'dotnet test MAL.WebAPI.Tests -c Debug'
            }
        }
    }
    post {
        success {
            echo 'Successfull build'
            }
        } 
}
