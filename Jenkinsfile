pipeline {
    agent any

    environment {
        GIT_REPO = 'https://github.com/imdepak/DotnetProject.git'
        BRANCH = 'main'
        SOLUTION_FILE = 'MyDotnetProject.sln'
        CONFIGURATION = 'Release'
        BUILD_DIR = 'C:\\IIS\\Dotnet_TestProject' // IIS Deployment Path
        PUBLISH_DIR = "${WORKSPACE}\\bin\\${CONFIGURATION}\\net8.0\\publish"
    }

    stages {
        stage('Checkout') {
            steps {
                git url: "${GIT_REPO}", branch: "${BRANCH}"
            }
        }

        stage('Build') {
            steps {
                bat "dotnet build ${SOLUTION_FILE} --configuration ${CONFIGURATION}"
            }
        }

        stage('Publish') {
            steps {
                bat "dotnet publish ${SOLUTION_FILE} --configuration ${CONFIGURATION} --output \"${PUBLISH_DIR}\""
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
                    if (fileExists(PUBLISH_DIR)) {
                        echo "Copying published files to IIS deployment folder..."
                        bat "xcopy /E /I /H /Y \"${PUBLISH_DIR}\\*\" \"${BUILD_DIR}\\\""
                    } else {
                        error "Publish output folder not found: ${PUBLISH_DIR}"
                    }
                }
            }
        }

        stage('Start IIS') {
            steps {
                bat 'iisreset /start'
            }
        }

        stage('Deploy (Optional Restart)') {
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
