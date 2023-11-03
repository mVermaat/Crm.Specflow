namespace Vermaat.Crm.Specflow.Entities
{
    public class UserProfile
    {
        public string Username { get; set; }
        public bool MFA { get; set; }

        public string SecretName { get; set; }

        public string Profile { get; set; }
    }
}
