######################### Parameters #########################
param(
[string]$outputPath
)

######################### Script #########################
$output = "<appSettings>" +
"<add key=""AuthType"" value="""" />" +
"<add key=""Url"" value="""" />" +
"<add key=""Username"" value="""" />" +
"<add key=""Password"" value="""" />" +
"<add key=""ClientId"" value="""" />" +
"<add key=""ClientSecret"" value="""" />" +
"</appSettings>"

Out-File -FilePath $outputpath -InputObject $output