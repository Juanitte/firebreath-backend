using Common.Utilities;
using FireBreath.PostsMicroservice.Models.Dtos.CreateDto;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using FireBreath.PostsMicroservice.Models.Entities;
using FireBreath.PostsMicroservice.Models.UnitsOfWork;
using FireBreath.PostsMicroservice.Translations;
using FireBreath.PostsMicroservice.Utilities;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MimeKit;

namespace FireBreath.PostsMicroservice.Services
{
    public interface IMessagesService
    {
        /// <summary>
        ///     Obtiene todos los mensajes
        /// </summary>
        /// <returns>un lista de mensajes <see cref="MessageDto"/></returns>
        public Task<List<MessageDto>> GetAll();

        /// <summary>
        ///     Obtiene el mensaje cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="id">el id del mensaje a buscar</param>
        /// <returns><see cref="MessageDto"/> con los datos del mensaje</returns>
        public Task<MessageDto> Get(int id);

        /// <summary>
        ///     Crea un nuevo mensaje
        /// </summary>
        /// <param name="message"><see cref="Message"/> con los datos del nuevo mensaje</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public Task<CreateEditRemoveResponseDto> Create(CreateMessageDto createMessage);

        /// <summary>
        ///     Actualiza los datos del mensaje cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="messageId">el id del mensaje</param>
        /// <param name="message"><see cref="MessageDto"/> con los nuevos datos del mensaje</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public Task<CreateEditRemoveResponseDto> Update(MessageDto newMessage);

        /// <summary>
        ///     Elimina el mensaje cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="id">el id del mensaje</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public Task<CreateEditRemoveResponseDto> Remove(int id);

        /// <summary>
        ///     Elimina los mensajes enviados por un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public Task<CreateEditRemoveResponseDto> RemoveByChat(int chatId);

        /// <summary>
        ///     Obtiene todos los mensajes que ha enviado un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns></returns>
        public Task<List<MessageDto>> GetByChat(int chatId);

        /// <summary>
        ///     Obtiene todos los mensajes que ha enviado un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns></returns>
        public Task<MessageDto> GetLastByChat(int chatId);

        /// <summary>
        ///     Crea un nuevo chat
        /// </summary>
        /// <param name="createChat"></param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> CreateChat(CreateChatDto createChat);

        /// <summary>
        ///     Actualiza los datos de un chat cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="newChat"></param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> UpdateChat(ChatDto newChat);

        /// <summary>
        ///     Elimina el chat cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> RemoveChat(int chatId);

        /// <summary>
        ///     Obtiene todos los chats
        /// </summary>
        /// <returns></returns>
        public Task<List<ChatDto>> GetAllChats();

        /// <summary>
        ///     Obtiene el chat cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public Task<ChatDto> GetChat(int chatId);

        /// <summary>
        ///     Obtiene todos los chats de un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<ChatDto>> GetChatsByUser(int userId);
    }
    public class MessagesService : BaseService, IMessagesService
    {
        #region Constructores

        public MessagesService(JuaniteUnitOfWork juaniteUnitOfWork, ILogger logger) : base(juaniteUnitOfWork, logger)
        {
        }

        #endregion

        #region Implementación de métodos de la interfaz

        /// <summary>
        ///     Crea un nuevo mensaje
        /// </summary>
        /// <param name="message"><see cref="Message"/> con los datos del nuevo mensaje</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public async Task<CreateEditRemoveResponseDto> Create(CreateMessageDto createMessage)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                Message message;
                message = new Message(createMessage.Content, createMessage.ChatId, createMessage.SenderId);
                if (message != null)
                {

                    if (_unitOfWork.MessagesRepository.Add(message) != null)
                    {
                        await _unitOfWork.SaveChanges();
                        response.IsSuccess(message.Id);


                        if (!createMessage.Attachments.IsNullOrEmpty())
                        {
                            foreach (var attachment in createMessage.Attachments)
                            {
                                Console.WriteLine(attachment);
                                if (attachment != null)
                                {
                                    string attachmentPath = await Utils.SaveAttachmentToFileSystem(attachment, message.Id, AttachmentContainerType.MESSAGE);
                                    Attachment newAttachment = new Attachment(attachmentPath, 0, message.Id);

                                    _unitOfWork.AttachmentsRepository.Add(newAttachment);
                                }
                            }
                            await _unitOfWork.SaveChanges();
                        }
                    }
                }
                else
                {
                    response.Id = message.Id;
                    response.Errors = new List<string> { Translation_Messages.Error_create_message };
                }

