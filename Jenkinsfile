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
            def changeAuthors = []
            def changeFiles = []

            def changeLogSets = currentBuild.changeSets
            for (changeSet in changeLogSets) {
                for (entry in changeSet.items) {
                    changeAuthors << entry.author.fullName
                   for (file in entry.affectedFiles) {
                        def path = file.path
                        // Skip bin/, obj/, .vs/, .git/, .idea/
                        if (!path.matches(/^.*\\b(bin|obj|.vs|.git|.idea)\\b.*/)) {
                            changeFiles << path
                        }
                    }
                }
            }

            def uniqueAuthors = changeAuthors.unique().join(', ')
            def fileList = changeFiles ? "<ul><li>${changeFiles.join('</li><li>')}</li></ul>" : "<i>No files changed</i>"

            emailext(
                from: 'info@leitensmartvms.com',
                to: 'deepak.v@leitenindia.com',
                replyTo: 'info@leitensmartvms.com',
                subject: "Build ${currentBuild.currentResult}: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                body: """
                    <p>The build for <strong>${env.JOB_NAME}</strong> #${env.BUILD_NUMBER} has completed with status: 
                    <span style="color:${currentBuild.currentResult == 'SUCCESS' ? 'green' : 'red'};">
                        <strong>${currentBuild.currentResult}</strong>
                    </span>.</p>

                    <p><strong>Triggered By:</strong> ${uniqueAuthors ?: 'Unknown (maybe scheduled build)'}</p>

                    <p><strong>Files Changed:</strong></p>
                    ${fileList}

                    <p><a href="${env.BUILD_URL}">Click here</a> to view full build details.</p>
                """,
                mimeType: 'text/html'
            )
        }
    }
}

}
