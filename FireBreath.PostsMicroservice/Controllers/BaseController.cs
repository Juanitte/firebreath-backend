using FireBreath.PostsMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace FireBreath.PostsMicroservice.Controllers
{
    public class BaseController : ControllerBase
    {
        #region Miembros privados

        public readonly IServiceProvider _serviceCollection;

        #endregion

        #region Miembros internos

        internal ILogger JuaniteLogger => (ILogger)_serviceCollection.GetService(typeof(ILogger));
        internal IConfiguration Configuration => (IConfiguration)_serviceCollection.GetService(typeof(IConfiguration));
        internal IPostsService JuaniteServicePosts => (IPostsService)_serviceCollection.GetService(typeof(IPostsService));
        internal IMessagesService JuaniteServiceMessages => (IMessagesService)_serviceCollection.GetService(typeof(IMessagesService));
        internal IAttachmentsService JuaniteServiceAttachments => (IAttachmentsService)_serviceCollection.GetService(typeof(IAttachmentsService));

        #endregion

        #region Constructores

        /// <summary>
        ///     Constructor base, almacena el service collection
        /// </summary>
        /// <param name="serviceCollection"></param>
        public BaseController(IServiceProvider serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        #endregion
    }
}
