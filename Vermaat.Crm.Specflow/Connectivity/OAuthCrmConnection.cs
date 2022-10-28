using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Security;

namespace Vermaat.Crm.Specflow.Connectivity
{
    public class OAuthCrmConnection : CrmConnection
    {
        private readonly BrowserLoginDetails _loginInfo;
        private readonly string _appId;
        private readonly string _redirectUrl;

        public OAuthCrmConnection(string username, string password, SecureString mfaKey = null)
           : base(username)
        {
            _loginInfo = new BrowserLoginDetails
            {
                Username = username,
                Password = password.ToSecureString(),
                MfaKey = mfaKey,
                Url = HelperMethods.GetAppSettingsValue("Url", false)
            };
            _appId = HelperMethods.GetAppSettingsValue("AppId", false);
            _redirectUrl = HelperMethods.GetAppSettingsValue("RedirectUrl", false);
        }

        public OAuthCrmConnection(string username, string password, string appId, string redirectUrl, SecureString mfaKey = null)
            : base(username)
        {
            _loginInfo = new BrowserLoginDetails
            {
                Username = username,
                Password = password.ToSecureString(),
                Url = HelperMethods.GetAppSettingsValue("Url", false),
                MfaKey = mfaKey,
            };
            _appId = appId;
            _redirectUrl = redirectUrl;
        }

        public static OAuthCrmConnection FromAppConfig()
        {
            return new OAuthCrmConnection(
                HelperMethods.GetAppSettingsValue("Username", false),
                HelperMethods.GetAppSettingsValue("Password", false),
                HelperMethods.GetAppSettingsValue("AppId", false),
                HelperMethods.GetAppSettingsValue("RedirectUrl", false),
                HelperMethods.GetAppSettingsValue("MfaKey", true)?.ToSecureString());
        }

        public static OAuthCrmConnection AdminConnectionFromAppConfig()
        {
            var userName = HelperMethods.GetAppSettingsValue("AdminUsername", true) ?? HelperMethods.GetAppSettingsValue("Username");
            var password = HelperMethods.GetAppSettingsValue("AdminPassword", true) ?? HelperMethods.GetAppSettingsValue("Password");
            var mfaKey = HelperMethods.GetAppSettingsValue("AdminMfaKey", true)?.ToSecureString() ?? HelperMethods.GetAppSettingsValue("MfaKey", true)?.ToSecureString();
            return new OAuthCrmConnection(userName, password,
                HelperMethods.GetAppSettingsValue("AppId", false),
                HelperMethods.GetAppSettingsValue("RedirectUrl", false),
                mfaKey);
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
