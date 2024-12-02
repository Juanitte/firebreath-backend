using Common.Dtos;
using Common.Utilities;
using FireBreath.PostsMicroservice.Models.Dtos.CreateDto;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using Microsoft.AspNetCore.Mvc;

namespace FireBreath.PostsMicroservice.Controllers
{
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
        /// <param name="newMessage"><see cref="CreateMessageDto"/> con los nuevos datos del mensaje</param>
        /// <returns></returns>
        [HttpPost("messages/update/{messageId}")]
        public async Task<IActionResult> Update(int messageId, CreateMessageDto newMessage)
        {
            try
            {
                var result = await JuaniteServiceMessages.Update(messageId, newMessage);
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
        [HttpDelete("messages/removebysender/{senderId}")]
        public async Task<IActionResult> RemoveBySender(int senderId)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await JuaniteServiceMessages.RemoveBySender(senderId);
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
        ///     Elimina los mensajes recibidos por el usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="receiverId">el id del receptor</param>
        /// <returns></returns>
        [HttpDelete("messages/removebyreceiver/{receiverId}")]
        public async Task<IActionResult> RemoveByReceiver(int receiverId)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await JuaniteServiceMessages.RemoveByReceiver(receiverId);
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
        ///     Obtiene los mensajes enviados por el usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id de remitente</param>
        /// <returns>un <see cref="IEnumerable{T}"/> de <see cref="MessageDto"/> con los mensajes del usuario</returns>
        [HttpGet("messages/getbysender/{senderId}")]
        public async Task<ActionResult<IEnumerable<MessageDto?>>> GetBySender(int senderId)
        {
            try
            {
                return await JuaniteServiceMessages.GetBySender(senderId);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        ///     Obtiene los mensajes recibidos por el usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="receiverId">el id de receptor</param>
        /// <returns>un <see cref="IEnumerable{T}"/> de <see cref="MessageDto"/> con los mensajes del usuario</returns>
        [HttpGet("messages/getbyreceiver/{receiverId}")]
        public async Task<ActionResult<IEnumerable<MessageDto?>>> GetByReceiver(int receiverId)
        {
            try
            {
                return await JuaniteServiceMessages.GetByReceiver(receiverId);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        ///     Descarga un archivo cuyo nombre se pasa como parámetro
        /// </summary>
        /// <param name="attachmentPath">el nombre del archivo</param>
        /// <returns></returns>
        [HttpGet("messages/download/{userId}/{attachmentPath}")]
        public IActionResult DownloadAttachment(string attachmentPath, int userId)
        {
            try
            {
                string directoryPath = Path.Combine("C:/ProyectoIoT/Back/ApiTest/AttachmentStorage/", userId.ToString());
                string filePath = Path.Combine(directoryPath, attachmentPath);

                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    string contentType = "application/octet-stream";

                    return File(fileBytes, contentType, attachmentPath);
                }
                else
                {
                    return NotFound("File not found");
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        #endregion
    }
}
