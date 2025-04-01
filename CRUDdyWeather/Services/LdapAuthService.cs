namespace CRUDdyWeather.Services
{
    using Novell.Directory.Ldap;

    public class LdapAuthService
    {
        private readonly string _ldapServer = "ldap://cloud.booser.tech";
        private readonly int _ldapPort = 389;
        private readonly string _baseDn = "dc=booser,dc=tech";

        public bool Authenticate(string username, string password)
        {
            try
            {
                using var ldapConnection = new LdapConnection();
                ldapConnection.ConnectAsync(_ldapServer, _ldapPort);
                ldapConnection.BindAsync($"uid={username},{_baseDn}", password);

                return ldapConnection.Bound;
            }
            catch (LdapException)
            {
                return false;
            }
        }
    }
}
