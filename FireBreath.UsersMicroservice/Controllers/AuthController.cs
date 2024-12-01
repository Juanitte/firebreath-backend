﻿using Common.Utilities;
using FireBreath.UserMicroservice.Helpers;
using FireBreath.UserMicroservice.Models.Dtos.EntityDto;
using FireBreath.UserMicroservice.Models.Dtos.ResponseDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities.Encoders;
using static EasyWeb.UserMicroservice.Services.UsersService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FireBreath.UsersMicroservice.Translations;

namespace FireBreath.UserMicroservice.Controllers
{
    public class AuthController : BaseController
    {
        #region Constructores

        public AuthController(IServiceProvider serviceCollection) : base(serviceCollection)
        {
        }

        #endregion

        #region Métodos públicos

        [HttpPost("users/authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto login)
        {

            try
            {
                var user = await IoTServiceUsers.GetByEmail(login.Email);
                if (user == null)
                {
                    return BadRequest(new ResponseLoginDto() { ErrorDescripcion = Translation_Account.User_not_found });
                }

                var authenticated = await IoTServiceUsers.Login(login);
                if (!authenticated)
                {
                    return BadRequest(new ResponseLoginDto() { ErrorDescripcion = Translation_Account.Incorrect_password });
                }

                var claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(Literals.Claim_UserId, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(Literals.Claim_Role, user.Role),
                    new Claim(Literals.Claim_FullName, user.FullName),
                    new Claim(Literals.Claim_Email, user.Email),
                    new Claim(Literals.Claim_PhoneNumber, user.PhoneNumber),
                    new Claim(Literals.Claim_LanguageId, user.Language.ToString())
                });

                var tokenHandler = new JwtSecurityTokenHandler();

                var appSettingsSection = Configuration.GetSection("AppSettings");

                var appSettings = appSettingsSection.Get<AppSettings>();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = JuanitEncoder.EncodeString(tokenHandler.WriteToken(token));

                return Ok(new ResponseLoginDto
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    LanguageId = user.Language,
                    Role = user.Role,
                    Token = tokenString
                });
            }
            catch (UserLockedException)
            {
                return BadRequest(new ResponseLoginDto()
                {
                    ErrorDescripcion = string.Format(Translation_UsersRoles.User_locked_message, login.Email)
                });
            }
            catch (UserSessionNotValidException)
            {
                return BadRequest(new ResponseLoginDto()
                {
                    ErrorDescripcion = Translation_Errors.Login_error
                });
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new ResponseLoginDto()
                {
                    ErrorDescripcion = Translation_Account.User_not_found
                });
            }
            catch (PasswordNotValidException)
            {
                return BadRequest(new ResponseLoginDto()
                {
                    ErrorDescripcion = Translation_Account.Incorrect_password
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseLoginDto()
                {
                    ErrorDescripcion = ex.Message
                });
            }
        }

        #endregion

    }
}
