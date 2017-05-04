nuget install NUnit.ConsoleRunner -Version 3.6.1 -OutputDirectory tools
nuget install OpenCover -Version 4.6.519 -OutputDirectory tools

nuget install coveralls.net -Version 0.412.0 -OutputDirectory tools

.\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:.\tools\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe -targetargs:"/nologo /noshadow .\NUnit.Tests1\bin\Release\NUnit.Tests1.dll" -filter:"+[*]* -[*.Tests]*" -register:user

.\tools\coveralls.net.0.412\tools\csmacnz.Coveralls.exe --opencover -i .\results.xml 