                var chat = await _unitOfWork.ChatsRepository.Get(createMessage.ChatId);
                if (chat != null)
                {
                    chat.LastMessage = message.Content;
                    chat.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.ChatsRepository.Update(chat);
                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    response.Errors.Add(Translation_Messages.Message_not_found);
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine($"MessagesService.Create => Exception: {e.Message}");
                Console.WriteLine($"InnerException: {e.InnerException?.Message}");
                Console.WriteLine($"StackTrace: {e.StackTrace}");
                Console.WriteLine($"Inner StackTrace: {e.InnerException?.StackTrace}");
                throw;
            }
        }

        /// <summary>
        ///     Elimina el mensaje cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="id">el id del mensaje a eliminar</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public async Task<CreateEditRemoveResponseDto> Remove(int id)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var message = _unitOfWork.MessagesRepository.GetFirst(g => g.Id == id);

                if (message != null)
                {
                    if (!message.Attachments.IsNullOrEmpty())
                    {
                        foreach (var attachmentPath in message.Attachments)
                        {
                            await _unitOfWork.AttachmentsRepository.Remove(attachmentPath.Id);
                        }
                    }
                    await _unitOfWork.MessagesRepository.Remove(id);
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(id);
                }
                else
                {
                    response.Id = id;
                    response.Errors = new List<string> { Translation_Messages.Message_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.Remove => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el mensaje cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="id">el id a buscar</param>
        /// <returns><see cref="MessageDto"/> con la información del mensaje</returns>
        public async Task<MessageDto> Get(int id)
        {
            try
            {
                var message = _unitOfWork.MessagesRepository.GetFirst(g => g.Id.Equals(id)).ConvertModel(new MessageDto());
                var attachments = await _unitOfWork.AttachmentsRepository.GetAll(a => a.MessageId == message.Id).ToListAsync();
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
                }
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.Remove => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los mensajes
        /// </summary>
        /// <returns>una lista con los mensajes <see cref="Message"/></returns>
        public async Task<List<MessageDto>> GetAll()
        {
            try
            {
                var messages = await _unitOfWork.MessagesRepository.GetAll().ToListAsync();
                List<MessageDto> result = new List<MessageDto>();
                foreach (var message in messages)
                {
                    result.Add(message.ConvertModel(new MessageDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.MessageId == message.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.GetAll => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Actualiza los datos de un mensaje cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="messageId">el id del mensaje</param>
        /// <param name="newMessage"><see cref="Message"/> con los nuevos datos del mensaje</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/> con los datos del mensaje</returns>
        public async Task<CreateEditRemoveResponseDto> Update(MessageDto newMessage)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                var message = await _unitOfWork.MessagesRepository.Get(newMessage.Id);
                if (message != null)
                {
                    message.Content = newMessage.Content;
                    message.LastEdited = DateTime.UtcNow;

                    _unitOfWork.MessagesRepository.Update(message);
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(newMessage.Id);
                }
                else
                {
                    response.Id = newMessage.Id;
                    response.Errors = new List<string> { Translation_Messages.Message_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.Update => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Elimina los mensajes enviados por un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public async Task<CreateEditRemoveResponseDto> RemoveByChat(int chatId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var messages = _unitOfWork.MessagesRepository.GetAll(g => g.ChatId == chatId);

                if (!messages.IsNullOrEmpty())
                {
                    foreach (var message in messages)
                    {
                        if (!message.Attachments.IsNullOrEmpty())
                        {
                            foreach (var attachment in message.Attachments)
                            {
                                await _unitOfWork.AttachmentsRepository.Remove(attachment.Id);
                            }
                        }
                        await _unitOfWork.MessagesRepository.Remove(message.Id);
                    }
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(chatId);
                }
                else
                {
                    response.Id = chatId;
                    response.Errors = new List<string> { Translation_Messages.Message_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.RemoveByChat => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los mensajes que ha enviado un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns></returns>
        public async Task<List<MessageDto>> GetByChat(int chatId)
        {
            try
            {
                var messages = await _unitOfWork.MessagesRepository.GetAll(m => m.ChatId == chatId).ToListAsync();
                List<MessageDto> result = new List<MessageDto>();
                foreach (var message in messages)
                {
                    result.Add(message.ConvertModel(new MessageDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.MessageId == message.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().Attachments.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.GetByChat => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los mensajes que ha enviado un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns></returns>
        public async Task<MessageDto> GetLastByChat(int chatId)
        {
            try
            {
                return (await _unitOfWork.MessagesRepository.GetAll(m => m.ChatId == chatId)
                    .OrderByDescending(m => m.Timestamp)
                    .FirstOrDefaultAsync()).ConvertModel(new MessageDto());
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.GetByChat => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Crea un nuevo chat
        /// </summary>
        /// <param name="createChat"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> CreateChat(CreateChatDto createChat)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                Chat chat = new Chat("", 1, createChat.UserIds);
                if (_unitOfWork.ChatsRepository.Add(chat) != null)
                {
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(chat.Id);
                }
                else
                {
                    response.Id = chat.Id;
                    response.Errors = new List<string> { Translation_Messages.Error_create_message };
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.CreateChat => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Actualiza los datos de un chat cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="newChat"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> UpdateChat(ChatDto newChat)
        {
            try
            {
                var chat = await _unitOfWork.ChatsRepository.Get(newChat.Id);
                if (chat != null)
                {
                    chat.UpdatedAt = DateTime.UtcNow;
                    chat.LastMessage = newChat.LastMessage;

                    _unitOfWork.ChatsRepository.Update(chat);
                    await _unitOfWork.SaveChanges();
                    return new CreateEditRemoveResponseDto { Id = chat.Id, Success = true };
                }
                else
                {
                    return new CreateEditRemoveResponseDto { Id = newChat.Id, Errors = new List<string> { Translation_Messages.Message_not_found } };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.UpdateChat => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Elimina el chat cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> RemoveChat(int chatId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                var chat = await _unitOfWork.ChatsRepository.Get(chatId);
                if (chat != null)
                {
                    if (!chat.Messages.IsNullOrEmpty())
                    {
                        foreach (var message in chat.Messages)
                        {
                            if (!message.Attachments.IsNullOrEmpty())
                            {
                                foreach (var attachment in message.Attachments)
                                {
                                    await _unitOfWork.AttachmentsRepository.Remove(attachment.Id);
                                }
                            }
                            await _unitOfWork.MessagesRepository.Remove(message.Id);
                        }
                    }
                    await _unitOfWork.ChatsRepository.Remove(chatId);
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(chatId);
                }
                else
                {
                    response.Id = chatId;
                    response.Errors = new List<string> { Translation_Messages.Message_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.RemoveChat => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los chats
        /// </summary>
        /// <returns></returns>
        public async Task<List<ChatDto>> GetAllChats()
        {
            try
            {
                var chats = await _unitOfWork.ChatsRepository.GetAll().Select(c => c.ConvertModel(new ChatDto())).ToListAsync();
                if (chats.IsNullOrEmpty())
                {
                    return new List<ChatDto>();
                }
                return chats;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.GetAllChats => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene el chat cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<ChatDto> GetChat(int chatId)
        {
            try
            {
                var chat = (await _unitOfWork.ChatsRepository.Get(chatId)).ConvertModel(new ChatDto());
                if (chat == null)
                {
                    return null;
                }
                return chat;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.GetChat => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los chats de un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ChatDto>> GetChatsByUser(int userId)
        {
            try
            {
                var chats = await _unitOfWork.ChatsRepository.GetAll(c => c.UserIds.Contains(userId)).Select(c => c.ConvertModel(new ChatDto())).ToListAsync();
                if(chats.IsNullOrEmpty())
                {
                    return new List<ChatDto>();
                }
                return chats;
            }
            catch (Exception e)
            {
                Console.WriteLine("MessagesService.GetChatsByUser => ", e);
                throw;
            }
        }

        /// <summary>
        ///     Envía un email
        /// </summary>
        /// <param name="email">el email destino</param>
        /// <param name="link">el enlace de seguimiento</param>
        /// <returns></returns>
        public bool SendMail(string email, string link)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(Literals.Email_Name, Literals.Email_Address));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = Translation_Messages.Email_title;
                message.Body = new TextPart("plain") { Text = string.Concat(Translation_Messages.Email_body, "\n", link) };

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
