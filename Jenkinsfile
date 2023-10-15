pipeline {
    agent{
        label { 'JDK-17' }
    }

    triggers{
        pollSCM('* * * * *')
    }

    stages{
        stage (vcs)  {
            step {
                git  url: 'https://github.com/Praneethysp007/nopcommerce.git', 
                     branch: 'develop'
            }

        }
        stage(build) {
            sh(script: 'dotnet build src/NopCommerce.sln')
        }

        stage(Artifacts) {
            archiveArtifacts(artifacts: '**/*.dll')
        }
          
    }
}