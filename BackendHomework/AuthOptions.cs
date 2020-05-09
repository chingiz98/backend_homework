namespace BackendHomework
{
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
 
    namespace TokenApp
    {
        public class AuthOptions
        {
            public const string ISSUER = "BackendServer";
            public const string AUDIENCE = "BackendClient";
            const string KEY = "very_very_secret_key_1234554321";
            public const int LIFETIME = 45;
            public static SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }
        }
    }
}