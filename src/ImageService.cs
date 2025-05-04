using ImageMagick;

namespace ImageProcessor;

public static class ImageService
{
    public static byte[] ProcessImage(byte[] imageBytes, uint? width = null, uint? height = null, uint? rotation = null)
    {
        ArgumentNullException.ThrowIfNull(imageBytes);

        using var image = new MagickImage(imageBytes);

        if (width.HasValue || height.HasValue)
        {
            uint targetWidth = width ?? 0;
            uint targetHeight = height ?? 0;

            image.Resize(targetWidth, targetHeight);

            image.Quality = 80;

            image.Strip();
        }

        if (rotation.HasValue && rotation.Value != 0)
        {
            image.Rotate(rotation.Value);
        }

        return image.Format is MagickFormat.Heic or MagickFormat.Heif
            ? image.ToByteArray(MagickFormat.Jpeg)
            : image.ToByteArray();
    }

    public static async Task<byte[]> ProcessImageFromUrlAsync(HttpClient httpClient, string url, uint? width = null, uint? height = null, uint? rotation = null)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("URL cannot be null or empty.", nameof(url));

        var imageBytes = await httpClient.GetByteArrayAsync(url).ConfigureAwait(false);

        return ProcessImage(imageBytes, width, height, rotation);
    }

    public static byte[] ProcessImageFromFile(string filePath, uint? width = null, uint? height = null, uint? rotation = null)
    {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found.", filePath);

        var imageBytes = File.ReadAllBytes(filePath);
        return ProcessImage(imageBytes, width, height, rotation);
    }

    public static async Task<byte[]> ProcessImageFromFileAsync(string filePath, uint? width = null, uint? height = null, uint? rotation = null)
    {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found.", filePath);

        var imageBytes = await File.ReadAllBytesAsync(filePath).ConfigureAwait(false);
        return ProcessImage(imageBytes, width, height, rotation);
    }
}