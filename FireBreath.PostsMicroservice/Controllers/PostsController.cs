using Common.Dtos;
using Common.Utilities;
using FireBreath.PostsMicroservice.Models.Dtos.CreateDto;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using FireBreath.PostsMicroservice.Models.Dtos.RequestDto;
using FireBreath.PostsMicroservice.Models.Dtos.ResponseDto;
using Microsoft.AspNetCore.Mvc;

namespace FireBreath.PostsMicroservice.Controllers
{
    [ApiController]
    public class PostsController : BaseController
    {
        #region Miembros privados

        private readonly IWebHostEnvironment _hostingEnvironment;

        #endregion

        #region Constructores

        public PostsController(IServiceProvider serviceCollection, IWebHostEnvironment hostingEnvironment) : base(serviceCollection)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        ///     Método que obtiene todos los posts
        /// </summary>
        /// <returns></returns>
        [HttpGet("posts/getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var posts = await JuaniteServicePosts.GetAll();

                return new JsonResult(posts);
            }
            catch (Exception e)
            {
                return new JsonResult(new List<PostDto>());
            }
        }

        /// <summary>
        ///     Método que obtiene todos los posts filtrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("posts/getallfilter")]
        public async Task<JsonResult> GetAllFilter([FromQuery] PostFilterRequestDto filter)
        {
            try
            {
                var posts = await JuaniteServicePosts.GetAllFilter(filter);
                return new JsonResult(posts);
            }
            catch (Exception e)
            {
                return new JsonResult(new ResponseFilterPostDto());
            }
        }

        /// <summary>
        ///     Método que obtiene un post según su id
        /// </summary>
        /// <param name="id">El id del post a buscar</param>
        /// <returns></returns>
        [HttpGet("posts/getbyid/{id}")]
        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                var post = await JuaniteServicePosts.Get(id);
                return new JsonResult(post);
            }
            catch (Exception e)
            {
                return new JsonResult(new PostDto());
            }
        }

        /// <summary>
        ///     Método que crea un nuevo post
        /// </summary>
        /// <param name="createPost"><see cref="CreatePostDto"/> con los datos del post</param>
        /// <returns></returns>
        [HttpPost("posts/create")]
        public async Task<IActionResult> Create([FromForm] CreatePostDto createPost)
        {
            try
            {
                var result = await JuaniteServicePosts.Create(createPost);

                return Ok(true);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        ///     Método que actualiza un post con id proporcionado como parámetro
        /// </summary>
        /// <param name="postId">El id del post a editar</param>
        /// <param name="newPost"><see cref="CreatePostDto"/> con los nuevos datos del post</param>
        /// <returns></returns>
        [HttpPost("posts/update/{postId}")]
        public async Task<IActionResult> Update(int postId, [FromBody] CreatePostDto newPost)
        {
            try
            {
                var result = await JuaniteServicePosts.Update(postId, newPost);

                if (result != null)
                {
                    return Ok(true);
                }
                else
                {
                    return Problem(Translations.Translation_Posts.Error_update);
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        ///     Método que cambia la prioridad a la incidencia con id pasado como parámetro
        /// </summary>
        /// <param name="ticketId">El id de la incidencia a modificar</param>
        /// <param name="priority">el valor de la prioridad</param>
        /// <returns></returns>
        [HttpPost("tickets/changepriority/{ticketId}/{priority}")]
        public async Task<IActionResult> ChangePriority(int ticketId, int priority)
        {

            var result = await JuaniteServiceTickets.ChangePriority(ticketId, (Priorities)priority);
            if (result)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        /// <summary>
        ///     Método que cambia el estado a la incidencia con id pasado como parámetro
        /// </summary>
        /// <param name="ticketId">El id de la incidencia a modificar</param>
        /// <param name="status">el valor del estado</param>
        /// <returns></returns>
        [HttpPost("tickets/changestatus/{ticketId}/{status}")]
        public async Task<IActionResult> ChangeStatus(int ticketId, int status)
        {

            var result = await JuaniteServiceTickets.ChangeStatus(ticketId, (Status)status);
            if (result)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        /// <summary>
        ///     Método que asigna una incidencia con id pasado como parámetro a un usuario con id pasado como parámetro
        /// </summary>
        /// <param name="ticketId">El id de la incidencia a asignar</param>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        [HttpPost("tickets/asign/{ticketId}/{userId}")]
        public async Task<IActionResult> AsignTicket(int ticketId, int userId)
        {

            var result = await JuaniteServiceTickets.AsignTicket(ticketId, userId);
            if (result)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        /// <summary>
        ///     Método que elimina una incidencia cuyo id se ha pasado como parámetro
        /// </summary>
        /// <param name="id">el id de la incidencia a eliminar</param>
        /// <returns></returns>
        [HttpDelete("tickets/remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var response = new GenericResponseDto();
            try
            {
                await JuaniteServiceMessages.RemoveByTicket(id);
                var result = await JuaniteServiceTickets.Remove(id);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Tickets/Remove" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Tickets/Remove" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Obtiene las incidencias pertenecientes a un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns><see cref="JsonResult"/> con los datos de los tickets</returns>
        [HttpGet("/tickets/getbyuser/{userId}")]
        public async Task<JsonResult> GetByUser(int userId)
        {
            try
            {
                var tickets = JuaniteServiceTickets.GetByUser(userId);
                return new JsonResult(tickets);
            }
            catch (Exception e)
            {
                return new JsonResult(new CreateTicketDataDto());
            }
        }

        /// <summary>
        ///     Obtiene las incidencias pertenecientes a un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns><see cref="JsonResult"/> con los datos de los tickets</returns>
        [HttpGet("/tickets/getbyuserwithnames/{userId}")]
        public async Task<JsonResult> GetByUserWithNames(int userId)
        {
            try
            {
                var tickets = JuaniteServiceTickets.GetByUserWithNames(userId);
                return new JsonResult(tickets);
            }
            catch (Exception e)
            {
                return new JsonResult(new CreateTicketDataDto());
            }
        }

        #endregion
    }
}
