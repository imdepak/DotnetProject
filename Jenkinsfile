pipeline {
    agent any

    environment {
        GIT_REPO = 'https://github.com/imdepak/DotnetProject.git'
        BUILD_DIR = 'C:\\IIS\\Dotnet_TestProject'
        BIN_DIR = 'D:\\Projects\\DotNet Projects\\Training\\MyDotnetProject\\bin\\Release\\net8.0' // Update to match the output publish path
    }

    stages {
        stage('Checkout') {
            steps {
                git url: "${GIT_REPO}", branch: 'main'
            }
        }
        
         stage('Build') {
            steps {
                bat 'dotnet build "D:\\Projects\\DotNet Projects\\Training\\MyDotnetProject\\MyDotnetProject.sln" --configuration Release'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish "D:\\Projects\\DotNet Projects\\Training\\MyDotnetProject\\MyDotnetProject.sln" --configuration Release --output "${BUILD_DIR}"'
            }
        }
        stage('Copy to IIS') {
            steps {
                script {
                    // Make sure bin directory exists and contains files
                    if (fileExists("${BIN_DIR}")) {
                        echo "Copying files from ${BIN_DIR} to ${BUILD_DIR}..."
                        bat "xcopy /E /I /H /Y \"${BIN_DIR}\" \"${BUILD_DIR}\""
                    } else {
                        error "Publish output folder does not exist or is empty!"
                    }
                }
            }
        }
        stage('Deploy') {
            steps {
                bat 'iisreset /restart'
            }
        }
    }
}
