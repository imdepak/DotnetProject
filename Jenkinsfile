pipeline {
    agent any

    environment {
        GIT_REPO = 'https://github.com/imdepak/DotnetProject.git'
        BUILD_DIR = 'C:\\IIS\\Dotnet_TestProject'
    }

    stages {
        stage('Checkout') {
            steps {
                git url: "${GIT_REPO}", branch: 'main'
            }
        }
        
         stage('Build') {
            steps {
                bat 'dotnet build D:\\Projects\\DotNet Projects\\Training\\MyDotnetProject\\MyDotnetProject.sln
 --configuration Release'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish D:\\Projects\\DotNet Projects\\Training\\MyDotnetProject\\MyDotnetProject.sln
 --configuration Release --output "${BUILD_DIR}"'
            }
        }

        stage('Deploy') {
            steps {
                bat 'iisreset /restart'
            }
        }
    }
}
