using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Week4Assigment.API
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _appDbContext;
        public UserService(IOptions<AppSettings> appSettings, AppDbContext appDbContext)
        {
            _appSettings = appSettings.Value;
            _appDbContext = appDbContext;
        }
        

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _appDbContext.Users.ToList().SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _appDbContext.Users.ToList();
        }

        public User GetById(int id)
        {
            return _appDbContext.Users.ToList().FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddSeconds(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            RefreshToken(user);
            return tokenHandler.WriteToken(token);
            
        }
        private string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private void RefreshToken(User user)
        {
            var refreshToken = CreateRefreshToken();
            var checkRefreshToken = _appDbContext.RefreshTokens.Find(user.Username);
            if (checkRefreshToken == null)
            {
                _appDbContext.RefreshTokens.Add(new RefreshToken { UserName = user.Username, Guid = refreshToken, ExpDate = DateTime.Now.AddDays(60) });
            }
            else
            {
                checkRefreshToken.Guid = refreshToken;
                checkRefreshToken.ExpDate = DateTime.Now.AddDays(60);
            }
            _appDbContext.SaveChanges();
        }

        string IUserService.GetTokenWithRefreshToken(string refreshToken)
        {
            var checkRefreshToken = _appDbContext.RefreshTokens.Where(p => p.Guid == refreshToken && p.ExpDate>=DateTime.Now).FirstOrDefault();
            if (checkRefreshToken==null)
            {
                return "Invalid refresh token";
            }
            else
            {
                var user = _appDbContext.Users.FirstOrDefault(p => p.Username == checkRefreshToken.UserName);
                return generateJwtToken(user);
            }
        }

    }
}
