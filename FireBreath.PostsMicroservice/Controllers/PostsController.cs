using Common.Dtos;
using Common.Utilities;
using FireBreath.PostsMicroservice.Models.Dtos.CreateDto;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using FireBreath.PostsMicroservice.Models.Dtos.RequestDto;
using FireBreath.PostsMicroservice.Models.Dtos.ResponseDto;
using FireBreath.PostsMicroservice.Models.Entities;
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
        public async Task<JsonResult> GetAllFilter([FromBody] PostFilterRequestDto filter)
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
        ///     Método que elimina un post cuyo id se ha pasado como parámetro
        /// </summary>
        /// <param name="id">el id del post a eliminar</param>
        /// <returns></returns>
        [HttpDelete("posts/remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await JuaniteServicePosts.Remove(id);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Posts/Remove" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Posts/Remove" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Obtiene los posts pertenecientes a un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="page">el numero de la pagina</param>
        /// <returns><see cref="JsonResult"/> con los datos de los posts</returns>
        [HttpGet("/posts/getbyuser/{userId}/{page}")]
        public async Task<JsonResult> GetByUser(int userId, int page)
        {
            try
            {
                var posts = await JuaniteServicePosts.GetByUser(userId, page);
                return new JsonResult(posts);
            }
            catch (Exception e)
            {
                return new JsonResult(new CreatePostDto());
            }
        }

        /// <summary>
        ///     Gestiona la acción de dar me gusta a un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpPost("/posts/like/{userId}/{postId}")]
        public async Task<IActionResult> LikePost(int userId, int postId)
        {
            try
            {
                var result = await JuaniteServicePosts.Like(userId, postId);

                return Ok(true);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        ///     Gestiona la acción de quitar me gusta a un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpDelete("posts/dislike/{userId}/{postId}")]
        public async Task<IActionResult> DislikePost(int userId, int postId)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await JuaniteServicePosts.Dislike(userId, postId);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Posts/DislikePost" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Posts/DislikePost" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Comprueba si un usuario ha dado me gusta a un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpGet("posts/postisliked/{userId}/{postId}")]
        public async Task<bool> PostIsLiked(int userId, int postId)
        {
            try
            {
                if (await JuaniteServicePosts.IsLiked(userId, postId))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Obtiene los post a los que le ha dado me gusta un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        [HttpGet("posts/getliked/{userId}")]
        public async Task<IActionResult> GetLiked(int userId)
        {
            try
            {
                var posts = await JuaniteServicePosts.GetLiked(userId);

                return new JsonResult(posts);
            }
            catch (Exception e)
            {
                return new JsonResult(new List<PostDto>());
            }
        }

        /// <summary>
        ///     Obtiene el id de los usuarios que han dado me gusta a un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpGet("posts/getlikers/{postId}")]
        public async Task<IActionResult> GetLikers(int postId)
        {
            try
            {
                var likers = await JuaniteServicePosts.GetLikers(postId);

                return new JsonResult(likers);
            }
            catch (Exception e)
            {
                return new JsonResult(new List<int>());
            }
        }

        /// <summary>
        ///     Gestiona la acción de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpPost("/posts/share/{userId}/{postId}")]
        public async Task<IActionResult> SharePost(int userId, int postId)
        {
            try
            {
                var result = await JuaniteServicePosts.Share(userId, postId);

                return Ok(true);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        ///     Gestiona la acción de dejar de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpDelete("posts/stopsharing/{userId}/{postId}")]
        public async Task<IActionResult> StopSharingPost(int userId, int postId)
        {
            var response = new GenericResponseDto();
            try
            {
                var result = await JuaniteServicePosts.StopSharing(userId, postId);
                if (result.Errors != null && result.Errors.Any())
                {
                    response.Error = new GenericErrorDto() { Id = ResponseCodes.DataError, Description = result.Errors.ToList().ToDisplayList(), Location = "Posts/StopSharingPost" };
                }
            }
            catch (Exception e)
            {
                response.Error = new GenericErrorDto() { Id = ResponseCodes.OtherError, Description = e.Message, Location = "Posts/StopSharingPost" };
            }
            return Ok(response);
        }

        /// <summary>
        ///     Comprueba si un usuario ha compartido un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpGet("posts/postisshared/{userId}/{postId}")]
        public async Task<bool> PostIsShared(int userId, int postId)
        {
            try
            {
                if (await JuaniteServicePosts.IsShared(userId, postId))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Obtiene los post que ha compartido un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        [HttpGet("posts/getshared/{userId}")]
        public async Task<IActionResult> GetShared(int userId)
        {
            try
            {
                var posts = await JuaniteServicePosts.GetShared(userId);

                return new JsonResult(posts);
            }
            catch (Exception e)
            {
                return new JsonResult(new List<PostDto>());
            }
        }

        /// <summary>
        ///     Obtiene los usuarios que han compartido un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpGet("posts/getsharers/{postId}")]
        public async Task<IActionResult> GetSharers(int postId)
        {
            try
            {
                var sharers = await JuaniteServicePosts.GetSharers(postId);

                return new JsonResult(sharers);
            }
            catch (Exception e)
            {
                return new JsonResult(new List<int>());
            }
        }

        /// <summary>
        ///     Obtiene los comentarios de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpGet("posts/getcomments/{postId}")]
        public async Task<IActionResult> GetComments(int postId)
        {
            try
            {
                var posts = await JuaniteServicePosts.GetComments(postId);

                return new JsonResult(posts);
            }
            catch (Exception e)
            {
                return new JsonResult(new List<PostDto>());
            }
        }

        /// <summary>
        ///     Obtiene el numero de comentarios de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpGet("posts/getcommentcount/{postId}")]
        public async Task<int> GetCommentCount(int postId)
        {
            try
            {
                var commentCount = await JuaniteServicePosts.GetCommentCount(postId);

                return commentCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Obtiene el numero de likes de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpGet("posts/getlikecount/{postId}")]
        public async Task<int> GetLikeCount(int postId)
        {
            try
            {
                var likeCount = await JuaniteServicePosts.GetLikeCount(postId);

                return likeCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Obtiene el numero de veces que un post cuyo id se pasa como parámetro se ha compartido
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        [HttpGet("posts/getsharecount/{postId}")]
        public async Task<int> GetShareCount(int postId)
        {
            try
            {
                var shareCount = await JuaniteServicePosts.GetShareCount(postId);

                return shareCount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        #endregion
    }
}
