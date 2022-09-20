using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow
{
    public class UserProfileHandler
    {
        private readonly Lazy<Dictionary<string, UserProfile>> _userProfiles;

        public UserProfileHandler()
        {
            _userProfiles = new Lazy<Dictionary<string, UserProfile>>(LoadUserProfiles);
        }


        public UserProfile GetProfile(string name)
        {
            if (_userProfiles.Value.TryGetValue(name, out var profile))
                return profile;
            else
                throw new TestExecutionException(Constants.ErrorCodes.USERPROFILE_NOT_FOUND, name);
        }


        private Dictionary<string, UserProfile> LoadUserProfiles()
        {
            FileInfo dllPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            FileInfo file = new FileInfo(Path.Combine(dllPath.DirectoryName, "Users.json"));
            Logger.WriteLine($"Loading profiles from {file.FullName}");
            if (!file.Exists)
                throw new TestExecutionException(Constants.ErrorCodes.USERPROFILE_FILE_NOT_FOUND, file.FullName);

            var profiles = JsonConvert.DeserializeObject<UserProfile[]>(File.ReadAllText(file.FullName));
            Logger.WriteLine($"Found {profiles.Length} profiles: {string.Join(", ", profiles.Select(p => p.Profile))}");

            return profiles.ToDictionary(p => p.Profile);
        }

    }
}
