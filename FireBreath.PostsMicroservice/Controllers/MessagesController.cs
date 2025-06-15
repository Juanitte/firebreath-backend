using Common.Dtos;
using Common.Utilities;
using FireBreath.PostsMicroservice.Models.Dtos.CreateDto;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using Microsoft.AspNetCore.Mvc;

namespace FireBreath.PostsMicroservice.Controllers
{
    [ApiController]
    public class MessagesController : BaseController
    {
        #region Miembros privados

        private readonly IWebHostEnvironment _hostingEnvironment;

        #endregion

        #region Constructores

        public MessagesController(IServiceProvider serviceCollection, IWebHostEnvironment hostingEnvironment) : base(serviceCollection)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        ///     Método que obtiene todos los mensajes
        /// </summary>
        /// <returns></returns>
        [HttpGet("messages/getall")]
        public async Task<JsonResult> GetAll()
        {
            try
            {
                var messages = await JuaniteServiceMessages.GetAll();
                return new JsonResult(messages);
            }
            catch (Exception e)
            {
                return new JsonResult(new List<CreateMessageDto>());
            }
        }

        /// <summary>
        ///     Método que obtiene un mensaje según su id
        /// </summary>
        /// <param name="id">El id del mensaje a buscar</param>
        /// <returns></returns>
        [HttpGet("messages/getbyid/{id}")]
        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                var message = await JuaniteServiceMessages.Get(id);
                return new JsonResult(message);
            }
            catch (Exception e)
            {
                return new JsonResult(new CreateMessageDto());
            }
        }

        /// <summary>
        ///     Método que crea un nuevo mensaje
        /// </summary>
        /// <param name="createMessage"><see cref="CreateMessageDto"/> con los datos del mensaje</param>
        /// <returns></returns>
        [HttpPost("messages/create")]
        public async Task<IActionResult> Create([FromForm] CreateMessageDto createMessage)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                response = await JuaniteServiceMessages.Create(createMessage);

                return Ok(true);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        ///     Método que actualiza un mensaje con id proporcionado como parámetro
        /// </summary>
        /// <param name="messageId">El id del mensaje a editar</param>
        /// <param name="newMessage"><see cref="MessageDto"/> con los nuevos datos del mensaje</param>
        /// <returns></returns>
        [HttpPost("messages/update")]
        public async Task<IActionResult> Update(MessageDto newMessage)
        {
            try
            {
                var result = await JuaniteServiceMessages.Update(newMessage);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }

        /// <summary>
        ///     Método que elimina un mensaje cuyo id se ha pasado como parámetro
        /// </summary>
        /// <param name="id">el id del mensaje a eliminar</param>
        /// <returns></returns>
        [HttpDelete("messages/remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await JuaniteServiceMessages.Remove(id);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Messages/Remove" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Messages/Remove" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Elimina los mensajes enviados por el usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del remitente</param>
        /// <returns></returns>
        [HttpDelete("messages/removebychat/{chatId}")]
        public async Task<IActionResult> RemoveByChat(int chatId)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await JuaniteServiceMessages.RemoveByChat(chatId);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Messages/RemoveByChat" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Messages/RemoveByChat" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Obtiene los mensajes enviados por el usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id de remitente</param>
        /// <returns>un <see cref="IEnumerable{T}"/> de <see cref="MessageDto"/> con los mensajes del usuario</returns>
        [HttpGet("messages/getbychat/{chatId}")]
        public async Task<ActionResult<IEnumerable<MessageDto?>>> GetByChat(int chatId)
        {
            try
            {
                return await JuaniteServiceMessages.GetByChat(chatId);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        ///     Obtiene los mensajes enviados por el usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id de remitente</param>
        /// <returns>un <see cref="IEnumerable{T}"/> de <see cref="MessageDto"/> con los mensajes del usuario</returns>
        [HttpGet("messages/getlastbychat/{chatId}")]
        public async Task<ActionResult<MessageDto?>> GetLastByChat(int chatId)
        {
            try
            {
                return await JuaniteServiceMessages.GetLastByChat(chatId);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost("chats/create")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDto createChat)
        {
            try
            {
                var response = await JuaniteServiceMessages.CreateChat(createChat);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("chats/getbyid/{id}")]
        public async Task<IActionResult> GetChatById(int id)
        {
            try
            {
                var chat = await JuaniteServiceMessages.GetChat(id);
                return Ok(chat);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("chats/getall")]
        public async Task<IActionResult> GetAllChats()
        {
            try
            {
                var chats = await JuaniteServiceMessages.GetAllChats();
                return Ok(chats);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost("chats/update")]
        public async Task<IActionResult> UpdateChat([FromBody] ChatDto chatDto)
        {
            try
            {
                var result = await JuaniteServiceMessages.UpdateChat(chatDto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpDelete("chats/remove/{id}")]
        public async Task<IActionResult> RemoveChat(int id)
        {
            try
            {
                var result = await JuaniteServiceMessages.RemoveChat(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("chats/getbyuser/{userId}")]
        public async Task<IActionResult> GetChatsByUser(int userId)
        {
            try
            {
                var chats = await JuaniteServiceMessages.GetChatsByUser(userId);
                return Ok(chats);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        #endregion
    }
}
