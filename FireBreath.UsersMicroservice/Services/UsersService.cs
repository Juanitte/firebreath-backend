using Common.Utilities;
using FireBreath.UsersMicroservice.Models.Dtos.CreateDto;
using FireBreath.UsersMicroservice.Models.Dtos.EntityDto;
using FireBreath.UsersMicroservice.Models.Entities;
using FireBreath.UsersMicroservice.Models.UnitsOfWork;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using FireBreath.UsersMicroservice.Translations;
using Microsoft.AspNetCore.Mvc;
using Common.Services;

namespace FireBreath.UsersMicroservice.Services
{
    public interface IUsersService
    {
        /// <summary>
        ///     Crea un token para reestablecer la contraseña
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns></returns>
        public Task<string> CreateTokenPassword(int userId);

        /// <summary>
        ///     Crea un token para un usuario y con un propósito específicos
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="purpose">Propósito del token</param>
        /// <returns></returns>
        Task<string> CreatePurposeToken(int userId, string purpose);

        /// <summary>
        ///     Valida un token para un usuario y con un propósito específicos
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="purpose">Propósito del token</param>
        /// <param name="token">Token a validar</param>
        /// <returns></returns>
        Task<bool> ValidateUserToken(int userId, string purpose, string token);

        /// <summary>
        ///     Realiza el login en la aplicación
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        Task<bool> Login(LoginDto loginDto, bool? rememberUser = false);

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        /// <returns></returns>
        Task<List<UserDto>> GetAll(int page, int pageSize = 10);

        /// <summary>
        ///     Obtiene un usuario según su nombre de usuario
        /// </summary>
        /// <param name="userName"></param>
        /// <returns><see cref="UserDto"/></returns>
        Task<UserDto> GetByUserName(string userName);

        /// <summary>
        ///     Obtiene un usuario según su email
        /// </summary>
        /// <param name="email"></param>
        /// <returns><see cref="User"/></returns>
        Task<UserDto> GetByEmail(string email);

        /// <summary>
        ///     Obtiene un usuario según su tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns><see cref="User"/></returns>
        Task<UserDto> GetByTag(string tag);

        /// <summary>
        ///     Obtiene un usuario según su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="User"/></returns>
        Task<UserDto> GetById(int id);

        /// <summary>
        ///     Elimina el usuario con el id pasado como parámetro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Remove(int id);

        /// <summary>
        ///     Actualiza los datos del usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="ioTUser"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IdentityResult> Update(int userId, CreateUserDto userDto);

        /// <summary>
        ///     Cambia el idioma al usuario pasado como parámetro
        /// </summary>
        /// <param name="changeLanguage"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ChangeLanguage(ChangeLanguageDto changeLanguage, int userId);

        /// <summary>
        ///     Obtiene el rol del usuario con el id pasado como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="RoleDto"/></returns>
        Task<RoleDto> GetRoleByUserId(int userId);

        /// <summary>
        ///     Método que envía un email
        /// </summary>
        /// <param name="username">el nombre del correo</param>
        /// <param name="domain">el dominio del correo (ej. 'gmail')</param>
        /// <param name="tld">el final del correo (ej. '.com')</param>
        /// <returns></returns>
        void SendMail(string username, string domain, string tld);

        /// <summary>
        ///     Obtiene todos los usuarios con Admin como rol.
        /// </summary>
        /// <returns>Una lista de <see cref="UserDto"/> con todos los admins</returns>
        Task<List<UserDto>> GetAdmins();

        /// <summary>
        ///     Obtiene todos los usuarios con User como rol.
        /// </summary>
        /// <returns>Una lista de <see cref="UserDto"/> con todos los usuarios</returns>
        Task<List<UserDto>> GetUsers();

        /// <summary>
        ///     Restablece la contraseña de un usuario
        /// </summary>
        /// <param name="resetPassword"><see cref="ResetPasswordDto"/> con los datos de restablecimiento de contraseña</param>
        /// <returns></returns>
        Task<User> ResetPassword(ResetPasswordDto resetPass);

        /// <summary>
        ///     Valida la creación de un usuario
        /// </summary>
        /// <param name="ioTUser"><see cref="CreateUserDto"/> con los datos de creación de usuario</param>
        /// <returns>Lista de errores</returns>
        Task<List<string>> ValidateUser(CreateUserDto user);

