using Common.Utilities;
using FireBreath.PostsMicroservice.Models.Dtos.CreateDto;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using FireBreath.PostsMicroservice.Models.Entities;
using FireBreath.PostsMicroservice.Models.UnitsOfWork;
using FireBreath.PostsMicroservice.Translations;
using FireBreath.PostsMicroservice.Utilities;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
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
        /// <param name="message"><see cref="Message"/> con los nuevos datos del mensaje</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public Task<CreateEditRemoveResponseDto> Update(int messageId, CreateMessageDto newMessage);

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
        public Task<CreateEditRemoveResponseDto> RemoveBySender(int senderId);

        /// <summary>
        ///     Elimina los mensajes recibidos por un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="receiverId">el id del receptor</param>
        /// <returns></returns>
        public Task<CreateEditRemoveResponseDto> RemoveByReceiver(int receiverId);

        /// <summary>
        ///     Obtiene todos los mensajes que ha enviado un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns></returns>
        public Task<List<MessageDto>> GetBySender(int senderId);

        /// <summary>
        ///     Obtiene todos los mensajes que ha recibido un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="receiverId">el id del receptor</param>
        /// <returns></returns>
        public Task<List<MessageDto>> GetByReceiver(int receiverId);
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
                message = new Message(createMessage.Content, createMessage.Author, createMessage.SenderId, createMessage.ReceiverId);
                if (!createMessage.Attachments.IsNullOrEmpty())
                {
                    foreach (var attachment in createMessage.Attachments)
                    {
                        if (attachment != null)
                        {
                            string attachmentPath = await Utils.SaveAttachmentToFileSystem(attachment, message.Id, AttachmentContainerType.MESSAGE);
                            Attachment newAttachment = new Attachment(attachmentPath, 0, message.Id);
                            message.Attachments.Add(newAttachment);
                        }
                    }
                }

                if (!message.Attachments.IsNullOrEmpty())
                {
                    foreach (var attachmentPath in message.Attachments)
                    {
                        _unitOfWork.AttachmentsRepository.Add(attachmentPath);
                    }
                }
                if (_unitOfWork.MessagesRepository.Add(message) != null)
                {
                    response.IsSuccess(message.Id);
                    await _unitOfWork.SaveChanges();
                }
                else
                {
                    response.Id = message.Id;
                    response.Errors = new List<string> { Translation_Messages.Error_create_message };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MessagesService.Create => ");
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
                _logger.LogError(e, "MessagesService.Remove => ");
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
                    message.AttachmentPaths.Add(attachment.ConvertModel(new AttachmentDto()));
                }
                return message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MessagesService.Get =>");
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
                        result.Last().AttachmentPaths.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MessagesService.GetAll => ");
                throw;
            }
        }

        /// <summary>
        ///     Actualiza los datos de un mensaje cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="messageId">el id del mensaje</param>
        /// <param name="newMessage"><see cref="Message"/> con los nuevos datos del mensaje</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/> con los datos del mensaje</returns>
        public async Task<CreateEditRemoveResponseDto> Update(int messageId, CreateMessageDto newMessage)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                var message = await _unitOfWork.MessagesRepository.Get(messageId);
                if (message != null)
                {
                    message.Content = newMessage.Content;
                    message.LastEdited = DateTime.UtcNow;

                    _unitOfWork.MessagesRepository.Update(message);
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(messageId);
                }
                else
                {
                    response.Id = messageId;
                    response.Errors = new List<string> { Translation_Messages.Message_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MessagesService.Update => ");
                throw;
            }
        }

        /// <summary>
        ///     Elimina los mensajes enviados por un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns><see cref="CreateEditRemoveResponseDto"/></returns>
        public async Task<CreateEditRemoveResponseDto> RemoveBySender(int senderId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var messages = _unitOfWork.MessagesRepository.GetAll(g => g.SenderId == senderId);

                if (!messages.IsNullOrEmpty())
                {
                    foreach (var message in messages)
                    {
                        if (!message.Attachments.IsNullOrEmpty())
                        {
                            foreach (var attachmentPath in message.Attachments)
                            {
                                await _unitOfWork.AttachmentsRepository.Remove(attachmentPath.Id);
                            }
                        }
                        await _unitOfWork.MessagesRepository.Remove(message.Id);
                    }
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(senderId);
                }
                else
                {
                    response.Id = senderId;
                    response.Errors = new List<string> { Translation_Messages.Message_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MessagesService.RemoveBySender => ");
                throw;
            }
        }

        /// <summary>
        ///     Elimina los mensajes recibidos por un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="receiverId">el id del receptor</param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> RemoveByReceiver(int receiverId)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();

                var messages = _unitOfWork.MessagesRepository.GetAll(g => g.ReceiverId == receiverId);

                if (!messages.IsNullOrEmpty())
                {
                    foreach (var message in messages)
                    {
                        if (!message.Attachments.IsNullOrEmpty())
                        {
                            foreach (var attachmentPath in message.Attachments)
                            {
                                await _unitOfWork.AttachmentsRepository.Remove(attachmentPath.Id);
                            }
                        }
                        await _unitOfWork.MessagesRepository.Remove(message.Id);
                    }
                    await _unitOfWork.SaveChanges();
                    response.IsSuccess(receiverId);
                }
                else
                {
                    response.Id = receiverId;
                    response.Errors = new List<string> { Translation_Messages.Message_not_found };
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MessagesService.RemoveByReceiver => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los mensajes que ha enviado un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="senderId">el id del emisor</param>
        /// <returns></returns>
        public async Task<List<MessageDto>> GetBySender(int senderId)
        {
            try
            {
                var messages = await _unitOfWork.MessagesRepository.GetAll(m => m.SenderId == senderId).ToListAsync();
                List<MessageDto> result = new List<MessageDto>();
                foreach (var message in messages)
                {
                    result.Add(message.ConvertModel(new MessageDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.MessageId == message.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().AttachmentPaths.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MessagesService.GetBySender => ");
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todos los mensajes que ha recibido un usuario cuyo id se pasa como parámetro
        /// </summary>
        /// <param name="receiverId">el id del receptor</param>
        /// <returns></returns>
        public async Task<List<MessageDto>> GetByReceiver(int receiverId)
        {
            try
            {
                var messages = await _unitOfWork.MessagesRepository.GetAll(m => m.ReceiverId == receiverId).ToListAsync();
                List<MessageDto> result = new List<MessageDto>();
                foreach (var message in messages)
                {
                    result.Add(message.ConvertModel(new MessageDto()));
                    var attachments = await _unitOfWork.AttachmentsRepository.GetAll(attachment => attachment.MessageId == message.Id).ToListAsync();
                    foreach (var attachment in attachments)
                    {
                        result.Last().AttachmentPaths.Add(attachment.ConvertModel(new AttachmentDto()));
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MessagesService.GetByReceiver => ");
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
