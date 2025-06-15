using System.Diagnostics;

namespace FireBreath.PostsMicroservice.Utilities
{
    public static class VideoThumbnailGenerator
    {
        /// <summary>
        /// Ejecuta FFmpeg para extraer un único fotograma (frame) a los 2 segundos del vídeo
        /// y devuelve esos bytes en formato PNG.
        /// </summary>
        /// <param name="videoPath">Ruta absoluta al fichero de vídeo.</param>
        /// <param name="logger">Logger para informar en caso de fallo.</param>
        /// <returns>Bytes del PNG extraído, o null si falla.</returns>
        public static byte[]? GenerateThumbnail(string videoPath, ILogger logger)
        {
            try
            {
                // Parámetros de FFmpeg:
                // -ss 00:00:02    → Salta a los 2 segundos (ajústalo si quieres otro timestamp)
                // -i videoPath    → Vídeo de entrada
                // -frames:v 1     → Solo un frame
                // -f image2pipe   → Salida en pipe
                // -vcodec png     → Codificar como PNG
                //
                // Esto hace que FFmpeg escriba directamente el PNG al stdout.

                var psi = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-ss 00:00:01 -i \"{videoPath}\" -frames:v 1 -f image2pipe -vcodec png -",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                if (process == null)
                {
                    logger.LogError("No se pudo iniciar el proceso FFmpeg para generar thumbnail.");
                    return null;
                }

                using var ms = new MemoryStream();
                // Leemos directamente del stdout el flujo de bytes del PNG:
                process.StandardOutput.BaseStream.CopyTo(ms);

                // Opcional: leer también el stderr para debug si falla
                string stderr = process.StandardError.ReadToEnd();

                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    logger.LogError("FFmpeg devolvió código {ExitCode}. Detalles: {Stderr}", process.ExitCode, stderr);
                    return null;
                }

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error ejecutando FFmpeg para generar thumbnail de {VideoPath}", videoPath);
                return null;
            }
        }
    }
}
