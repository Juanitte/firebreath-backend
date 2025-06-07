using Azure;
using Common.Services;
using Common.Utilities;
using FireBreath.PostsMicroservice.Models.Dtos.CreateDto;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using FireBreath.PostsMicroservice.Models.Dtos.RequestDto;
using FireBreath.PostsMicroservice.Models.Dtos.ResponseDto;
using FireBreath.PostsMicroservice.Models.Entities;
using FireBreath.PostsMicroservice.Models.UnitsOfWork;
using FireBreath.PostsMicroservice.Translations;
using FireBreath.PostsMicroservice.Utilities;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net.Http;
using Attachment = FireBreath.PostsMicroservice.Models.Entities.Attachment;

namespace FireBreath.PostsMicroservice.Services
{
    /// <summary>
    ///     Interfaz del servicio de posts
    /// </summary>
    public interface IPostsService
    {
        /// <summary>
        ///     Obtiene todos los posts
        /// </summary>
        /// <returns>una lista de posts <see cref="PostDto"/></returns>
        public Task<List<PostDto>> GetAll(int page, int pageSize=10);

        /// <summary>
        ///     Obtiene el post cuyo id se ha pasado como parámetro
        /// </summary>
        /// <param name="id">el id del post a buscar</param>
        /// <returns><see cref="PostDto"/> con los datos del post</returns>
        public Task<PostDto> Get(int id);

        /// <summary>
        ///     Crea un nuevo post
        /// </summary>
        /// <param name="createPost"><see cref="CreatePostDto"/> con los datos del post</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/> con los datos del post</returns>
        public Task<CreateEditRemoveResponseDto> Create(CreatePostDto createPost);

        /// <summary>
        ///     Actualiza los datos de un post
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <param name="newPost"><see cref="CreatePostDto"/> con los datos modificados del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> Update(int postId, CreatePostDto newPost);

        /// <summary>
        ///     Elimina el post cuyo id se ha pasado como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> Remove(int postId);

        /// <summary>
        ///     Obtiene los posts asignados al usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns>una lista con los posts asignados al usuario <see cref="Post"/></returns>
        public Task<IEnumerable<PostDto>> GetByUser(int userId, bool areComments, int page, int pageSize=10);
        
        /// <summary>
        ///     Obtiene los posts filtrados
        /// </summary>
        /// <returns></returns>
        Task<ResponseFilterPostDto> GetAllFilter(PostFilterRequestDto filter, int page, int pageSize = 10);
        
        /// <summary>
        ///     Envía un email
        /// </summary>
        /// <param name="email">el email destino</param>
        /// <param name="link">el enlace al post</param>
        public bool SendMail(string email, string link);

        /// <summary>
        ///     Gestiona la acción de dar me gusta a un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> Like(int userId, int postId);

        /// <summary>
        ///     Gestiona la acción de quitar me gusta a un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> Dislike(int userId, int postId);

        /// <summary>
        ///     Comprueba si un usuario ha dado me gusta a un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<bool> IsLiked(int userId, int postId);

        /// <summary>
        ///     Obtiene los post a los que le ha dado me gusta un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        public Task<List<PostDto>> GetLiked(int userId, int page, int pageSize = 10);

        /// <summary>
        ///     Obtiene los usuarios que han dado me gusta a un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<List<int>> GetLikers(int postId);

        /// <summary>
        ///     Gestiona la acción de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> Share(int userId, int postId);

        /// <summary>
        ///     Gestiona la acción de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> Save(int userId, int postId);

        /// <summary>
        ///     Gestiona la acción de dejar de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> StopSharing(int userId, int postId);

        /// <summary>
        ///     Gestiona la acción de dejar de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> StopSaving(int userId, int postId);

        /// <summary>
        ///     Comprueba si un usuario ha compartido un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<bool> IsShared(int userId, int postId);

        /// <summary>
        ///     Comprueba si un usuario ha compartido un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<bool> IsSaved(int userId, int postId);

        /// <summary>
        ///     Obtiene los post que ha compartido un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        public Task<List<PostDto>> GetShared(int userId, int page, int pageSize = 10);

        /// <summary>
        ///     Obtiene los post que ha compartido un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        public Task<List<PostDto>> GetSaved(int userId, int page, int pageSize = 10);

        /// <summary>
        ///     Obtiene los usuarios que han compartido un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<List<int>> GetSharers(int postId);

        /// <summary>
        ///     Obtiene los comentarios de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<List<PostDto>> GetComments(int postId, int page, int pageSize = 10);

        /// <summary>
        ///     Obtiene el numero de comentarios de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<int> GetCommentCount(int postId);

        /// <summary>
        ///     Obtiene el numero de likes de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<int> GetLikeCount(int postId);

        /// <summary>
        ///     Obtiene el numero de veces que un post cuyo id se pasa como parámetro se ha compartido
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<int> GetShareCount(int postId);

