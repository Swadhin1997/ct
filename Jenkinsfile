def project_folder = "C:/ProgramData/Jenkins/.jenkins/workspace/ct/PrjPASS/bin"
def JOB_NAME = "ct"
def backup_folder = 'C:/swadhin'

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
        stage ('Prep') {
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
         stage ('publish') {
              steps {
                   sh "dotnet publish PrjPASS.sln"
              }
         }
         stage ('Copy proj to backup') {
            steps {
                script {
                    echo "Copying project folder to backup folder"
                    sh "'powershell.exe cp -r ${project_folder} ${backup_folder}/${JOB_NAME}${currentBuild.number}_$timestamp'"
                    echo "Current timestamp :: $timestamp"
                }
            }
        }
    }
}
