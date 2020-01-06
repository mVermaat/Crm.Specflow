using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow
{
    public class UserDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserSettings UserSettings { get; set; } 
    }
}
