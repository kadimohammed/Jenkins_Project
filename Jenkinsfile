pipeline {
    agent any

    environment {
        DOCKER = '/usr/bin/docker'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build') {
            steps {
                sh '''
                    whoami
                    id
                    ${DOCKER} ps
                    ${DOCKER} --version
                    ${DOCKER} compose version
                    sudo ${DOCKER} compose build
                '''
            }
        }

        stage('Test') {
            steps {
                script {
                    sh '''
                        export PATH="/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/snap/bin:$PATH"
                        dotnet --version
                        cd AspNetApp
                        dotnet test
                    '''
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    sh "sudo ${DOCKER} compose up -d"
                }
            }
        }
    }

    post {
        always {
            script {
                sh "sudo ${DOCKER} compose down || true"
            }
        }
    }
} 