        /// <summary>
        ///     Obtiene el numero de veces que un post cuyo id se pasa como parámetro se ha compartido
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<int> GetSaveCount(int postId);

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos posts desde una fecha determinada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        Task<bool> HasNewPostsForUser(int userId, DateTime since);

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos posts de los usuarios que sigue desde una fecha determinada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        Task<bool> HasNewPostsFromFollowing(int userId, DateTime since);

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos comentarios desde una fecha determinada
        /// </summary>
        /// <param name="since"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HasNewComments(int userId, DateTime since);

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos Shares desde una fecha determinada
        /// </summary>
        /// <param name="since"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HasNewShares(int userId, DateTime since);

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos Saves desde una fecha determinada
        /// </summary>
        /// <param name="since"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HasNewSaves(int userId, DateTime since);

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos Likes desde una fecha determinada
        /// </summary>
        /// <param name="since"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HasNewLikes(int userId, DateTime since);

        /// <summary>
        /// Asynchronously retrieves the stream and MIME type of an attachment by its identifier.
        /// </summary>
        /// <remarks>The caller is responsible for disposing the returned <see cref="Stream"/> after
        /// use.</remarks>
        /// <param name="attachmentId">The unique identifier of the attachment to retrieve. Must be a positive integer.</param>
        /// <returns>A tuple containing the attachment's <see cref="Stream"/> and its MIME type as a <see cref="string"/>,  or
        /// <see langword="null"/> if the attachment does not exist.</returns>
        Task<(Stream Stream, string MimeType)?> GetAttachmentStreamAsync(int attachmentId);
    }
    public class PostsService : BaseService, IPostsService
    {

        #region Miembros privados

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRedisCacheService _redisCacheService;

        #endregion

        #region Constructores

        public PostsService(JuaniteUnitOfWork juaniteUnitOfWork, ILogger logger, IHttpClientFactory httpClientFactory, IRedisCacheService redisCacheService) : base(juaniteUnitOfWork, logger)
        {
            _httpClientFactory = httpClientFactory;
            _redisCacheService = redisCacheService;
        }

        #endregion

        #region Implementación de métodos de la interfaz

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos posts desde una fecha determinada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        public async Task<bool> HasNewPostsForUser(int userId, DateTime since)
        {
            return await _unitOfWork.PostsRepository.Any(p => p.UserId == userId && p.Created > since);
        }

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos posts de los usuarios que sigue desde una fecha determinada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        public async Task<bool> HasNewPostsFromFollowing(int userId, DateTime since)
        {
            var followedUserIds = await GetFollowedUserIdsAsync(userId);
            if (followedUserIds == null || !followedUserIds.Any()) return false;

            return await _unitOfWork.PostsRepository.Any(p =>
                followedUserIds.Contains(p.UserId) && p.Created > since);
        }

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos posts desde una fecha determinada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        public async Task<bool> HasNewComments(int userId, DateTime since)
        {
            return await _unitOfWork.PostsRepository.Any(p => p.UserId == userId && p.Created > since && p.PostId != 0);
        }

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos posts desde una fecha determinada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        public async Task<bool> HasNewShares(int userId, DateTime since)
        {
            return await _unitOfWork.SharesRepository.Any(s => s.UserId == userId && s.Timestamp > since);
        }

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos posts desde una fecha determinada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        public async Task<bool> HasNewSaves(int userId, DateTime since)
        {
            return await _unitOfWork.SavesRepository.Any(s => s.UserId == userId && s.Timestamp > since);
        }

        /// <summary>
        ///     Comprueba si un usuario tiene nuevos posts desde una fecha determinada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="since"></param>
        /// <returns></returns>
        public async Task<bool> HasNewLikes(int userId, DateTime since)
        {
            return await _unitOfWork.LikesRepository.Any(s => s.UserId == userId && s.Timestamp > since);
        }

        /// <summary>
        ///     Obtiene los comentarios de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<List<PostDto>> GetComments(int postId, int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var postsQuery = _unitOfWork.PostsRepository.GetAll().Where(p => p.PostId == postId).OrderByDescending(p => p.Created).Skip(skip).Take(pageSize);

                var posts = postsQuery.Select(p => p.ConvertModel(new PostDto())).ToList();

