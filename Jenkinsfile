pipeline {
    agent{
        label 'JDK-17' 
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
                rtDotnetResolver(
                id : "nopinstance" 
                serverId : "nopcommerce"
                repo : "nopcomm-nuget"
                )
                rtDotnetRun(
                    args : "build src/NopCommerce.sln",
                    resolverId : "nopinstance"

                )
                rtPublishBuildInfo(
                    serverId: 'nopcommerce'
                    
                )
            }
            
        }

        stage(Artifacts) {
            steps{
                archiveArtifacts(artifacts: '**/*.dll')
            }
            
        }
          
    }
}