        /// <summary>
        ///     Gestiona el seguir a un usuario
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> FollowUser(FollowDto follow);

        /// <summary>
        ///     Gestiona el bloquear a un usuario
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> BlockUser(BlockDto block);

        /// <summary>
        ///     Gestiona el dejar de seguir a un usuario
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> UnfollowUser(FollowDto follow);

        /// <summary>
        ///     Gestiona el desbloquear a un usuario
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> UnblockUser(BlockDto block);

        /// <summary>
        ///     Comprueba si un usuario sigue a otro
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        Task<bool> IsFollowing(FollowDto follow);

        /// <summary>
        ///     Comprueba si un usuario tiene bloqueado a otro
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        Task<bool> IsBlocked(BlockDto block);

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserDto>> GetFollowers(int userId);

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserDto>> GetFollowers(int userId, int page, int pageSize=10);

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserDto>> GetFollowing(int userId);

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserDto>> GetFollowing(int userId, int page, int pageSize = 10);

        /// <summary>
        ///     Obtiene los usuarios con rol User filtrados.
        /// </summary>
        /// <returns></returns>
        Task<List<UserDto>> GetUsersFilter(string searchStringint,int page, int pageSize = 10);

        /// <summary>
        ///     Obtiene los ids de los usuarios que sigue un usuario cuyo id se pasa como parámetro.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<int>> GetFollowingUserIdsCached(int userId);

        /// <summary>
        ///     Actualiza el avatar de un usuario cuyo id se pasa como parámetro.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarUrl"></param>
        /// <returns></returns>
        Task<bool> UpdateAvatar(int userId, string avatarUrl);

        /// <summary>
        ///     Obtiene 3 usuarios aleatorios de entre los 30 con más seguidores
        /// </summary>
        /// <returns></returns>
        Task<List<UserDto>> GetTopFollowed(int userId, int sampleSize = 3, int topLimit = 30);
    }
    public sealed class UsersService : BaseService, IUsersService
    {
        #region Miembros privados

        private IdentitiesService _identitiesService;
        private readonly IRedisCacheService _redisCacheService;

        #endregion

        #region Constructores

        public UsersService(JuaniteUnitOfWork juaniteUnitOfWork, ILogger logger, IIdentitiesService identitiesService, IRedisCacheService redisCacheService) : base(juaniteUnitOfWork, logger)
        {
            _identitiesService = (IdentitiesService)identitiesService;
            _redisCacheService = redisCacheService;
        }

        #endregion

        #region Implementación IUsersService

        /// <summary>
        ///     Actualiza el avatar de un usuario cuyo id se pasa como parámetro.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarUrl"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAvatar(int userId, string avatarUrl)
        {
            try
            {
                var user = await _unitOfWork.UsersRepository.Get(userId);
                if (user != null)
                {
                    user.Avatar = avatarUrl;
                    _unitOfWork.UsersRepository.Update(user);
                    await _unitOfWork.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.UpdateUserAvatar => ");
                return false;
            }
        }

        /// <summary>
        ///     Obtiene los usuarios con rol User filtrados.
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserDto>> GetUsersFilter(string searchString, int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var query = _unitOfWork.UsersRepository
                    .GetAll(user => user.Role == "User");

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    query = query.Where(u =>
                        u.UserName.Contains(searchString) ||
                        u.Tag.Contains(searchString));
                }

                var users = query
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var result = users.Select(u => Extensions.ConvertModel(u, new UserDto())).ToList();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Users Filter => ");
                throw;
            }
        }

