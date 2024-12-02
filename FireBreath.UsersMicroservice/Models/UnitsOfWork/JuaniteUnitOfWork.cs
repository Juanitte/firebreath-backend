using FireBreath.UsersMicroservice.Models.Context;
using FireBreath.UsersMicroservice.Models.Entities;
using FireBreath.UsersMicroservice.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FireBreath.UsersMicroservice.Models.UnitsOfWork
{
    public sealed class JuaniteUnitOfWork
    {
        #region Miembros privados

        /// <summary>
        ///     Contexto de acceso a la base de datos
        /// </summary>
        private readonly UsersDbContext _context;

        /// <summary>
        ///     Logger de la aplicación
        /// </summary>
        private readonly ILogger _logger;

        #region Repositorios

        /// <summary>
        ///     Repositorio de usuarios
        /// </summary>
        private JuaniteRepository<User> _usersRepository;

        /// <summary>
        ///     Repositorio de roles
        /// </summary>

        private JuaniteRepository<Role> _rolesRepository;

        /// <summary>
        ///     Repositorio de follows
        /// </summary>
        private JuaniteRepository<Follow> _followsRepository;

        /// <summary>
        ///     Repositorio de bloqueos
        /// </summary>
        private JuaniteRepository<Block> _blocksRepository;

        #endregion
        #endregion

        #region Propiedades públicas

        /// <summary>
        ///     Repositorio de usuarios
        /// </summary>
        public JuaniteRepository<User> UsersRepository => _usersRepository ?? (_usersRepository = new JuaniteRepository<User>(_context, _logger));

        /// <summary>
        ///     Repositorio de roles
        /// </summary>
        public JuaniteRepository<Role> RolesRepository => _rolesRepository ?? (_rolesRepository = new JuaniteRepository<Role>(_context, _logger));

        /// <summary>
        ///     Repositorio de follows
        /// </summary>
        public JuaniteRepository<Follow> FollowsRepository => _followsRepository ?? (_followsRepository = new JuaniteRepository<Follow>(_context, _logger));

        /// <summary>
        ///     Repositorio de bloqueos
        /// </summary>
        public JuaniteRepository<Block> BlocksRepository => _blocksRepository ?? (_blocksRepository = new JuaniteRepository<Block>(_context, _logger));

        #endregion

        #region Constructores

        /// <summary>
        ///     Constructor por defecto
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="context"><see cref="UsersDbContext"/></param>
        public JuaniteUnitOfWork(ILogger logger, UsersDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        ///     Guarda los cambios pendientes en los contextos de base de datos
        /// </summary>
        /// <returns></returns>
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }


        public void DetachLocal(User t, string entryId)
        {
            var local = _context.Set<User>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.Entry(t).State = EntityState.Modified;
        }

        #endregion
    }
}
