using EasyWeb.TicketsMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyWeb.TicketsMicroservice.Controllers
{
    public class BaseController : ControllerBase
    {
        #region Miembros privados

        public readonly IServiceProvider _serviceCollection;

        #endregion

        #region Miembros internos

        internal ILogger JuaniteLogger => (ILogger)_serviceCollection.GetService(typeof(ILogger));
        internal IConfiguration Configuration => (IConfiguration)_serviceCollection.GetService(typeof(IConfiguration));
        internal ITicketsService JuaniteServiceTickets => (ITicketsService)_serviceCollection.GetService(typeof(ITicketsService));
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
