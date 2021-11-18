using System.Collections.Generic;
using ToMo.hAngularProject.Security.Model;

namespace ToMo.hAngularProject.Security
{
    public interface IAuthService
    {
        string GenerateJwtToken(LoginUser userUserName);
        string Hash(string password);
        List<Permission> GetPermissions(int userId);
    }
}