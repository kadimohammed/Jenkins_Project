pipeline {
    agent any

    environment {
        DOCKER = '/usr/bin/docker'
        PROJECT_NAME = "${env.JOB_NAME.replaceAll(/[^a-zA-Z0-9_]/, '_').toLowerCase()}"
    }

    stages {
        stage('Cleanup') {
            steps {
                sh '''
                    # Clean up any existing containers and resources
                    sudo ${DOCKER} compose -p ${PROJECT_NAME} down --remove-orphans --volumes || true
                    sudo ${DOCKER} system prune -f || true
                '''
            }
        }

        stage('Build') {
            steps {
                sh '''
                    # Build the images
                    sudo ${DOCKER} compose -p ${PROJECT_NAME} build --no-cache
                '''
            }
        }

        stage('Test') {
            steps {
                script {
                    sh '''
                        export PATH="/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/snap/bin:$PATH"
                        cd AspNetApp
                        dotnet restore
                        dotnet build
                        dotnet test
                    '''
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    sh '''
                        # Deploy the application
                        sudo ${DOCKER} compose -p ${PROJECT_NAME} up -d --force-recreate
                        
                        # Wait for containers to be healthy
                        sleep 10
                        
                        # Verify containers are running
                        sudo ${DOCKER} compose -p ${PROJECT_NAME} ps
                    '''
                }
            }
        }

        stage('Verify') {
            steps {
                script {
                    sh '''
                        # Wait for services to be ready
                        sleep 5
                        
                        # Check if services are running
                        sudo ${DOCKER} compose -p ${PROJECT_NAME} ps --format json
                        
                        # Check ASP.NET application logs
                        sudo ${DOCKER} compose -p ${PROJECT_NAME} logs aspnet_app
                    '''
                }
            }
        }
    }

    post {
        always {
            script {
                sh '''
                    # Cleanup if the build fails
                    sudo ${DOCKER} compose -p ${PROJECT_NAME} down --remove-orphans --volumes || true
                    sudo ${DOCKER} system prune -f || true
                '''
            }
        }
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed! Check the logs for details.'
        }
    }
} 