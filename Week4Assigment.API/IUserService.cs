namespace Week4Assigment.API
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        string GetTokenWithRefreshToken(string refreshToken);
    }
}
