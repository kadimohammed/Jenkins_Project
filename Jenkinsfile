pipeline {
    agent any

    environment {
        DOCKER = '/usr/bin/docker'
        CONTAINER_NAME_PREFIX = 'pipeline'
        GIT_CREDS = credentials('github-credentials')
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
                    sudo ${DOCKER} compose down --remove-orphans
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
                    sh '''
                        # Stop and remove existing containers
                        sudo ${DOCKER} compose down --remove-orphans
                        
                        # Start new containers
                        sudo ${DOCKER} compose up -d
                    '''
                }
            }
        }

        stage('Push to Main') {
            steps {
                script {
                    sh '''
                        git config --global user.email "jenkins@example.com"
                        git config --global user.name "Jenkins Pipeline"
                        
                        # Configure git to use credentials
                        git remote set-url origin https://${GIT_CREDS_USR}:${GIT_CREDS_PSW}@github.com/kadimohammed/Jenkins_Project.git
                        
                        # Add all changes
                        git add .
                        
                        # Commit changes (will only commit if there are changes)
                        git diff --quiet && git diff --staged --quiet || git commit -m "Jenkins Pipeline: Automated commit [skip ci]"
                        
                        # Push to main branch
                        git push origin HEAD:main
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
                    sudo ${DOCKER} compose down --remove-orphans || true
                    
                    # Remove unused Docker resources
                    sudo ${DOCKER} system prune -f || true
                '''
            }
        }
    }
} 