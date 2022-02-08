using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Vermaat.Crm.Specflow.Connectivity
{
    class OAuthCrmConnection : CrmConnection
    {
        private readonly BrowserLoginDetails _loginInfo;
        private readonly string _appId;
        private readonly string _redirectUrl;

        public OAuthCrmConnection(string username, string password, string appId, string redirectUrl)
            : base(username)
        {
            _loginInfo = new BrowserLoginDetails
            {
                Username = username,
                Password = password.ToSecureString(),
                Url = HelperMethods.GetAppSettingsValue("Url", false)
            };
            _appId = appId;
            _redirectUrl = redirectUrl;
        }

        public static OAuthCrmConnection FromAppConfig()
        {
            return new OAuthCrmConnection(
                HelperMethods.GetAppSettingsValue("Username", false),
                HelperMethods.GetAppSettingsValue("Password", false),
                HelperMethods.GetAppSettingsValue("ClientId", false),
                HelperMethods.GetAppSettingsValue("RedirectUrl", false));
        }

        public static OAuthCrmConnection AdminConnectionFromAppConfig()
        {
            var userName = HelperMethods.GetAppSettingsValue("AdminUsername", true) ?? HelperMethods.GetAppSettingsValue("Username");
            var password = HelperMethods.GetAppSettingsValue("AdminPassword", true) ?? HelperMethods.GetAppSettingsValue("Password");
            return new OAuthCrmConnection(userName, password,
                HelperMethods.GetAppSettingsValue("ClientId", false),
                HelperMethods.GetAppSettingsValue("RedirectUrl", false));
        }


        public override CrmService CreateCrmServiceInstance()
        {
            return new CrmService($"AuthType=OAuth;Url='{_loginInfo.Url}';Username='{_loginInfo.Username}';Password='{_loginInfo.Password.ToUnsecureString()}';AppId='{_appId}';RedirectUri='{_redirectUrl}';LoginPrompt=Never;RequireNewInstance=True");
        }

        public override BrowserLoginDetails GetBrowserLoginInformation()
        {
            return _loginInfo;
        }
    }
}
