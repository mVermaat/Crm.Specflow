######################### Parameters #########################
param(
[string]$outputPath
)

######################### Script #########################
$output = "<appSettings>" +
"<add key=""AuthType"" value=""Office365"" />" +
"<add key=""Url"" value="""" />" +
"<add key=""Username"" value="""" />" +
"<add key=""Password"" value="""" />" +
"</appSettings>"

Out-File -FilePath $outputpath -InputObject $output