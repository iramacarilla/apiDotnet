using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetApi.Data;
using DotnetApi.Dtos;
using DotnetApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotnetApi.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config) //constructer
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);//with underscore we have private fields name

        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email='"
                 + userForRegistration.Email + "'";
                IEnumerable<string> existUser = _dapper.LoadData<string>(sqlCheckUserExists);
                if (existUser.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128/8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create() )
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                   byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlAddAuth = @"
                    INSERT INTO TutorialAppSchema.Auth (
                        [Email]
                        ,[PasswordHash]
                        ,[PasswordSalt]) VALUES ('" + userForRegistration.Email +
                        "',@PasswordHash, @PasswordSalt)"; 

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    SqlParameter passwordSaltParameter  = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;   
                     SqlParameter passwordHashParameter  = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                     sqlParameters.Add(passwordHashParameter);
                     if (_dapper.ExecuteSqlWithParametrs(sqlAddAuth, sqlParameters))
                     {
                    string sqlAddUser = @"
                    INSERT INTO TutorialAppSchema.Users(
                     [FirstName]
                     , [LastName]
                     , [Email]
                    , [Gender]
                     , [Active]
                    ) VALUES (" +
                     "'"+ userForRegistration.FirstName + 
                      "',  '"+ userForRegistration.LastName + 
                       "','"+ userForRegistration.Email +
                       "',  '"+ userForRegistration.Gender + 
                     "', 1)";
                     if (_dapper.ExecuteSql(sqlAddUser))
                     {

                        return Ok();
                     }
                      throw new Exception("Failed to add user.");
                     }

                     throw new Exception("Failed to register user.");
                }
                throw new Exception("User with this email ready exist"); 
            
            }
            throw new Exception("Password not match!");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashandSalt  = @"SELECT 
            [PasswordHash], 
            [PasswordSalt] 
            FROM TutorialAppSchema.Auth WHERE Email = '" +
            userForLogin.Email + "'";

            UserForLoginConformationDto userForLoginConformation = _dapper
                .LoadDataSingle<UserForLoginConformationDto>(sqlForHashandSalt);

             byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForLoginConformation.PasswordSalt);

           // if (passwordHash == userForLoginConformation.PasswordHash) // will not work (object)
            for (int ind = 0; ind < passwordHash.Length; ind++)
            {
                if (passwordHash[ind] != userForLoginConformation.PasswordHash[ind])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }
            string userIdSql = @"  SELECT 
            UserId FROM TutorialAppSchema.Users 
            WHERE Email = '" + userForLogin.Email +
            "'";
            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>
            {
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        [HttpGet("RefreshToken")]

        public string RefreshToken()
        {
            string userIdSql = @"  
            SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" + 
            User.FindFirst("userId")?.Value + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);
            return _authHelper.CreateToken(userId);
        }
        
    }
}