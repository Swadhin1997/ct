pipeline {
agent any

     environment {
        def timestamp = sh(script: "echo `date +%Y-%m-%d-%H-%M-%S`", returnStdout: true).trim()
        //def timestamp = sh(script: "echo `date +%s`", returnStdout: true).trim()
    }

     options {
        timestamps()
    }
    stages {  

        stage ("Clone Repository") {
                steps {
                   git branch: 'main', url: 'https://github.com/Swadhin1997/ct.git'
                }
            }  
        stage('Prep') {
            steps {
                script {
                    GIT_BRANCH=sh(returnStdout: true, script: 'git symbolic-ref --short HEAD').trim()
                    currentBuild.setDisplayName("#${currentBuild.number} [" + GIT_BRANCH + "]")
                    sh "export GIT_BRANCH=$GIT_BRANCH"
                }
            }
        }  
        stage ('Building dll') {
            steps {
               sh "dotnet build PrjPASS.sln"
            }           
        }
    }
}
