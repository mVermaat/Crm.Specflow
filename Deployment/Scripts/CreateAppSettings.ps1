######################### Parameters #########################
param(
[string]$outputPath
)

######################### Script #########################
$output = "<appSettings>" +
"<add key=""Url"" value="""" />" +
"<add key=""Username"" value="""" />" +
"<add key=""Password"" value="""" />" +
"</appSettings>"

Out-File -FilePath $outputpath -InputObject $output