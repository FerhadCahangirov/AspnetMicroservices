using JwtAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "1FC9e51obZHFsLyf_kM4iyzfLUgPV045cfoBqZhUFlA7Y_QUq8fSF";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;

        private readonly List<UserAccount> _userAccountsList;

        public JwtTokenHandler()
        {
            _userAccountsList = new List<UserAccount>
            {
                new UserAccount { UserName = "admin", Password = "admin1234", Role = "Administrator"},
                new UserAccount { UserName = "user01", Password = "user01", Role = "User"}
            };
        }

        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticationRequest.UserName) || string.IsNullOrWhiteSpace(authenticationRequest.Password))
                return null;

            UserAccount? userAccount = _userAccountsList.FirstOrDefault(x => x.UserName == authenticationRequest.UserName && x.Password == authenticationRequest.Password);

            if (userAccount == null) return null;

            DateTime tokenExpiryTimeSpan = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);

            byte[] tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim("Role", userAccount.Role)
            });

            SigningCredentials signingCredentials = new SigningCredentials( new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeSpan,
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse()
            {
                UserName = authenticationRequest.UserName,
                ExpiresIn = (int)tokenExpiryTimeSpan.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token,
            };
        }

    }

}