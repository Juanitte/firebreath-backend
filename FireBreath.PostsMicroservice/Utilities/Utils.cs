﻿using Common.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace FireBreath.PostsMicroservice.Utilities
{
    public class Utils
    {
        /// <summary>
        ///     Guarda un archivo adjunto en el sistema de archivos
        /// </summary>
        /// <param name="attachment"><see cref="IFormFile"/> con los datos del archivo adjunto a guardar</param>
        /// <param name="containerId">el id del post/mensaje</param>
        /// <param name="containerType"><see cref="AttachmentContainerType"/>que define si el contenedor es un post o un mensaje</param>
        /// <returns>la ruta del archivo guardado</returns>
        public static async Task<string> SaveAttachmentToFileSystem(IFormFile attachment, int containerId, AttachmentContainerType containerType)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            var fileName = Path.GetFileNameWithoutExtension(attachment.FileName) + "_" + date + Path.GetExtension(attachment.FileName);
            string directoryPath = Path.Combine("D://Proyectos/.NET/FireBreath_Backend_Storage/Files/", containerType.ToString() + "/");
            directoryPath = Path.Combine(directoryPath, containerId.ToString());
            string filePath = Path.Combine(directoryPath, fileName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            return filePath;
        }

        /// <summary>
        ///     Busca un archivo adjunto en el sistema de archivos y lo devuelve como un FileStream.
        /// </summary>
        /// <param name="path">La ruta del archivo en el sistema.</param>
        /// <returns>Un FileStream si el archivo existe, o null si no se encuentra.</returns>
        public static FileStream GetAttachmentFromFileSystem(string path)
        {
            if (File.Exists(path))
            {
                return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            return null;
        }

        /// <summary>
        ///     Hashea un texto
        /// </summary>
        /// <param name="text">el texto a hashear</param>
        /// <returns></returns>
        public static string Hash(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
