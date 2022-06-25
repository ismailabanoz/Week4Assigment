using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Week4Assigment.API
{
    public class SignService
    {
        public static SecurityKey GetSymetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
