using System;

namespace WebFormsApp.Service.Abstract
{
    public interface ITokenService
    {
        Tuple<string, string> GenerateToken(string username);
        bool ValidateToken(string token, string secretkey);
    }
}
