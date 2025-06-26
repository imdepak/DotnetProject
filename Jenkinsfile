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
        stage('Stop IIS') {
            steps {
                bat 'iisreset /stop'
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
         stage('Start IIS') {
            steps {
                bat 'iisreset /start'
            }
        }
        stage('Deploy') {
            steps {
                bat 'iisreset /restart'
            }
        }
    }
    post {
    always {
        script {
            def changeAuthors = [] as Set
            def changeFiles = []

            for (changeSet in currentBuild.changeSets) {
                for (entry in changeSet.items) {
                    changeAuthors << entry.author.fullName
                    for (file in entry.affectedFiles) {
                        def path = file.path
                        // Skip bin/, obj/, .vs/, .git/, .idea folders
                        if (!path.toLowerCase().matches("(?i).*(/|\\\\)(bin|obj|.vs|.git|.idea)(/|\\\\).*")) {
                            changeFiles << path
                        }
                    }
                }
            }

            def authors = changeAuthors ? changeAuthors.join(', ') : 'Unknown (possibly scheduled or triggered manually)'
            def files = changeFiles ? "<ul><li>${changeFiles.join('</li><li>')}</li></ul>" : "<i>No relevant files changed</i>"

            emailext(
                from: 'info@leitensmartvms.com',
                to: 'deepak.v@leitenindia.com',
                replyTo: 'info@leitensmartvms.com',
                subject: "Build ${currentBuild.currentResult}: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                body: """
                    <p><strong>Build Result:</strong> 
                    <span style="color:${currentBuild.currentResult == 'SUCCESS' ? 'green' : 'red'};">
                        ${currentBuild.currentResult}
                    </span></p>

                    <p><strong>Triggered By:</strong> ${authors}</p>
                    <p><strong>Files Changed:</strong><br>${files}</p>

                    <p><a href="${env.BUILD_URL}">Click here</a> to view full console output.</p>
                """,
                mimeType: 'text/html'
            )
        }
    }
}
}