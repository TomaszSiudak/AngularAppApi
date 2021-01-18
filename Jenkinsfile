pipeline {
    agent any
    stages 
    {
        stage ('CleanIIS')   
        {
            steps
            {
                    bat label: 'Stop IIS', script: 'net stop W3SVC'
                    powershell 'Remove-Item C:\\inetpub\\wwwroot\\petbook\\* -Recurse -Force'
            }
        }
        stage('GetCodeFromGit') 
        {
            steps 
            {
                git credentialsId: 'd9f43553-5300-4317-af72-af1c5128eaf5', url: 'https://github.com/TomaszSiudak/AngularAppApi.git'
            }
        }
        
        stage('Build') 
        {
            steps
                {
                    bat label: 'restoreNugetLibraries',  script: '"%WORKSPACE%\\BuildTools\\nuget.exe" restore "%WORKSPACE%\\PetBookAPI\\PetBookAPI.sln"'
                    bat label: 'buildSolution', script: 'dotnet publish "%WORKSPACE%\\PetBookAPI\\PetBookAPI.sln" -c RELEASE -o C:\\inetpub\\wwwroot\\petbook'
                
                }
        }
        stage('DropDB') 
        {
            steps 
            {
                bat label: 'dropbDB', script: 'sqlcmd -U sa -P test -S .\\SQLEXPRESS -q "drop database PetBook"'
            }
        }
        stage ('StartIIS')   
        {
            steps
            {
                    bat label: 'Start IIS', script: 'net start w3svc'
                    bat label: 'start api',  script: 'start chrome http://localhost:5000'
                    sleep 5
                    powershell('Invoke-WebRequest http://localhost:5000 -Method "GET"' )
                    sleep 30
            }
        }
    }
}