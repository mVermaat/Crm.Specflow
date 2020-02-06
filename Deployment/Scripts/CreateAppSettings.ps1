$output = "<appSettings>" +
"<add key=""AuthType"" value=""Office365"" />" +
"<add key=""Url"" value="""" />" +
"<add key=""Username"" value="""" />" +
"<add key=""Password"" value="""" />" +
"</appSettings>"

Out-File -FilePath "$env:SYSTEM_DEFAULTWORKINGDIRECTORY\Vermaat.Crm.Specflow.Sample\appsettings.config" -InputObject $output