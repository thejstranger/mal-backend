pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                echo 'Building MAL Backend'
                sh 'dotnet restore MAL-Backend.sln'
                sh 'dotnet build MAL-Backend.sln -c Debug '
            }
        }
    }

    post {
        success {
            echo 'Successfull build'
        }
    }
}