                List<PostDto> result = new List<PostDto>();
                foreach (var post in posts)
                {
                    result.Add(post);
                    var attachments = await _unitOfWork.AttachmentsRepository
                        .GetAll(attachment => attachment.PostId == post.Id)
                        .ToListAsync();
                    if (!attachments.IsNullOrEmpty())
                    {
                        foreach (var attachment in attachments)
                        {
                            // Creo el DTO básico
                            AttachmentDto attachmentDto = attachment.ConvertModel(new AttachmentDto());

                            // Si es imagen, cargo el base64
                            var ext = Path.GetExtension(attachment.Path).ToLower();
                            bool isVideo = ext == ".mp4" || ext == ".webm" || ext == ".ogg";
                            attachmentDto.IsVideo = isVideo;

                            if (!isVideo)
                            {
                                // Leer solo la imagen en base64
                                attachmentDto.File = Convert.ToBase64String(
                                    await File.ReadAllBytesAsync(attachment.Path));
                            }
                            else
                            {
                                // 1) Generar el thumbnail con FFmpeg en tiempo real
                                var thumbBytes = VideoThumbnailGenerator.GenerateThumbnail(attachment.Path, _logger);

                                if (thumbBytes != null && thumbBytes.Length > 0)
                                {
                                    // Codifico a Base64 y entrego un data URI
                                    attachmentDto.Thumbnail = $"data:image/png;base64,{Convert.ToBase64String(thumbBytes)}";
                                }
                                else
                                {
                                    // Si FFmpeg falló, puedes dejar null o una imagen "placeholder"
                                    attachmentDto.Thumbnail = null;
                                }
                                // → Para vídeo, no asignamos attachmentDto.File (queda null).
                                //    Podrías llenar attachmentDto.Thumbnail con un poster si lo tuvieras:
                                //    attachmentDto.Thumbnail = Convert.ToBase64String(File.ReadAllBytes(thumbnailPath));
                                attachmentDto.File = null;
                            }
                            result.Last().Attachments.Add(attachmentDto);
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetComments => ");
                Console.WriteLine($"Exception: {e.StackTrace}\nMessage: {e.Message}\nInnerException: {e.InnerException}");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los comentarios de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<int> GetCommentCount(int postId)
        {
            try
            {
                var posts = await _unitOfWork.PostsRepository.GetAll().Where(p => p.PostId == postId).ToListAsync();
                List<PostDto> result = new List<PostDto>();
                foreach (var post in posts)
                {
                    result.Add(post.ConvertModel(new PostDto()));
                }
                return result.Count;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetCommentCount => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los likes de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<int> GetLikeCount(int postId)
        {
            try
            {
                var likes = await _unitOfWork.LikesRepository.GetAll(l => l.PostId == postId).ToListAsync();
                return likes.Count;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetLikeCount => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene las veces que un post cuyo id se pasa como parámetro ha sido compartido
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<int> GetShareCount(int postId)
        {
            try
            {
                var shares = await _unitOfWork.SharesRepository.GetAll(l => l.PostId == postId).ToListAsync();
                return shares.Count;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetShareCount => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene las veces que un post cuyo id se pasa como parámetro ha sido compartido
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<int> GetSaveCount(int postId)
        {
            try
            {
                var shares = await _unitOfWork.SavesRepository.GetAll(l => l.PostId == postId).ToListAsync();
                return shares.Count;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetSaveCount => ");
                throw;
            }
        }

        /// <summary>
        ///     Crea un nuevo post
        /// </summary>
        /// <param name="createPost"><see cref="CreatePostDto"/> con los datos del post</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public async Task<CreateEditRemoveResponseDto> Create(CreatePostDto createPost)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var post = new Post(createPost.Author, createPost.AuthorTag, createPost.AuthorAvatar, createPost.Content, createPost.UserId, createPost.PostId);

                if (post != null)
                {

                    if (_unitOfWork.PostsRepository.Add(post) != null)
                    {
                        await _unitOfWork.SaveChanges();
                        response.IsSuccess(post.Id);


                        if (!createPost.Attachments.IsNullOrEmpty())
                        {
                            foreach (var attachment in createPost.Attachments)
                            {
                                Console.WriteLine(attachment);
                                if (attachment != null)
                                {
                                    string attachmentPath = await Utils.SaveAttachmentToFileSystem(attachment, post.Id, AttachmentContainerType.POST);
                                    Attachment newAttachment = new Attachment(attachmentPath, post.Id);

                                    _unitOfWork.AttachmentsRepository.Add(newAttachment);
                                }
                            }
                            await _unitOfWork.SaveChanges();
                        }
                    }
                }
                else
                {
                    response.Id = 0;
                    response.Errors = new List<string> { Translation_Posts.Error_create_post };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.Create => ");
                throw;
            }
        }

        /// <summary>
        ///     Elimina un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public async Task<CreateEditRemoveResponseDto> Remove(int postId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var post = _unitOfWork.PostsRepository.GetFirst(g => g.Id == postId);

                if (post != null)
                {
                    await _unitOfWork.PostsRepository.Remove(postId);
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(postId);
                }
                else
                {
                    response.Id = postId;
                    response.Errors = new List<string> { Translation_Posts.Post_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.Remove => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PostDto> Get(int id)
        {
            try
            {
                var post = await Task.FromResult(_unitOfWork.PostsRepository.GetFirst(g => g.Id.Equals(id)).ConvertModel(new PostDto()));

                if (post != null)
                {
                    var attachments = await _unitOfWork.AttachmentsRepository
                        .GetAll(attachment => attachment.PostId == post.Id)
                        .ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        AttachmentDto attachmentDto = attachment.ConvertModel(new AttachmentDto());

                        // Si es imagen, cargo el base64
                        var ext = Path.GetExtension(attachment.Path).ToLower();
                        bool isVideo = ext == ".mp4" || ext == ".webm" || ext == ".ogg";
                        attachmentDto.IsVideo = isVideo;

                        if (!isVideo)
                        {
                            // Leer solo la imagen en base64
                            attachmentDto.File = Convert.ToBase64String(
                                await File.ReadAllBytesAsync(attachment.Path));
                        }
                        else
                        {
                            // 1) Generar el thumbnail con FFmpeg en tiempo real
                            var thumbBytes = VideoThumbnailGenerator.GenerateThumbnail(attachment.Path, _logger);

                            if (thumbBytes != null && thumbBytes.Length > 0)
                            {
                                // Codifico a Base64 y entrego un data URI
                                attachmentDto.Thumbnail = $"data:image/png;base64,{Convert.ToBase64String(thumbBytes)}";
                            }
                            else
                            {
                                // Si FFmpeg falló, puedes dejar null o una imagen "placeholder"
                                attachmentDto.Thumbnail = null;
                            }
                            // → Para vídeo, no asignamos attachmentDto.File (queda null).
                            //    Podrías llenar attachmentDto.Thumbnail con un poster si lo tuvieras:
                            //    attachmentDto.Thumbnail = Convert.ToBase64String(File.ReadAllBytes(thumbnailPath));
                            attachmentDto.File = null;
                        }
                        post.Attachments.Add(attachmentDto);
                    }
                }
                
                return post;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.Get =>");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los posts
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostDto>> GetAll(int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var postsQuery = _unitOfWork.PostsRepository.GetAll().OrderByDescending(p => p.Created).Skip(skip).Take(pageSize);

                var posts = postsQuery.Select(p => p.ConvertModel(new PostDto())).ToList();

                List<PostDto> result = new List<PostDto>();
                foreach (var post in posts)
                {
                    result.Add(post);
                    var attachments = await _unitOfWork.AttachmentsRepository
                        .GetAll(attachment => attachment.PostId == post.Id)
                        .ToListAsync();
                    if (!attachments.IsNullOrEmpty())
                    {
                        foreach (var attachment in attachments)
                        {
                            // Creo el DTO básico
                            AttachmentDto attachmentDto = attachment.ConvertModel(new AttachmentDto());

                            // Si es imagen, cargo el base64
                            var ext = Path.GetExtension(attachment.Path).ToLower();
                            bool isVideo = ext == ".mp4" || ext == ".webm" || ext == ".ogg";
                            attachmentDto.IsVideo = isVideo;

                            if (!isVideo)
                            {
                                // Leer solo la imagen en base64
                                attachmentDto.File = Convert.ToBase64String(
                                    await File.ReadAllBytesAsync(attachment.Path));
                            }
                            else
                            {
                                // 1) Generar el thumbnail con FFmpeg en tiempo real
                                var thumbBytes = VideoThumbnailGenerator.GenerateThumbnail(attachment.Path, _logger);

                                if (thumbBytes != null && thumbBytes.Length > 0)
                                {
                                    // Codifico a Base64 y entrego un data URI
                                    attachmentDto.Thumbnail = $"data:image/png;base64,{Convert.ToBase64String(thumbBytes)}";
                                }
                                else
                                {
                                    // Si FFmpeg falló, puedes dejar null o una imagen "placeholder"
                                    attachmentDto.Thumbnail = null;
                                }
                                // → Para vídeo, no asignamos attachmentDto.File (queda null).
                                //    Podrías llenar attachmentDto.Thumbnail con un poster si lo tuvieras:
                                //    attachmentDto.Thumbnail = Convert.ToBase64String(File.ReadAllBytes(thumbnailPath));
                                attachmentDto.File = null;
                            }
                            result.Last().Attachments.Add(attachmentDto);
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetAll => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los posts filtrados
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseFilterPostDto> GetAllFilter(PostFilterRequestDto filter, int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var response = new ResponseFilterPostDto();

                // Filtrar con equals
                var equalsQuery = string.IsNullOrEmpty(filter.SearchString)
                    ? _unitOfWork.PostsRepository.GetAll().Skip(skip).Take(pageSize)
                    : _unitOfWork.PostsRepository.GetFiltered(filter.PropertyName, filter.SearchString, FilterType.equals).Skip(skip).Take(pageSize);

                var containsQuery = string.IsNullOrEmpty(filter.SearchString)
                    ? _unitOfWork.PostsRepository.GetAll().Skip(skip).Take(pageSize)
                    : _unitOfWork.PostsRepository.GetFiltered(filter.SearchString).Skip(skip).Take(pageSize);

                // Combinar resultados, eliminar duplicados y ordenar
                var result = equalsQuery
                    .Concat(containsQuery)
                    .Distinct()
                    .OrderByDescending(post =>
                        equalsQuery.Contains(post) ? int.MaxValue :
                        CalculateCosineSimilarity(post.Content, filter.SearchString))
                    .ToList();

                // Si buscamos los recientes, combinar la ordenación por relevancia y fecha
                if (filter.ByDate)
                {
                    result = result
                        .OrderByDescending(post => post.Created)
                        .ThenByDescending(post => equalsQuery.Contains(post) ? int.MaxValue : CalculateCosineSimilarity(post.Content, filter.SearchString))
                        .ToList();
                }

                var resultDto = result.Select(s => s.ConvertModel(new PostDto())).ToList();

                if (!resultDto.IsNullOrEmpty())
                {
                    foreach (var post in resultDto)
                    {
                        response.Posts.Add(post.ConvertModel(new PostDto()));
                        var attachments = await _unitOfWork.AttachmentsRepository
                        .GetAll(attachment => attachment.PostId == post.Id)
                        .ToListAsync();
                        if (!attachments.IsNullOrEmpty())
                        {
                            foreach (var attachment in attachments)
                            {
                                // Creo el DTO básico
                                AttachmentDto attachmentDto = attachment.ConvertModel(new AttachmentDto());

                                // Si es imagen, cargo el base64
                                var ext = Path.GetExtension(attachment.Path).ToLower();
                                bool isVideo = ext == ".mp4" || ext == ".webm" || ext == ".ogg";
                                attachmentDto.IsVideo = isVideo;

                                if (!isVideo)
                                {
                                    // Leer solo la imagen en base64
                                    attachmentDto.File = Convert.ToBase64String(
                                        await File.ReadAllBytesAsync(attachment.Path));
                                }
                                else
                                {
                                    // 1) Generar el thumbnail con FFmpeg en tiempo real
                                    var thumbBytes = VideoThumbnailGenerator.GenerateThumbnail(attachment.Path, _logger);

                                    if (thumbBytes != null && thumbBytes.Length > 0)
                                    {
                                        // Codifico a Base64 y entrego un data URI
                                        attachmentDto.Thumbnail = $"data:image/png;base64,{Convert.ToBase64String(thumbBytes)}";
                                    }
                                    else
                                    {
                                        // Si FFmpeg falló, puedes dejar null o una imagen "placeholder"
                                        attachmentDto.Thumbnail = null;
                                    }
                                    // → Para vídeo, no asignamos attachmentDto.File (queda null).
                                    //    Podrías llenar attachmentDto.Thumbnail con un poster si lo tuvieras:
                                    //    attachmentDto.Thumbnail = Convert.ToBase64String(File.ReadAllBytes(thumbnailPath));
                                    attachmentDto.File = null;
                                }
                                response.Posts.Last().Attachments.Add(attachmentDto);
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, Translation_Posts.Error_post_filter);
                throw;
            }
        }

        #region Algoritmos de filtrado

        /// <summary>
        /// Calcula la similitud coseno entre el contenido de un post y la cadena de búsqueda.
        /// </summary>
        private double CalculateCosineSimilarity(string postContent, string searchString)
        {
            if (string.IsNullOrEmpty(postContent) || string.IsNullOrEmpty(searchString)) return 0;

            // Dividir el texto en palabras
            var postWords = Tokenize(postContent);
            var searchWords = Tokenize(searchString);

            // Obtener la frecuencia de términos (TF) para ambos textos
            var postTf = CalculateTermFrequency(postWords);
            var searchTf = CalculateTermFrequency(searchWords);

            // Calcular TF-IDF
            var vocabulary = postTf.Keys.Union(searchTf.Keys).ToList();
            var postVector = CreateTfIdfVector(postTf, vocabulary);
            var searchVector = CreateTfIdfVector(searchTf, vocabulary);

            // Calcular similitud coseno
            return ComputeCosineSimilarity(postVector, searchVector);
        }

        /// <summary>
        /// Tokeniza un texto en palabras.
        /// </summary>
        private List<string> Tokenize(string text)
        {
            return text.ToLower().Split(new[] { ' ', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Calcula la frecuencia de términos (TF) en un conjunto de palabras.
        /// </summary>
        private Dictionary<string, int> CalculateTermFrequency(List<string> words)
        {
            var frequency = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (frequency.ContainsKey(word))
                    frequency[word]++;
                else
                    frequency[word] = 1;
            }
            return frequency;
        }

        /// <summary>
        /// Crea un vector TF-IDF dado un diccionario de frecuencias y un vocabulario.
        /// </summary>
        private List<double> CreateTfIdfVector(Dictionary<string, int> termFrequency, List<string> vocabulary)
        {
            var vector = new List<double>();
            foreach (var term in vocabulary)
            {
                vector.Add(termFrequency.ContainsKey(term) ? termFrequency[term] : 0);
            }
            return vector;
        }

        /// <summary>
        /// Calcula la similitud coseno entre dos vectores.
        /// </summary>
        private double ComputeCosineSimilarity(List<double> vectorA, List<double> vectorB)
        {
            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (int i = 0; i < vectorA.Count; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += Math.Pow(vectorA[i], 2);
                magnitudeB += Math.Pow(vectorB[i], 2);
            }

            if (magnitudeA == 0 || magnitudeB == 0) return 0;

            return dotProduct / (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
        }

        #endregion

        /// <summary>
        /// Obtiene los posts asignados al usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="page">el número de la página</param>
        /// <param name="pageSize">el número de posts por página</param>
        /// <returns>una lista con los posts <see cref="PostDto"/></returns>
        public async Task<IEnumerable<PostDto>> GetByUser(int userId, bool areComments, int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var postsQuery = _unitOfWork.PostsRepository.GetAll(post => post.UserId == userId)
                    .OrderByDescending(p => p.Created);

                var posts = postsQuery.Where(p => areComments ? p.PostId != 0 : p.PostId == 0).Skip(skip).Take(pageSize).Select(p => p.ConvertModel(new PostDto())).ToList();

                List<PostDto> result = new List<PostDto>();

                foreach (var post in posts)
                {
                    result.Add(post);

                    // Obtengo todos los attachments
                    var attachments = await _unitOfWork.AttachmentsRepository
                        .GetAll(attachment => attachment.PostId == post.Id)
                        .ToListAsync();

                    if(attachments.IsNullOrEmpty())
                        continue; // Si no hay attachments, saltamos al siguiente post

                    foreach (var attachment in attachments)
                    {
                        // Creo el DTO básico
                        AttachmentDto attachmentDto = attachment.ConvertModel(new AttachmentDto());

                        // Si es imagen, cargo el base64
                        var ext = Path.GetExtension(attachment.Path).ToLower();
                        bool isVideo = ext == ".mp4" || ext == ".webm" || ext == ".ogg";
                        attachmentDto.IsVideo = isVideo;

                        if (!isVideo)
                        {
                            // Leer solo la imagen en base64
                            attachmentDto.File = Convert.ToBase64String(
                                await File.ReadAllBytesAsync(attachment.Path));
                        }
                        else
                        {
                            // 1) Generar el thumbnail con FFmpeg en tiempo real
                            var thumbBytes = VideoThumbnailGenerator.GenerateThumbnail(attachment.Path, _logger);

                            if (thumbBytes != null && thumbBytes.Length > 0)
                            {
                                // Codifico a Base64 y entrego un data URI
                                attachmentDto.Thumbnail = $"data:image/png;base64,{Convert.ToBase64String(thumbBytes)}";
                            }
                            else
                            {
                                // Si FFmpeg falló, puedes dejar null o una imagen "placeholder"
                                attachmentDto.Thumbnail = null;
                            }
                            // → Para vídeo, no asignamos attachmentDto.File (queda null).
                            //    Podrías llenar attachmentDto.Thumbnail con un poster si lo tuvieras:
                            //    attachmentDto.Thumbnail = Convert.ToBase64String(File.ReadAllBytes(thumbnailPath));
                            attachmentDto.File = null;
                        }

                        result.Last().Attachments.Add(attachmentDto);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetByUser => ");
                throw;
            }
        }

        /// <summary>
        ///     Actualiza la información de un post
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <param name="newPost"><see cref="CreatePostDto"/> con los datos del nuevo post</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Update(int postId, CreatePostDto newPost)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                Post post = await _unitOfWork.PostsRepository.Get(postId);
                if (post != null)
                {
                    post.Content = newPost.Content;
                    post.LastEdited = DateTime.UtcNow;

                    _unitOfWork.PostsRepository.Update(post);
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(postId);
                }
                else
                {
                    response.Id = postId;
                    response.Errors = new List<string> { Translation_Posts.Post_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.Update => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona la acción de dar me gusta a un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Like(int userId, int postId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var like = new Like(userId, postId);

                if (!IsLiked(userId, postId).Result)
                {

                    if (_unitOfWork.LikesRepository.Add(like) != null)
                    {
                        await _unitOfWork.SaveChanges();
                        response.IsSuccess(postId);
                    }
                }
                else
                {
                    response.Id = 0;
                    response.Errors = new List<string> { Translation_Posts.Default_error };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.Like => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona la acción de quitar me gusta a un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Dislike(int userId, int postId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var post = await Get(postId);

                if (post != null)
                {
                    await _unitOfWork.LikesRepository.Remove([userId, postId]);
                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    response.Errors = new List<string> { String.Format(Translation_Posts.Default_error, postId) };
                }
                response.Id = postId;
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.Dislike => ");
                throw;
            }
        }

        /// <summary>
        ///     Comprueba si un usuario ha dado me gusta a un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<bool> IsLiked(int userId, int postId)
        {
            try
            {
                if (await _unitOfWork.LikesRepository.Get([userId, postId]) != null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.IsLiked => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los post a los que le ha dado me gusta un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        public async Task<List<PostDto>> GetLiked(int userId, int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var likes = _unitOfWork.LikesRepository.GetAll(l => l.UserId == userId).OrderByDescending(s => s.Timestamp).Skip(skip).Take(pageSize);
                var posts = new List<PostDto>();

                foreach (var like in likes)
                {
                    // Obtenemos el post principal
                    var postEntity = await _unitOfWork.PostsRepository.Get(like.PostId);
                    var postDto = postEntity.ConvertModel(new PostDto());
                    posts.Add(postDto);

                    // Obtenemos attachments de ese post
                    var attachments = await _unitOfWork.AttachmentsRepository
                        .GetAll(attachment => attachment.PostId == like.PostId)
                        .ToListAsync();

                    foreach (var attachment in attachments)
                    {
                        var attachmentDto = attachment.ConvertModel(new AttachmentDto());
                        var ext = Path.GetExtension(attachment.Path).ToLower();
                        bool isVideo = ext == ".mp4" || ext == ".webm" || ext == ".ogg";
                        attachmentDto.IsVideo = isVideo;

                        if (!isVideo)
                        {
                            // Si es imagen, cargo el base64
                            var bytes = await File.ReadAllBytesAsync(attachment.Path);
                            attachmentDto.File = Convert.ToBase64String(bytes);
                            attachmentDto.Thumbnail = null; // No aplica a imagen
                        }
                        else
                        {
                            // 1) Generar el thumbnail con FFmpeg en tiempo real
                            var thumbBytes = VideoThumbnailGenerator.GenerateThumbnail(attachment.Path, _logger);

                            if (thumbBytes != null && thumbBytes.Length > 0)
                            {
                                // Codifico a Base64 y entrego un data URI
                                attachmentDto.Thumbnail = $"data:image/png;base64,{Convert.ToBase64String(thumbBytes)}";
                            }
                            else
                            {
                                // Si FFmpeg falló, puedes dejar null o una imagen "placeholder"
                                attachmentDto.Thumbnail = null;
                            }

                            // No cargamos el archivo completo aquí; se descargará bajo demanda
                            attachmentDto.File = null;
                        }

                        posts.Last().Attachments.Add(attachmentDto);
                    }
                }

                return posts.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetLiked => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los usuarios que han dado me gusta a un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<List<int>> GetLikers(int postId)
        {
            try
            {
                var likes = await _unitOfWork.LikesRepository.GetAll(l => l.PostId == postId).ToListAsync();
                var userIds = new List<int>();
                foreach (var like in likes)
                {
                    userIds.Add(like.UserId);
                }
                return userIds;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetLikers => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona la acción de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Share(int userId, int postId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var share = new Share(userId, postId);

                if (!IsShared(userId, postId).Result)
                {

                    if (_unitOfWork.SharesRepository.Add(share) != null)
                    {
                        await _unitOfWork.SaveChanges();
                        response.IsSuccess(postId);
                    }
                }
                else
                {
                    response.Id = 0;
                    response.Errors = new List<string> { Translation_Posts.Default_error };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.Share => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona la acción de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Save(int userId, int postId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var share = new Save(userId, postId);

                if (!IsSaved(userId, postId).Result)
                {

                    if (_unitOfWork.SavesRepository.Add(share) != null)
                    {
                        await _unitOfWork.SaveChanges();
                        response.IsSuccess(postId);
                    }
                }
                else
                {
                    response.Id = 0;
                    response.Errors = new List<string> { Translation_Posts.Default_error };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.Save => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona la acción de dejar de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> StopSharing(int userId, int postId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var share = await _unitOfWork.SharesRepository.Get([userId, postId]);

                if (share != null)
                {
                    await _unitOfWork.SharesRepository.Remove([userId, postId]);
                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    response.Errors = new List<string> { String.Format(Translation_Posts.Default_error, postId) };
                }
                response.Id = postId;
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.StopSharing => ");
                throw;
            }
        }

        /// <summary>
        ///     Gestiona la acción de dejar de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> StopSaving(int userId, int postId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var save = await _unitOfWork.SavesRepository.Get([userId, postId]);

                if (save != null)
                {
                    await _unitOfWork.SavesRepository.Remove([userId, postId]);
                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    response.Errors = new List<string> { String.Format(Translation_Posts.Default_error, postId) };
                }
                response.Id = postId;
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.StopSaving => ");
                throw;
            }
        }

        /// <summary>
        ///     Comprueba si un usuario ha compartido un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<bool> IsShared(int userId, int postId)
        {
            try
            {
                if (await _unitOfWork.SharesRepository.Get([userId, postId]) != null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.IsShared => ");
                throw;
            }
        }

        /// <summary>
        ///     Comprueba si un usuario ha compartido un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<bool> IsSaved(int userId, int postId)
        {
            try
            {
                if (await _unitOfWork.SavesRepository.Get([userId, postId]) != null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.IsSaved => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los posts que ha compartido un usuario cuyo id se pasa como parámetro
        ///     y genera thumbnails para cada vídeo si aún no existe uno en la BD.
        /// </summary>
        public async Task<List<PostDto>> GetShared(int userId, int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var shares = _unitOfWork.SharesRepository.GetAll(l => l.UserId == userId).OrderByDescending(s => s.Timestamp).Skip(skip).Take(pageSize);
                var posts = new List<PostDto>();

                foreach (var share in shares)
                {
                    // Obtenemos el post principal
                    var postEntity = await _unitOfWork.PostsRepository.Get(share.PostId);
                    var postDto = postEntity.ConvertModel(new PostDto());
                    posts.Add(postDto);

                    // Obtenemos attachments de ese post
                    var attachments = await _unitOfWork.AttachmentsRepository
                        .GetAll(attachment => attachment.PostId == share.PostId)
                        .ToListAsync();

                    foreach (var attachment in attachments)
                    {
                        var attachmentDto = attachment.ConvertModel(new AttachmentDto());
                        var ext = Path.GetExtension(attachment.Path).ToLower();
                        bool isVideo = ext == ".mp4" || ext == ".webm" || ext == ".ogg";
                        attachmentDto.IsVideo = isVideo;

                        if (!isVideo)
                        {
                            // Si es imagen, cargo el base64
                            var bytes = await File.ReadAllBytesAsync(attachment.Path);
                            attachmentDto.File = Convert.ToBase64String(bytes);
                            attachmentDto.Thumbnail = null; // No aplica a imagen
                        }
                        else
                        {
                            // 1) Generar el thumbnail con FFmpeg en tiempo real
                            var thumbBytes = VideoThumbnailGenerator.GenerateThumbnail(attachment.Path, _logger);

                            if (thumbBytes != null && thumbBytes.Length > 0)
                            {
                                // Codifico a Base64 y entrego un data URI
                                attachmentDto.Thumbnail = $"data:image/png;base64,{Convert.ToBase64String(thumbBytes)}";
                            }
                            else
                            {
                                // Si FFmpeg falló, puedes dejar null o una imagen "placeholder"
                                attachmentDto.Thumbnail = null;
                            }

                            // No cargamos el archivo completo aquí; se descargará bajo demanda
                            attachmentDto.File = null;
                        }

                        posts.Last().Attachments.Add(attachmentDto);
                    }
                }

                return posts.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetShared => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los posts que ha compartido un usuario cuyo id se pasa como parámetro
        ///     y genera thumbnails para cada vídeo si aún no existe uno en la BD.
        /// </summary>
        public async Task<List<PostDto>> GetSaved(int userId, int page, int pageSize = 10)
        {
            try
            {
                var skip = (page - 1) * pageSize;

                var shares = _unitOfWork.SavesRepository.GetAll(l => l.UserId == userId).OrderByDescending(s => s.Timestamp).Skip(skip).Take(pageSize);
                var posts = new List<PostDto>();

                foreach (var share in shares)
                {
                    // Obtenemos el post principal
                    var postEntity = await _unitOfWork.PostsRepository.Get(share.PostId);
                    var postDto = postEntity.ConvertModel(new PostDto());
                    posts.Add(postDto);

                    // Obtenemos attachments de ese post
                    var attachments = await _unitOfWork.AttachmentsRepository
                        .GetAll(attachment => attachment.PostId == share.PostId)
                        .ToListAsync();

                    foreach (var attachment in attachments)
                    {
                        var attachmentDto = attachment.ConvertModel(new AttachmentDto());
                        var ext = Path.GetExtension(attachment.Path).ToLower();
                        bool isVideo = ext == ".mp4" || ext == ".webm" || ext == ".ogg";
                        attachmentDto.IsVideo = isVideo;

                        if (!isVideo)
                        {
                            // Si es imagen, cargo el base64
                            var bytes = await File.ReadAllBytesAsync(attachment.Path);
                            attachmentDto.File = Convert.ToBase64String(bytes);
                            attachmentDto.Thumbnail = null; // No aplica a imagen
                        }
                        else
                        {
                            // 1) Generar el thumbnail con FFmpeg en tiempo real
                            var thumbBytes = VideoThumbnailGenerator.GenerateThumbnail(attachment.Path, _logger);

                            if (thumbBytes != null && thumbBytes.Length > 0)
                            {
                                // Codifico a Base64 y entrego un data URI
                                attachmentDto.Thumbnail = $"data:image/png;base64,{Convert.ToBase64String(thumbBytes)}";
                            }
                            else
                            {
                                // Si FFmpeg falló, puedes dejar null o una imagen "placeholder"
                                attachmentDto.Thumbnail = null;
                            }

                            // No cargamos el archivo completo aquí; se descargará bajo demanda
                            attachmentDto.File = null;
                        }

                        posts.Last().Attachments.Add(attachmentDto);
                    }
                }

                return posts.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetShared => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene los usuarios que han compartido un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<List<int>> GetSharers(int postId)
        {
            try
            {
                var shares = await _unitOfWork.SharesRepository.GetAll(l => l.PostId == postId).ToListAsync();
                var userIds = new List<int>();
                foreach (var share in shares)
                {
                    userIds.Add(share.UserId);
                }
                return userIds;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetSharers => ");
                throw;
            }
        }

        /// <summary>
        ///     Envía un email
        /// </summary>
        /// <param name="email">el email destino</param>
        /// <param name="link">el enlace al post</param>
        /// <returns></returns>
        public bool SendMail(string email, string link)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(Literals.Email_Name, Literals.Email_Address));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = Translation_Posts.Email_title;
                message.Body = new TextPart("plain") { Text = string.Concat(Translation_Posts.Email_body, "\n", link) };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(Literals.Email_Service, Literals.Email_Port, SecureSocketOptions.StartTls);
                    client.Authenticate(Literals.Email_Address, Literals.Email_Auth);
                    client.Send(message);
                    client.Disconnect(true);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Send Mail => ");
                return false;
            }
        }

        public async Task<List<int>> GetFollowedUserIdsAsync(int userId)
        {
            try
            {
                var key = $"{Literals.Redis_Users_Following}{userId}";
                var ids = await _redisCacheService.GetAsync<List<int>>(key);
                return ids ?? new List<int>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetFollowedUserIdsAsync => ");
                Console.WriteLine($"Exception: {e.StackTrace}\nMessage: {e.Message}\nInnerException: {e.InnerException}");
                throw;
            }
        }

        /// <summary>
        /// Busca en la base de datos el attachment con el ID indicado y, si existe,
        /// abre un FileStream sobre su Path y devuelve también el MimeType apropiado.
        /// </summary>
        /// <param name="attachmentId">ID del attachment</param>
        /// <returns>
        /// Un tupla (Stream, mimeType) si el attachment existe y el fichero en disco está
        /// disponible; o null si no se encontró o no existe el fichero.
        /// </returns>
        public async Task<(Stream Stream, string MimeType)?> GetAttachmentStreamAsync(int attachmentId)
        {
            try
            {
                // 1) Obtener el attachment de la BD
                var attachment = await _unitOfWork.AttachmentsRepository.Get(attachmentId);

                if (attachment == null)
                    return null;

                // 2) Comprobar que el fichero existe en disco
                var fullPath = attachment.Path;
                if (!File.Exists(fullPath))
                    return null;

                // 3) Determinar el MIME según extensión
                string mimeType = attachment.Path.ToLower() switch
                {
                    var p when p.EndsWith(".mp4") => "video/mp4",
                    var p when p.EndsWith(".webm") => "video/webm",
                    var p when p.EndsWith(".ogg") => "video/ogg",
                    _ => "application/octet-stream"
                };

                // 4) Abrir el FileStream en modo lectura
                var stream = File.OpenRead(fullPath);
                return (stream, mimeType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo stream de attachment {AttachmentId}", attachmentId);
                return null;
            }
        }

        #endregion
    }
}
