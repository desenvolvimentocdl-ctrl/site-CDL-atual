using ImageMagick;

namespace SiteCDL.Utilities
{
    public static class ConvertImage
    {

        public static void ConvertToWebP(string inputPath, string outputPath)
        {
            try
            { 
                using var image = new MagickImage(inputPath);

                // Configuração de qualidade e compressão
                image.Format = MagickFormat.WebP;
                image.Quality = 100; // Qualidade (0 a 100)

                // Salvar como WebP
                image.Write(outputPath);
                Console.WriteLine($"Imagem convertida com sucesso: {outputPath}");

                // Excluir o arquivo original
                File.Delete(inputPath);
                Console.WriteLine($"Arquivo original excluído: {inputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

    }
}
