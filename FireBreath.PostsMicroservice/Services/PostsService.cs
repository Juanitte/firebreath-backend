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
        public IEnumerable<PostDto> GetByUser(int userId);
        
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
        ///     Crea un nuevo post
        /// </summary>
        /// <param name="createPost"><see cref="CreatePostDto"/> con los datos del post</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public async Task<CreateEditRemoveResponseDto> Create(CreatePostDto createPost)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var post = new Post(createPost.Author, createPost.Content, createPost.UserId, createPost.PostId);

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
                _logger.LogError(postId, "PostsService.Remove => ");
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
                return await Task.FromResult(Extensions.ConvertModel(_unitOfWork.PostsRepository.GetFirst(g => g.Id.Equals(id)), new PostDto()));
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
                List<PostDto> result = posts.Select(t => Extensions.ConvertModel(t, new PostDto())).ToList();
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
                        .OrderByDescending(post => post.Timestamp)
                        .ThenByDescending(post => equalsQuery.Contains(post) ? int.MaxValue : CalculateCosineSimilarity(post.Content, filter.SearchString))
                        .ToList();
                }

                response.Posts = result.Select(s => s.ConvertModel(new PostDto())).ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Translation_Posts.Error_post_filter);
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
        public IEnumerable<PostDto> GetByUser(int userId)
        {
            try
            {
                var posts = _unitOfWork.PostsRepository.GetAll(post => post.UserId == userId).Select(p => p.ConvertModel(new PostDto())).ToList();
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
