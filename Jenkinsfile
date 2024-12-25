pipeline {
    agent any

    environment {
        DOCKER = '/usr/bin/docker'
        PROJECT_NAME = "${env.JOB_NAME.replaceAll(/[^a-zA-Z0-9_]/, '_')}"
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
                    
                    # Stop and remove existing containers if they exist
                    sudo ${DOCKER} compose -p ${PROJECT_NAME} down --remove-orphans
                    sudo ${DOCKER} compose -p ${PROJECT_NAME} build
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
                    sh '''
                        # Stop and remove existing containers
                        sudo ${DOCKER} compose -p ${PROJECT_NAME} down --remove-orphans
                        
                        # Start new containers
                        sudo ${DOCKER} compose -p ${PROJECT_NAME} up -d
                    '''
                }
            }
        }
    }

    post {
        always {
            script {
                sh '''
                    # Cleanup: Stop and remove containers
                    sudo ${DOCKER} compose -p ${PROJECT_NAME} down --remove-orphans || true
                    
                    # Remove unused Docker resources
                    sudo ${DOCKER} system prune -f || true
                '''
            }
        }
    }
} 