using FireBreath.PostsMicroservice.Models.Context;
using FireBreath.PostsMicroservice.Models.Entities;
using FireBreath.PostsMicroservice.Models.Repositories;

namespace FireBreath.PostsMicroservice.Models.UnitsOfWork
{
    public sealed class JuaniteUnitOfWork
    {
        #region Miembros privados

        /// <summary>
        ///     Contexto de acceso a la base de datos
        /// </summary>
        private readonly PostsDbContext _context;

        /// <summary>
        ///     Logger de la aplicación
        /// </summary>
        private readonly ILogger _logger;

        #region Repositorios

        /// <summary>
        ///     Repositorio de posts
        /// </summary>
        private JuaniteRepository<Post> _postsRepository;

        /// <summary>
        ///     Repositorio de mensajes
        /// </summary>

        private JuaniteRepository<Message> _messagesRepository;

        /// <summary>
        ///     Repositorio de archivos adjuntos
        /// </summary>
        private JuaniteRepository<Attachment> _attachmentsRepository;

        /// <summary>
        ///     Repositorio de likes
        /// </summary>
        private JuaniteRepository<Like> _likesRepository;

        /// <summary>
        ///     Repositorio de shares
        /// </summary>
        private JuaniteRepository<Share> _sharesRepository;

        #endregion

        #endregion

        #region Propiedades públicas

        /// <summary>
        ///     Repositorio de posts
        /// </summary>
        public JuaniteRepository<Post> PostsRepository => _postsRepository ?? (_postsRepository = new JuaniteRepository<Post>(_context, _logger));

        /// <summary>
        ///     Repositorio de mensajes
        /// </summary>
        public JuaniteRepository<Message> MessagesRepository => _messagesRepository ?? (_messagesRepository = new JuaniteRepository<Message>(_context, _logger));

        /// <summary>
        ///     Repositorio de archivos adjuntos
        /// </summary>
        public JuaniteRepository<Attachment> AttachmentsRepository => _attachmentsRepository ?? (_attachmentsRepository = new JuaniteRepository<Attachment>(_context, _logger));

        /// <summary>
        ///     Repositorio de likes
        /// </summary>
        public JuaniteRepository<Like> LikesRepository => _likesRepository ?? (_likesRepository = new JuaniteRepository<Like>(_context, _logger));

        /// <summary>
        ///     Repositorio de shares
        /// </summary>
        public JuaniteRepository<Share> SharesRepository => _sharesRepository ?? (_sharesRepository = new JuaniteRepository<Share>(_context, _logger));

        #endregion

        #region Constructores

        /// <summary>
        ///     Constructor por defecto
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="context"><see cref="PostsDbContext"/></param>
        public JuaniteUnitOfWork(ILogger logger, PostsDbContext context)
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

        #endregion
    }
}
