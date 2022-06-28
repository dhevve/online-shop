namespace shop
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}