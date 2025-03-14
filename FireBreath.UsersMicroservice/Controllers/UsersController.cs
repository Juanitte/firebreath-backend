﻿using Common.Dtos;
using Common.Utilities;
using FireBreath.UsersMicroservice.Models.Dtos.CreateDto;
using FireBreath.UsersMicroservice.Models.Dtos.EntityDto;
using FireBreath.UsersMicroservice.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using FireBreath.UsersMicroservice.Translations;
using Duende.IdentityServer.Extensions;

namespace FireBreath.UsersMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : BaseController
    {
        #region Miembros privados

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<User> _userManager;

        #endregion

        #region Constructores

        public UsersController(IServiceProvider serviceCollection, IWebHostEnvironment hostingEnvironment, UserManager<User> userManager) : base(serviceCollection)
        {
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        ///     Método que obtiene todos los usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/getall")]
        public async Task<JsonResult> GetAll()
        {
            try
            {
                var users = await ServiceUsers.GetAll();
                return new JsonResult(users);
            }
            catch (Exception e)
            {
                return new JsonResult(new List<UserDto>());
            }
        }

        /// <summary>
        ///     Método que obtiene un usuario según su id
        /// </summary>
        /// <param name="userId">El id del usuario a buscar</param>
        /// <returns></returns>
        [HttpGet("users/getbyid/{id}")]
        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                var user = await ServiceUsers.GetById(id);
                return new JsonResult(user);
            }
            catch (Exception e)
            {
                return new JsonResult(new UserDto());
            }
        }

        /// <summary>
        ///     Método que crea un nuevo usuario
        /// </summary>
        /// <param name="userDto"><see cref="CreateUserDto"/> con los datos del usuario</param>
        /// <returns></returns>
        [HttpPost("users/create")]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            try
            {
                var errorList = ServiceUsers.ValidateUser(userDto).Result;
                if (errorList.IsNullOrEmpty())
                {

                    var user = new User
                    {
                        Tag = userDto.Tag,
                        Bio = userDto.Bio,
                        Avatar = userDto.Avatar,
                        UserName = userDto.UserName,
                        Email = userDto.Email,
                        PhoneNumber = userDto.PhoneNumber,
                        Country = userDto.Country,
                        Language = userDto.Language,
                        Created = userDto.Created,
                        FullName = userDto.FullName,
                        Role = userDto.Role,
                        IsBanned = userDto.IsBanned
                    };

                    string password = userDto.Password;

                    var createUser = await _userManager.CreateAsync(user, password);

                    if (!createUser.Succeeded)
                    {
                        var errorMessage = string.Join(", ", createUser.Errors.Select(error => error.Description));
                        return BadRequest(errorMessage);
                    }

                    await _userManager.AddToRoleAsync(user, user.Role);

                    return Ok(createUser);
                }

                return BadRequest(Extensions.ToDisplayList(errorList));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Método que actualiza un usuario con id proporcionado como parámetro
        /// </summary>
        /// <param name="userId">El id del usuario a editar</param>
        /// <param name="user"><see cref="CreateUserDto"/> con los nuevos datos de usuario</param>
        /// <returns></returns>
        [HttpPost("users/update/{userId}")]
        public async Task<IActionResult> Update(int userId, CreateUserDto userDto)
        {
            try
            {
                var result = await ServiceUsers.Update(userId, userDto);

                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return Problem(Translation_Errors.Error_user_update);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Método que cambia el idioma al usuario con id pasado como parámetro
        /// </summary>
        /// <param name="userId">El id del usuario a modificar</param>
        /// <param name="changeLanguage"><see cref="ChangeLanguageDto"/> con los datos del idioma</param>
        /// <returns></returns>
        [HttpPut("users/changelanguage/{userId}")]
        public async Task<IActionResult> ChangeLanguage(int userId, ChangeLanguageDto changeLanguage)
        {
            var response = new GenericResponseDto();
            try
            {
                await ServiceUsers.ChangeLanguage(changeLanguage, userId);
                response.ReturnData = true;
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Users/ChangeLanguage" };
                return Ok(response);
            }
            return Ok(response);
        }

        /// <summary>
        ///     Método que elimina un usuario cuyo id se ha pasado como parámetro
        /// </summary>
        /// <param name="id">el id del usuario a eliminar</param>
        /// <returns></returns>
        [HttpDelete("users/remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await ServiceUsers.Remove(id);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Users/Remove" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Users/Remove" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Obtiene los usuarios con rol Admin.
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/getadmins")]
        public async Task<JsonResult> GetAdmins()
        {
            try
            {
                var result = await ServiceUsers.GetAdmins();

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                return new JsonResult(new UserDto());
            }
        }

        /// <summary>
        ///     Obtiene los usuarios con rol User.
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/getusers")]
        public async Task<JsonResult> GetUsers()
        {
            try
            {
                var result = await ServiceUsers.GetUsers();

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                return new JsonResult(new UserDto());
            }
        }

        /// <summary>
        ///     Gestiona el seguir a un usuario
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        [HttpPost("users/followuser/{userId}/{followerId}")]
        public async Task<IActionResult> FollowUser(int userId, int followerId)
        {
            var response = new GenericResponseDto();
            try
            {
                var follow = new FollowDto(userId, followerId);
                await ServiceUsers.FollowUser(follow);
                response.ReturnData = true;
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Users/FollowUser" };
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        ///     Gestiona el bloquear a un usuario
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        [HttpPost("users/blockuser")]
        public async Task<IActionResult> BlockUser(BlockDto block)
        {
            var response = new GenericResponseDto();
            try
            {
                await ServiceUsers.BlockUser(block);
                response.ReturnData = true;
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Users/BlockUser" };
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        ///     Gestiona el dejar de seguir a un usuario
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        [HttpDelete("users/unfollow/{userId}/{followerId}")]
        public async Task<IActionResult> UnfollowUser(int userId, int followerId)
        {
            var response = new GenericResponseDto();
            try
            {
                var follow = new FollowDto(userId, followerId);
                var result = await ServiceUsers.UnfollowUser(follow);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Users/UnfollowUser" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Users/UnfollowUser" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Gestiona el desbloquear a un usuario
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        [HttpDelete("users/unblock")]
        public async Task<IActionResult> UnblockUser(BlockDto block)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await ServiceUsers.UnblockUser(block);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Users/UnblockUser" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Users/UnblockUser" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Envía un correo de restablecer contraseña al email proporcionado
        /// </summary>
        /// <param name="username">el nombre del correo</param>
        /// <param name="domain">el dominio del correo (ej. 'gmail')</param>
        /// <param name="tld">el final del correo (ej. '.com')</param>
        /// <returns></returns>
        [HttpGet("users/sendemail/{username}/{domain}/{tld}")]
        public async Task<IActionResult> SendEmail(string username, string domain, string tld)
        {
            try
            {
                ServiceUsers.SendMail(username, domain, tld);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(false);
            }
        }

        /// <summary>
        ///     Restablece la contraseña de un usuario
        /// </summary>
        /// <param name="resetPassword"><see cref="ResetPasswordDto"/> con los datos de restablecimiento de contraseña</param>
        /// <returns></returns>
        [HttpPost("users/resetpassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDto resetPass)
        {
            try
            {
                var user = await ServiceUsers.ResetPassword(resetPass);
                if (user != null)
                {
                    if (await ServiceIdentity.UpdateUserPassword(user, resetPass.Password))
                        return Ok(true);
                }
                return BadRequest(false);
            }
            catch (Exception e)
            {
                return BadRequest(false);
            }
        }

        /// <summary>
        ///     Comprueba si un usuario sigue a otro
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        [HttpGet("users/isfollowing/{userId}/{followerId}")]
        public async Task<bool> IsFollowing(int userId, int followerId)
        {
            try
            {
                var follow = new FollowDto(userId, followerId);
                if (await ServiceUsers.IsFollowing(follow))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Comprueba si un usuario tiene bloqueado a otro
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        [HttpGet("users/isblocked")]
        public async Task<bool> IsBlocked([FromQuery]BlockDto block)
        {
            try
            {
                if (await ServiceUsers.IsBlocked(block))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("users/getfollowers/{userId}")]
        public async Task<JsonResult> GetFollowers(int userId)
        {
            try
            {
                var result = await ServiceUsers.GetFollowers(userId);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                return new JsonResult(new UserDto());
            }
        }

        /// <summary>
        ///     Obtiene todos los usuarios seguidos por un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("users/getfollowing/{userId}")]
        public async Task<JsonResult> GetFollowing(int userId)
        {
            try
            {
                var result = await ServiceUsers.GetFollowing(userId);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                return new JsonResult(new UserDto());
            }
        }

        /// <summary>
        ///     Obtiene los usuarios con rol User filtrados.
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/getusersfilter/{searchString}")]
        public async Task<JsonResult> GetUsersFilter(string searchString)
        {
            try
            {
                var result = await ServiceUsers.GetUsersFilter(searchString);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                return new JsonResult(new UserDto());
            }
        }

        /// <summary>
        ///     Obtiene el avatar de un usuario
        /// </summary>
        /// <param name="userId">id del usuario</param>
        /// <returns></returns>
        [HttpGet("users/getavatar/{userId}")]
        public async Task<IActionResult> GetAvatar(int userId)
        {
            try
            {
                var user = await ServiceUsers.GetById(userId);
                if (user != null)
                {
                    string directoryPath = Path.Combine("C:/ProyectoIoT/Back/ApiTest/AttachmentStorage/", userId.ToString());
                    string filePath = Path.Combine(directoryPath, user.Avatar);

                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                        string contentType = "application/octet-stream";

                        return File(fileBytes, contentType, user.Avatar);
                    }
                }
                return NotFound("File not found");
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        #endregion

        #region Métodos Privados



        /// <summary>
        ///     Hashea una contraseña igual que el frontend
        /// </summary>
        /// <param name="password">la contraseña a hashear</param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return string.Concat(builder.ToString(), "@", "A", "a");
            }
        }

        #endregion
    }
}
