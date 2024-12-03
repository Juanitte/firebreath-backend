﻿using Common.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using FireBreath.UsersMicroservice.Models.Entities;
using FireBreath.UsersMicroservice.Models.Dtos.EntityDto;
using static FireBreath.UsersMicroservice.Services.UsersService;
using System.Security.Cryptography;
using FireBreath.UsersMicroservice.Models.UnitsOfWork;

namespace FireBreath.UsersMicroservice.Services
{
    /// <summary>
    ///     Interfaz que define los métodos referentes al usuario actual
    /// </summary>
    public interface IIdentitiesService
    {
        /// <summary>
        ///     Método para el inicio de sesión del usuario en la aplicación
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rememberUser"></param>
        /// <returns></returns>
        Task<bool> Login(User userDb, string password, bool rememberUser);

        /// <summary>
        ///     Crea un nuevo usuario en el sistema
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        /// <param name="roleName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> CreateUser(User user, string roleName, string password);

        /// <summary>
        ///     Cierra la sesión del usuario
        /// </summary>
        /// <returns></returns>
        Task Logoff();

        /// <summary>
        ///     Obtiene un token de verificación de email
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetTokenPassword(User user);

        /// <summary>
        ///     Crea un nuevo rol
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        Task<IdentityResult> CreateRole(RoleDto roleDto);

        /// <summary>
        ///     Obtiene los roles del usuario pasado como parámetro
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IList<string>> GetUserRoles(User user);

        /// <summary>
        ///     Actualiza la contraseña del usuario
        /// </summary>
        /// <param name="user"><see cref="User"/> con los datos del usuario</param>
        /// <param name="password">Nueva contraseña</param>
        /// <returns></returns>
        Task<bool> UpdateUserPassword(User user, string password);

        /// <summary>
        ///     Genera el usuario SupportManager
        /// </summary>
        /// <returns></returns>
        Task<User> DefaultUser();
    }

    /// <summary>
    ///     Implementación de la interfaz IIdentitiesService<see cref="IIdentitiesService"/>
    /// </summary>
    public class IdentitiesService : BaseService, IIdentitiesService
    {
        #region Miembros privados

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;

        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="ioTUnitOfWork"></param>
        /// <param name="logger"></param>
        public IdentitiesService(UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager,
            JuaniteUnitOfWork juaniteUnitOfWork, ILogger logger) : base(juaniteUnitOfWork, logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

            DefaultRoles().Wait();
            DefaultUser().Wait();
        }

        #endregion

        #region Implementación interfaz IIdentitiesService

        /// <summary>
        ///     Método para el inicio de sesión del usuario de la aplicación
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <param name="rememberUser">Define si se debe mantener la sesión iniciada en el equipo</param>
        /// <returns>Resultado de la operación</returns>
        /// <exception cref="UserLockedException"></exception>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="PasswordNotValidException"></exception>
        public async Task<bool> Login(User userDb, string password, bool rememberUser)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userDb, password, rememberUser, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(userDb.Email);
                    var roles = await GetUserRoles(user);

                    await SetUserClaims(user);

