pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                echo 'Building MAL Backend'
                sh 'dotnet build MAL-Backend.sln -c Debug -r linux-musl-x64'
            }
        }
    }

    post {
        success {
            echo 'Successfull build'
        }
    }
}