        /// <summary>
        ///     Cambio de idioma al usuario pasado como parámetro
        /// </summary>
        /// <param name="changeLanguage"><see cref="ChangeLanguageDto"/> con los datos del nuevo idioma</param>
        /// <param name="userId">El id del usuario</param>
        /// <returns></returns>
        public async Task<bool> ChangeLanguage(ChangeLanguageDto changeLanguage, int userId)
        {
            try
            {
                var userDb = await _unitOfWork.UsersRepository.Get(userId);
                if (userDb != null)
                {
                    userDb.Language = changeLanguage.LanguageId;
                    _unitOfWork.UsersRepository.Update(userDb);
                    await _unitOfWork.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.ChangeLanguage => ");
                throw;
            }
        }

        /// <summary>
        ///     Crea un token para un usuario y un propósito específicos
        /// </summary>
        /// <param name="userId">El id del usuario</param>
        /// <param name="purpose">El propósito</param>
        /// <returns>Token</returns>
        public async Task<string> CreatePurposeToken(int userId, string purpose)
        {
            try
            {
                var user = await _unitOfWork.UsersRepository.Get(userId);
                return await _identitiesService.GetPurposeToken(user, purpose);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        ///     Crea un token para reestablecer la contraseña
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns>Token</returns>
        public async Task<string> CreateTokenPassword(int userId)
        {
            try
            {
                var user = await _unitOfWork.UsersRepository.Get(userId);
                return await _identitiesService.GetTokenPassword(user);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los usuarios
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserDto>> GetAll(int page, int pageSize=10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var users = _unitOfWork.UsersRepository.GetAll().Skip(skip).Take(pageSize);
                var result = users.Select(u => Extensions.ConvertModel(u, new UserDto())).ToList();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetAll => ");
                throw;
            }
        }

        public async Task<List<UserDto>> GetTopFollowed(int userId, int sampleSize = 3, int topLimit = 30)
        {
            // 1) Carga todos los follows
            var allFollows = await _unitOfWork.FollowsRepository
                .GetAll()
                .ToListAsync();

            // 2) Usuarios que el current user ya sigue
            var alreadyFollowingIds = allFollows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.UserId)
                .ToHashSet();

            // 3) Trae *todos* los usuarios excepto self y exceptuando ya-following
            var candidateUsers = await _unitOfWork.UsersRepository
                .GetAll(u =>
                    u.Id != userId &&
                    u.Id != 1 &&
                    !alreadyFollowingIds.Contains(u.Id))
                .ToListAsync();

            // 4) Construye un diccionario de counts: userId → número de followers
            var followCounts = allFollows
                .GroupBy(f => f.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            // 5) Proyecta cada candidato con su count (0 si no está en followCounts)
            var ranked = candidateUsers
                .Select(u => new
                {
                    User = u,
                    Count = followCounts.TryGetValue(u.Id, out var c) ? c : 0
                })
                // 6) Ordena descendentemente y toma hasta topLimit
                .OrderByDescending(x => x.Count)
                .Take(topLimit)
                .ToList();

            // 7) Mapea a DTOs
            var userDtos = ranked
                .Select(x => Extensions.ConvertModel(x.User, new UserDto()))
                .ToList();

            // 8) Shuffle Fisher–Yates + Take sampleSize
            var rnd = new Random();
            var list = userDtos.ToList();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }

            return list.Take(sampleSize).ToList();
        }

        /// <summary>
        ///     Obtiene el usuario según el email
        /// </summary>
        /// <param name="email">El email</param>
        /// <returns><see cref="UserDto"/> con los datos del usuario</returns>
        public async Task<UserDto> GetByEmail(string email)
        {
            try
            {
                var user = await Task.FromResult(_unitOfWork.UsersRepository.GetFirst(g => g.Email.Equals(email)));
                return Extensions.ConvertModel(user, new UserDto());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetByEmail =>");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el usuario según el tag
        /// </summary>
        /// <param name="tag">El tag</param>
        /// <returns><see cref="UserDto"/> con los datos del usuario</returns>
        public async Task<UserDto> GetByTag(string tag)
        {
            try
            {
                var user = _unitOfWork.UsersRepository.GetFirst(g => g.Tag.Equals(tag));
                if (user == null)
                {
                    return new UserDto();
                }
                return Extensions.ConvertModel(user, new UserDto());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetByTag =>");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el usuario según su id
        /// </summary>
        /// <param name="id">El id del usuario</param>
        /// <returns><see cref="UserDto"/> con los datos del usuario</returns>
        public async Task<UserDto> GetById(int id)
        {
            try
            {
                var user = await Task.FromResult(_unitOfWork.UsersRepository.GetFirst(g => g.Id.Equals(id)));
                return Extensions.ConvertModel(user, new UserDto());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetByUserName =>");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el usuario según su nombre de usuario
        /// </summary>
        /// <param name="userName">El nombre de usuario</param>
        /// <returns><see cref="UserDto"/> con los datos del usuario</returns>
        public async Task<UserDto> GetByUserName(string userName)
        {
            try
            {
                var user = _unitOfWork.UsersRepository.GetFirst(g => g.UserName.Equals(userName));
                return Extensions.ConvertModel(user, new UserDto());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetByUserName =>");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el rol de un usuario según su id
        /// </summary>
        /// <param name="userId">El id del usuario</param>
        /// <returns><see cref="RoleDto"/> con los datos del rol</returns>
        public async Task<RoleDto> GetRoleByUserId(int userId)
        {
            try
            {
                var user = await _unitOfWork.UsersRepository.Get(userId);
                if (user != null)
                {
                    var roleName = _identitiesService.GetUserRoles(user).Result.FirstOrDefault();
                    var roleDb = _unitOfWork.RolesRepository.GetFirst(g => g.Name == roleName);

                    return new RoleDto()
                    {
                        Id = Convert.ToInt32(roleDb.Id),
                        Name = roleDb.Name
                    };
                }
                return new RoleDto();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetRoleByUserId => ");
                throw;
            }
        }

        /// <summary>
        ///     Realización del login de la aplicación
        /// </summary>
        /// <param name="loginDto"><see cref="LoginDto"/> con los datos de inicio de sesión</param>
        /// <param name="rememberUser"></param>
        /// <returns></returns>
        public async Task<bool> Login(LoginDto loginDto, bool? rememberUser = false)
        {
            try
            {
                User userDb = _unitOfWork.UsersRepository.GetFirst(u => u.Email.Equals(loginDto.Email));
                if (userDb != null && userDb != new User())
                {

                    var login = await _identitiesService.Login(userDb, loginDto.Password, rememberUser.Value);
                    if (login)
                    {
                        await _unitOfWork.SaveChanges();
                    }
                    return login;
                }
                throw new UserNotFoundException();
            }
            catch (UserLockedException)
            {
                throw;
            }
            catch (UserSessionNotValidException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (PasswordNotValidException)
            {
                throw;
            }
            catch (UserWithoutPermissionException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.Login");
                return false;
            }
        }

        /// <summary>
        ///     Elimina a un usuario con el id pasado como parámetro
        /// </summary>
        /// <param name="id">El id del usuario</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Remove(int id)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var user = await GetById(id);

                if (user != null)
                {
                    await _unitOfWork.UsersRepository.Remove(id);
                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    response.Errors = new List<string> { String.Format(Translation_UsersRoles.ID_no_found_description, id) };
                }
                response.Id = id;
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.Remove => ");
                throw;
            }
        }

        /// <summary>
        ///     Actualiza los datos de un usuario en la base de datos
        /// </summary>
        /// <param name="ioTUser"><see cref="CreateUserDto"/> con los nuevos datos de usuario</param>
        /// <param name="userId">El id del usuario</param>
        /// <returns>Una lista de errores</returns>
        public async Task<IdentityResult> Update(int userId, CreateUserDto userDto)
        {
            User user = await _unitOfWork.UsersRepository.Get(userId);
            if (user == null)
            {
                return IdentityResult.Failed();
            }
            user.FullName = userDto.FullName;
            user.Bio = userDto.Bio;
            user.Link = userDto.Link;
            user.Avatar = userDto.Avatar;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;
            user.UserName = userDto.UserName;
            user.Tag = userDto.Tag;
            user.Country = userDto.Country;


            _unitOfWork.UsersRepository.Update(user);
            await _unitOfWork.SaveChanges();
            return IdentityResult.Success;
        }

        /// <summary>
        ///     Valida un token para un usuario y un propósito específicos
        /// </summary>
        /// <param name="userId">El id del usuario</param>
        /// <param name="purpose">El propósito</param>
        /// <param name="token">El token</param>
        /// <returns></returns>
        public async Task<bool> ValidateUserToken(int userId, string purpose, string token)
        {
            try
            {
                var user = await _unitOfWork.UsersRepository.Get(userId);
                return await _identitiesService.VerifyUserToken(user, purpose, token);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        ///     Envía un email
        /// </summary>
        /// <param name="email">el email destino</param>
        /// <param name="link">el enlace de restablecer contraseña</param>
        /// <returns></returns>
        public void SendMail(string username, string domain, string tld)
        {
            try
            {
                var email = string.Concat(username, "@", domain, ".", tld);
                var user = _unitOfWork.UsersRepository.GetFirst(u => u.Email == email);
                string hashedEmail = Hash(email);
                if (user != null)
                {
                    var link = string.Concat(Literals.Link_Recover, hashedEmail, "/", username, "/", domain, "/", tld);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(Literals.Email_Name, Literals.Email_Address));
                    message.To.Add(new MailboxAddress("", email));
                    message.Subject = Translation_Account.Email_title;
                    message.Body = new TextPart("plain") { Text = string.Concat(Translation_Account.Email_body, "\n", link) };

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect(Literals.Email_Service, Literals.Email_Port, SecureSocketOptions.StartTls);
                        client.Authenticate(Literals.Email_Address, Literals.Email_Auth);
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Send Mail => ");
            }
        }

        /// <summary>
        ///     Obtiene todos los usuarios con Admin como rol.
        /// </summary>
        /// <returns>Una lista de <see cref="UserDto"/> con todos los admins</returns>
        public async Task<List<UserDto>> GetAdmins()
        {
            try
            {
                var users = await _unitOfWork.UsersRepository.GetAll(user => user.Role == "Admin").ToListAsync();
                var result = users.Select(u => Extensions.ConvertModel(u, new UserDto())).ToList();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Admins => ");
                throw;
            }
        }


        /// <summary>
        ///     Obtiene todos los usuarios con User como rol.
        /// </summary>
        /// <returns>Una lista de <see cref="UserDto"/> con todos los usuarios</returns>
        public async Task<List<UserDto>> GetUsers()
        {
            try
            {
                var users = await _unitOfWork.UsersRepository.GetAll(user => user.Role == "User").ToListAsync();
                var result = users.Select(u => Extensions.ConvertModel(u, new UserDto())).ToList();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Users => ");
                throw;
            }
        }

        /// <summary>
        ///     Restablece la contraseña de un usuario
        /// </summary>
        /// <param name="resetPassword"><see cref="ResetPasswordDto"/> con los datos de restablecimiento de contraseña</param>
        /// <returns></returns>
        public async Task<User> ResetPassword(ResetPasswordDto resetPass)
        {
            try
            {
                var email = string.Concat(resetPass.Username, "@", resetPass.Domain, ".", resetPass.Tld);
                return _unitOfWork.UsersRepository.GetFirst(u => u.Email == email);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Reset password => ");
                throw;
            }
        }


        /// <summary>
        ///     Valida la creación de un usuario
        /// </summary>
        /// <param name="ioTUser"><see cref="CreateUserDto"/> con los datos de creación de usuario</param>
        /// <returns>Lista de errores</returns>
        public async Task<List<string>> ValidateUser(CreateUserDto user)
        {
            List<string> errorMessages = new List<string>();

            //Verificar nombre de usuario único
            var userNameUser = await _unitOfWork.UsersRepository.Any(a => a.Tag == user.Tag);
            if (userNameUser)
            {
                errorMessages.Add(string.Format(Translation_UsersRoles.NotAvailable_Username, user.UserName));
            }

            //Verificar email único
            var emailUser = await _unitOfWork.UsersRepository.Any(a => a.Email == user.Email);
            if (emailUser)
            {
                errorMessages.Add(string.Format(Translation_UsersRoles.NotAvailable_Email, user.Email));
            }

            return errorMessages;
        }

        /// <summary>
        ///     Gestiona el seguir a un usuario
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> FollowUser(FollowDto follow)
        {
            var response = new CreateEditRemoveResponseDto();
            try
            {
                var userDb = await _unitOfWork.UsersRepository.Get(follow.UserId);
                if (userDb != null)
                {
                    userDb = await _unitOfWork.UsersRepository.Get(follow.FollowerId);
                    if (userDb != null)
                    {
                        var followData = new Follow(follow.UserId, follow.FollowerId);
                        if(_unitOfWork.FollowsRepository.Add(followData) != null)
                        {
                            await _unitOfWork.SaveChanges();
                            response.IsSuccess(followData.UserId);
                        }
                    }
                    else
                    {
                        response.Id = follow.UserId;
                        response.Errors = new List<string> { Translation_Errors.Error_user_update };
                    }
                }
                else
                {
                    response.Id = follow.UserId;
                    response.Errors = new List<string> { Translation_Errors.Error_user_update };
                }

                if (response.Success)
                {
                    var key = $"{Literals.Redis_Users_Following}{follow.FollowerId}";
                    var followingIds = await _redisCacheService.GetAsync<List<int>>(key);
                    if (followingIds == null)
                        followingIds = await GetFollowingUserIdsCached(follow.FollowerId);

                    if (followingIds.Contains(follow.UserId))
                    {
                        await _redisCacheService.SetAsync(key, followingIds, TimeSpan.FromMinutes(60));
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.FollowUser => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona el bloquear a un usuario
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> BlockUser(BlockDto block)
        {
            var response = new CreateEditRemoveResponseDto();
            try
            {
                var userDb = await _unitOfWork.UsersRepository.Get(block.UserId);
                if (userDb != null)
                {
                    userDb = await _unitOfWork.UsersRepository.Get(block.BlockedUserId);
                    if (userDb != null)
                    {
                        var blockData = new Block(block.UserId, block.BlockedUserId);
                        if (_unitOfWork.BlocksRepository.Add(blockData) != null)
                        {
                            await _unitOfWork.SaveChanges();
                            response.IsSuccess(blockData.UserId);
                        }
                    }
                    else
                    {
                        response.Id = block.UserId;
                        response.Errors = new List<string> { Translation_Errors.Error_user_update };
                    }
                }
                else
                {
                    response.Id = block.UserId;
                    response.Errors = new List<string> { Translation_Errors.Error_user_update };
                }

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.BlockUser => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona el dejar de seguir a un usuario
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> UnfollowUser(FollowDto follow)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var user = await GetById(follow.UserId);

                if (user != null)
                {
                    user = await GetById(follow.FollowerId);
                    if (user != null)
                    {
                        await _unitOfWork.FollowsRepository.Remove([follow.UserId, follow.FollowerId]);
                        await _unitOfWork.SaveChanges();
                    }
                }
                else
                {
                    response.Errors = new List<string> { String.Format(Translation_UsersRoles.ID_no_found_description, follow.UserId) };
                }
                response.Id = follow.UserId;

                if (response.Success)
                {
                    var key = $"{Literals.Redis_Users_Following}{follow.FollowerId}";
                    var followingIds = await _redisCacheService.GetAsync<List<int>>(key);
                    if (followingIds == null)
                        followingIds = await GetFollowingUserIdsCached(follow.FollowerId);

                    if (followingIds.Remove(follow.UserId))
                    {
                        await _redisCacheService.SetAsync(key, followingIds, TimeSpan.FromMinutes(60));
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.UnfollowUser => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona el desbloquear a un usuario
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> UnblockUser(BlockDto block)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var user = await GetById(block.UserId);

                if (user != null)
                {
                    user = await GetById(block.BlockedUserId);
                    if (user != null)
                    {
                        await _unitOfWork.BlocksRepository.Remove([block.UserId, block.BlockedUserId]);
                        await _unitOfWork.SaveChanges();
                    }
                }
                else
                {
                    response.Errors = new List<string> { String.Format(Translation_UsersRoles.ID_no_found_description, block.UserId) };
                }
                response.Id = block.UserId;
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.UnBlockUser => ");
                throw;
            }
        }

        /// <summary>
        ///     Comprueba si un usuario sigue a otro
        /// </summary>
        /// <param name="follow"><see cref="FollowDto"/> con los datos del follow</param>
        /// <returns></returns>
        public async Task<bool> IsFollowing(FollowDto follow)
        {
            try
            {
                if (await _unitOfWork.FollowsRepository.Get([follow.UserId, follow.FollowerId]) != null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.IsFollowing => ");
                throw;
            }
        }

        /// <summary>
        ///     Comprueba si un usuario tiene bloqueado a otro
        /// </summary>
        /// <param name="block"><see cref="BlockDto"/> con los datos del bloqueo</param>
        /// <returns></returns>
        public async Task<bool> IsBlocked(BlockDto block)
        {
            try
            {
                if (await _unitOfWork.BlocksRepository.Get([block.UserId, block.BlockedUserId]) != null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.IsBlocked => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserDto>> GetFollowing(int userId)
        {
            try
            {
                // 1) Obtén todas las filas donde yo (userId) soy "follower"
                var follows = await _unitOfWork.FollowsRepository
                                   .GetAll(f => f.FollowerId == userId)
                                   .ToListAsync();

                // 2) Extrae los IDs de quienes sigo: f.UserId
                List<int> followingIds = follows
                                          .Select(f => f.UserId)
                                          .Distinct()
                                          .ToList();

                // 3) Trae esos usuarios de UsersRepository
                var users = await _unitOfWork.UsersRepository
                                 .GetAll(u => followingIds.Contains(u.Id))
                                 .ToListAsync();

                return users.Select(u => Extensions.ConvertModel(u, new UserDto()))
                            .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetFollowers => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserDto>> GetFollowing(int userId, int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                // 1) Obtén todas las filas donde yo (userId) soy "follower"
                var follows = await _unitOfWork.FollowsRepository
                                   .GetAll(f => f.FollowerId == userId)
                                   .Skip(skip)
                                   .Take(pageSize)
                                   .ToListAsync();

                // 2) Extrae los IDs de quienes sigo: f.UserId
                List<int> followingIds = follows
                                          .Select(f => f.UserId)
                                          .Distinct()
                                          .ToList();

                // 3) Trae esos usuarios de UsersRepository
                var users = await _unitOfWork.UsersRepository
                                 .GetAll(u => followingIds.Contains(u.Id))
                                 .ToListAsync();

                return users.Select(u => Extensions.ConvertModel(u, new UserDto()))
                            .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetFollowers => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserDto>> GetFollowers(int userId)
        {
            try
            {

                // 1) Obtén todas las filas donde ‘yo’ (userId) soy “el seguido”
                var follows = await _unitOfWork.FollowsRepository
                                   .GetAll(f => f.UserId == userId)
                                   .ToListAsync();

                // 2) Extrae los IDs de mis seguidores: f.FollowerId
                List<int> followerIds = follows
                                          .Select(f => f.FollowerId)
                                          .Distinct()
                                          .ToList();

                // 3) Trae esos usuarios de UsersRepository
                var users = await _unitOfWork.UsersRepository
                                 .GetAll(u => followerIds.Contains(u.Id))
                                 .ToListAsync();

                return users.Select(u => Extensions.ConvertModel(u, new UserDto()))
                            .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetFollowing => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los seguidores de un user cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserDto>> GetFollowers(int userId, int page, int pageSize = 10)
        {
            try
            {

                var skip = (page - 1) * pageSize;

                // 1) Obtén todas las filas donde ‘yo’ (userId) soy “el seguido”
                var follows = await _unitOfWork.FollowsRepository
                                   .GetAll(f => f.UserId == userId)
                                   .Skip(skip)
                                   .Take(pageSize)
                                   .ToListAsync();

                // 2) Extrae los IDs de mis seguidores: f.FollowerId
                List<int> followerIds = follows
                                          .Select(f => f.FollowerId)
                                          .Distinct()
                                          .ToList();

                // 3) Trae esos usuarios de UsersRepository
                var users = await _unitOfWork.UsersRepository
                                 .GetAll(u => followerIds.Contains(u.Id))
                                 .ToListAsync();

                return users.Select(u => Extensions.ConvertModel(u, new UserDto()))
                            .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UsersService.GetFollowing => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los ids de los usuarios que sigue un usuario, almacenados en caché
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetFollowingUserIdsCached(int userId)
        {
            try
            {
                return await _redisCacheService.GetOrCreateAsync(
                    $"{Literals.Redis_Users_Following}{userId}",
                    async () =>
                    {
                        var following = await GetFollowing(userId);
                        return following.Select(f => f.Id).ToList();
                    },
                    TimeSpan.FromMinutes(60)
                ) ?? new List<int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Error obteniendo usuarios seguidos para {userId}: {ex.Message}");
                _logger.LogError(ex, $"[Redis] Error obteniendo usuarios seguidos para {userId}");
                return new List<int>(); // No interrumpas el login
            }
        }

        #endregion

        #region Excepciones particulares del servicio

        public class UserApiException : Exception { }
        public class UserNotFoundException : Exception { }
        public class PasswordNotValidException : Exception { }
        public class UserLockedException : Exception { }
        public class UserWithoutVerificationException : Exception { }
        public class UserSessionNotValidException : Exception { }
        public class UserWithoutPermissionException : Exception { }

        #endregion

        #region Métodos privados

        /// <summary>
        ///     Hashea un texto
        /// </summary>
        /// <param name="text">el texto a hashear</param>
        /// <returns></returns>
        public static string Hash(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        #endregion
    }
}
