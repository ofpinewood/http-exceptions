ForEach ($folder in (Get-ChildItem -Path .\tests -Directory)) {
    $project = Get-ChildItem -Path ($folder.FullName + "\*.csproj") | Select-Object -First 1

    Write-Host $project

    dotnet test $project --configuration Release /p:CollectCoverage=true /p:Include="[Opw.*]*"
}

Read-Host -Prompt "Press ENTER to exit"