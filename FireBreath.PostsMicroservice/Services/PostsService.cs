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
using MimeKit;
using Microsoft.EntityFrameworkCore;
using Attachment = FireBreath.PostsMicroservice.Models.Entities.Attachment;
using Microsoft.Extensions.Hosting;

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
        public Task<List<PostDto>> GetAll();

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
        public Task<IEnumerable<PostDto>> GetByUser(int userId);
        
        /// <summary>
        ///     Obtiene los posts filtrados
        /// </summary>
        /// <returns></returns>
        Task<ResponseFilterPostDto> GetAllFilter(PostFilterRequestDto filter);
        
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
        public Task<List<PostDto>> GetLiked(int userId);

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
        ///     Gestiona la acción de dejar de compartir un post por un usuario
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> StopSharing(int userId, int postId);

        /// <summary>
        ///     Comprueba si un usuario ha compartido un post
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<bool> IsShared(int userId, int postId);

        /// <summary>
        ///     Obtiene los post que ha compartido un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        public Task<List<PostDto>> GetShared(int userId);

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
        public Task<List<PostDto>> GetComments(int postId);

        /// <summary>
        ///     Obtiene el numero de comentarios de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public Task<int> GetCommentCount(int postId);
    }
    public class PostsService : BaseService, IPostsService
    {
        #region Constructores

        public PostsService(JuaniteUnitOfWork juaniteUnitOfWork, ILogger logger) : base(juaniteUnitOfWork, logger)
        {
        }

        #endregion

        #region Implementación de métodos de la interfaz

        /// <summary>
        ///     Obtiene los comentarios de un post cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="postId">el id del post</param>
        /// <returns></returns>
        public async Task<List<PostDto>> GetComments(int postId)
        {
            try
            {
                var posts = await _unitOfWork.PostsRepository.GetAll().Where(p => p.PostId == postId).ToListAsync();
                List<PostDto> result = new List<PostDto>();
                foreach (var post in posts)
                {
                    result.Add(post.ConvertModel(new PostDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.PostId == post.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PostsService.GetComments => ");
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
                                if (attachment != null)
                                {
                                    string attachmentPath = await Utils.SaveAttachmentToFileSystem(attachment, post.Id, AttachmentContainerType.POST);
                                    Attachment newAttachment = new Attachment(attachmentPath, post.Id, 0);
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
                    var attachments = _unitOfWork.AttachmentsRepository.GetAll(a => a.PostId == id);
                    if (attachments != null)
                        post.Attachments = attachments.Select(att => att.ConvertModel(new AttachmentDto())).ToList();
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
        public async Task<List<PostDto>> GetAll()
        {
            try
            {
                var posts = await _unitOfWork.PostsRepository.GetAll().ToListAsync();
                List<PostDto> result = new List<PostDto>();
                foreach (var post in posts)
                {
                    result.Add(post.ConvertModel(new PostDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.PostId == post.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
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
        public async Task<ResponseFilterPostDto> GetAllFilter(PostFilterRequestDto filter)
        {
            try
            {
                var response = new ResponseFilterPostDto();

                // Filtrar con equals
                var equalsQuery = string.IsNullOrEmpty(filter.SearchString)
                    ? _unitOfWork.PostsRepository.GetAll()
                    : _unitOfWork.PostsRepository.GetFiltered(filter.PropertyName, filter.SearchString, FilterType.equals);

                var containsQuery = string.IsNullOrEmpty(filter.SearchString)
                    ? _unitOfWork.PostsRepository.GetAll()
                    : _unitOfWork.PostsRepository.GetFiltered(filter.SearchString);

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
                        var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.PostId == post.Id).ToListAsync();
                        if (!attachments.IsNullOrEmpty())
                        {
                            foreach (var attachment in attachments)
                            {
                                response.Posts.Last().Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
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
        ///     Obtiene los posts asignados al usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns>una lista con los posts <see cref="PostDto"/></returns>
        public async Task<IEnumerable<PostDto>> GetByUser(int userId)
        {
            try
            {
                var posts = _unitOfWork.PostsRepository.GetAll(post => post.UserId == userId).Select(p => p.ConvertModel(new PostDto())).ToList();
                List<PostDto> result = new List<PostDto>();
                foreach (var post in posts)
                {
                    result.Add(post.ConvertModel(new PostDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.PostId == post.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return posts;
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
        public async Task<List<PostDto>> GetLiked(int userId)
        {
            try
            {
                var likes = _unitOfWork.LikesRepository.GetAll(l => l.UserId == userId);
                var posts = new List<PostDto>();
                foreach (var like in likes)
                {
                    posts.Add(_unitOfWork.PostsRepository.Get(like.UserId).ConvertModel(new PostDto()));
                }
                List<PostDto> result = new List<PostDto>();
                foreach (var post in posts)
                {
                    result.Add(post.ConvertModel(new PostDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.PostId == post.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return result;
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
                var likes = _unitOfWork.LikesRepository.GetAll(l => l.PostId == postId);
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

                var post = await Get(postId);

                if (post != null)
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
        ///     Obtiene los post que ha compartido un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId">el id del usuario</param>
        /// <returns></returns>
        public async Task<List<PostDto>> GetShared(int userId)
        {
            try
            {
                var shares = _unitOfWork.SharesRepository.GetAll(l => l.UserId == userId);
                var posts = new List<PostDto>();
                foreach (var share in shares)
                {
                    posts.Add(_unitOfWork.PostsRepository.Get(share.UserId).ConvertModel(new PostDto()));
                }
                List<PostDto> result = new List<PostDto>();
                foreach (var post in posts)
                {
                    result.Add(post.ConvertModel(new PostDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.PostId == post.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return result;
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
                var shares = _unitOfWork.SharesRepository.GetAll(l => l.PostId == postId);
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

        #endregion
    }
}
