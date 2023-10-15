pipeline {
    agent{
        label { 'JDK-17' }
    }

    triggers{
        pollSCM ( * * * * * )
    }

    stages{
        stage (vcs)  {
            step {
                git  url: 'https://github.com/Praneethysp007/nopcommerce.git', 
                     branch: 'develop'
            }

        }
        stage(build) {
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

        stage(Artifacts) {
            archiveArtifacts(artifacts: '**/*.dll')
        }
          
    }
}