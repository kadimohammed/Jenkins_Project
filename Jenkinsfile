pipeline {
    agent any

    environment {
        DOCKER_COMPOSE = 'docker-compose'
        APP_NAME = 'aspnet_app'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build') {
            steps {
                sh "${DOCKER_COMPOSE} build"
            }
        }

        stage('Test') {
            steps {
                script {
                    // Run tests for ASP.NET application
                    dir('AspNetApp') {
                        sh 'dotnet test'
                    }
                }
            }
        }

        stage('Deploy to Dev') {
            when {
                branch 'develop'
            }
            steps {
                script {
                    sh "${DOCKER_COMPOSE} -f docker-compose.yml -f docker-compose.dev.yml up -d"
                }
            }
        }

        stage('Deploy to Staging') {
            when {
                branch 'staging'
            }
            steps {
                script {
                    sh "${DOCKER_COMPOSE} -f docker-compose.yml -f docker-compose.staging.yml up -d"
                }
            }
        }

        stage('Deploy to Production') {
            when {
                branch 'main'
            }
            steps {
                script {
                    sh "${DOCKER_COMPOSE} -f docker-compose.yml -f docker-compose.prod.yml up -d"
                }
            }
        }
    }

    post {
        always {
            // Cleanup
            sh "${DOCKER_COMPOSE} down"
        }
    }
} 