                    return true;
                }
                else if (result.IsLockedOut)
                {
                    throw new UserLockedException();
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(userDb.Email);
                    if (user == null)
                    {
                        throw new UserNotFoundException();
                    }
                    else
                    {
                        throw new PasswordNotValidException();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, "IdentitiesService.Login");
                throw;
            }
        }

        /// <summary>
        ///     Crea un nuevo usuario en el sistema
        /// </summary>
        /// <param name="user">Datos del usuario</param>
        /// <param name="roleName">Rol del usuario</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Resultado de la operación</returns>
        public async Task<IdentityResult> CreateUser(User user, string roleName, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                    return roleResult;
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.CreateUser");
                throw;
            }
        }

        /// <summary>
        ///     Cierra la sesión actual del usuario
        /// </summary>
        /// <returns></returns>
        public async Task Logoff()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.Logoff");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el token para resetear la contraseña del usuario
        /// </summary>
        /// <param name="user">El usuario</param>
        /// <returns>El token</returns>
        public async Task<string> GetTokenPassword(User user)
        {
            try
            {
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.GetTokenPassword");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene un token para un usuario y un propósito específicados
        /// </summary>
        /// <param name="user">El usuario</param>
        /// <param name="purpose">El propósito</param>
        /// <returns>El token</returns>
        public async Task<string> GetPurposeToken(User user, string purpose)
        {
            try
            {
                return await _userManager.GenerateUserTokenAsync(user, "Default", purpose);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.GetPurposeToken");
                throw;
            }
        }

        /// <summary>
        ///     Valida un token para un usuario y propósito especificados
        /// </summary>
        /// <param name="user">El usuario</param>
        /// <param name="purpose">El propósito</param>
        /// <param name="token">El token</param>
        /// <returns></returns>
        public async Task<bool> VerifyUserToken(User user, string purpose, string token)
        {
            try
            {
                return await _userManager.VerifyUserTokenAsync(user, "Default", purpose, token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.VerifyUserToken");
                throw;
            }
        }

        /// <summary>
        ///     Crea un nuevo rol
        /// </summary>
        /// <param name="roleDto"><see cref="RoleDto"/></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateRole(RoleDto roleDto)
        {
            try
            {
                var role = new IdentityRole<int>()
                {
                    Name = roleDto.Name,
                };
                return await _roleManager.CreateAsync(role);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.CreateRole");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los roles del usuario pasado como parámetro
        /// </summary>
        /// <param name="user"><see cref="User"/> con los datos del usuario</param>
        /// <returns>Roles del usuario</returns>
        public async Task<IList<string>> GetUserRoles(User user)
        {
            try
            {
                return await _userManager.GetRolesAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.GetUserRoles");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los usuarios que pertenezcan al rol pasado como parámetro
        /// </summary>
        /// <param name="roleName">El nombre del rol</param>
        /// <returns>Los usuarios pertenecientes a ese rol</returns>
        public async Task<IList<User>> GetUsersByRole(string roleName)
        {
            try
            {
                return await _userManager.GetUsersInRoleAsync(roleName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.GetUsersByRole");
                throw;
            }
        }

        /// <summary>
        ///     Establece o actualiza los claims del usuario pasado como parámetro
        /// </summary>
        /// <param name="user"><see cref="User"/> con los datos del usuario</param>
        /// <param name="refreshSession">Determina si se ha de refrescar la sesión del usuario actual (por defecto false)</param>
        /// <returns></returns>
        public async Task SetUserClaims(User user, bool refreshSession = false)
        {
            try
            {
                var claimsForUser = await _userManager.GetClaimsAsync(user);
                var rolesForUser = await _userManager.GetRolesAsync(user);

                //Claim nombre completo del usuario
                if (claimsForUser.Any(c => c.Type == Literals.Claim_Tag))
                {
                    await _userManager.ReplaceClaimAsync(user, claimsForUser.FirstOrDefault(c => c.Type == Literals.Claim_Tag), new Claim(Literals.Claim_Tag, user.Tag));
                }
                else
                {
                    await _userManager.AddClaimAsync(user, new Claim(Literals.Claim_Tag, user.Tag));
                }

                //Claim del id del idioma del usuario
                if (claimsForUser.Any(c => c.Type == Literals.Claim_LanguageId))
                {
                    await _userManager.ReplaceClaimAsync(user, claimsForUser.FirstOrDefault(c => c.Type == Literals.Claim_LanguageId), new Claim(Literals.Claim_LanguageId, user.Language.ToString()));
                }
                else
                {
                    await _userManager.AddClaimAsync(user, new Claim(Literals.Claim_LanguageId, user.Language.ToString()));
                }

                //Claim del rol del usuario
                if (claimsForUser.Any(c => c.Type == Literals.Claim_Role))
                {
                    await _userManager.ReplaceClaimAsync(user, claimsForUser.FirstOrDefault(c => c.Type == Literals.Claim_Role), new Claim(Literals.Claim_Role, rolesForUser != null ? rolesForUser.FirstOrDefault() : string.Empty));
                }
                else
                {
                    await _userManager.AddClaimAsync(user, new Claim(Literals.Claim_Role, rolesForUser != null ? rolesForUser.FirstOrDefault() : string.Empty));
                }

                //Claim del id del usuario
                if (claimsForUser.Any(c => c.Type == Literals.Claim_UserId))
                {
                    await _userManager.ReplaceClaimAsync(user, claimsForUser.FirstOrDefault(c => c.Type == Literals.Claim_UserId), new Claim(Literals.Claim_UserId, user.Id.ToString()));
                }
                else
                {
                    await _userManager.AddClaimAsync(user, new Claim(Literals.Claim_UserId, user.Id.ToString()));
                }

                if (refreshSession)
                {
                    await RefreshUserSession(user);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.SetUserClaims");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los claims del usuario pasado como parámetro
        /// </summary>
        /// <param name="user"><see cref="User"/> con los datos del usuario</param>
        /// <returns></returns>
        public async Task<IList<Claim>> GetClaims(User user)
        {
            try
            {
                return await _userManager.GetClaimsAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.GetClaims");
                throw;
            }
        }

        /// <summary>
        ///     Actualiza la contraseña del usuario
        /// </summary>
        /// <param name="user"><see cref="User"/> con los datos del usuario</param>
        /// <param name="password">Nueva contraseña</param>
        /// <returns></returns>
        public async Task<bool> UpdateUserPassword(User user, string password)
        {
            try
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                _unitOfWork.DetachLocal(user, user.Id.ToString());
                var result = await _userManager.ResetPasswordAsync(user, resetToken, password);
                await _userManager.UpdateAsync(user);

                return result.Succeeded;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.UpdateUserPassword");
                throw;
            }
        }

        /// <summary>
        ///     Actualiza el rol de un usuario
        /// </summary>
        /// <param name="user"><see cref="User"/> con los datos del usuario</param>
        /// <param name="roleName">El nombre del nuevo rol</param>
        /// <returns></returns>
        public async Task<bool> UpdateUserRole(User user, string roleName)
        {
            try
            {
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    return true;
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                return roleResult.Succeeded;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.UpdateUserRole");
                throw;
            }
        }

        #endregion

        #region Métodos privados

        /// <summary>
        ///     Genera los roles de la aplicación
        /// </summary>
        /// <returns></returns>
        public async Task DefaultRoles()
        {
            try
            {
                var roles = new List<IdentityRole<int>>
                {
                    new IdentityRole<int>() {Name = Literals.Role_Admin},
                    new IdentityRole<int>() {Name = Literals.Role_User}
                };

                foreach (var role in roles)
                {
                    if (_roleManager.FindByNameAsync(role.Name).Result == null)
                    {
                        var result = await _roleManager.CreateAsync(role);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IdentitiesService.DefaultRoles");
                throw;
            }
        }

        /// <summary>
        ///     Genera el usuario SupportManager
        /// </summary>
        /// <returns></returns>
        public async Task<User> DefaultUser()
        {
            const string userName = "Admin";
            string password = HashPassword("Pass@123");

            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    user = new User()
                    {
                        UserName = userName,
                        Email = "admin@gmail.com",
                        PhoneNumber = "123456789",
                        FullName = "Administrator",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        LockoutEnabled = false,
                        LockoutEnd = null,
                        Role = Literals.Role_Admin,
                        Language = Language.Spanish
                    };

                    var result = _userManager.CreateAsync(user, password).Result;
                    if (result.Succeeded)
                    {
                        var rolesForUser = _userManager.GetRolesAsync(user).Result;
                        if (!rolesForUser.Contains(Literals.Role_Admin))
                        {
                            await _userManager.AddToRoleAsync(user, Literals.Role_Admin);
                        }

                        await SetUserClaims(user);

                        await _unitOfWork.SaveChanges();
                    }
                }
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, "IdentitiesService => DefaultUser");
                throw;
            }
        }

        /// <summary>
        ///     Vuelve a iniciar sesión del usuario actual para refrescar los claims
        /// </summary>
        /// <param name="user">Usuario logeado</param>
        /// <returns></returns>
        private async Task RefreshUserSession(User user)
        {
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);
        }

        #endregion

        #region Métodos privados

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
