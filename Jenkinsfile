pipeline {
    agent{
        label { 'JDK-17' }
    }

    triggers{
        pollSCM('* * * * *')
    }

    stages{
        stage (vcs)  {
            steps {
                git  url: 'https://github.com/Praneethysp007/nopcommerce.git', 
                     branch: 'develop'
            }

        }
        stage(build) {
            steps{
                sh(script: 'dotnet build src/NopCommerce.sln')
            }
            
        }

        stage(Artifacts) {
            steps{
                archiveArtifacts(artifacts: '**/*.dll')
            }
            
        }
          
    }
}