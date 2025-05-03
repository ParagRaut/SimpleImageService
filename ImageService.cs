using ImageMagick;

public static class ImageService
{
    public static byte[] ProcessImage(byte[] imageBytes, uint? width = null, uint? height = null, uint? rotation = null)
    {
        using var image = new MagickImage(imageBytes);

        if (width.HasValue && height.HasValue)
            image.Resize(width.Value, height.Value);

        if (rotation.HasValue)
            image.Rotate(rotation.Value);

        image.Quality = 75;

        return image.Format is MagickFormat.Heic or MagickFormat.Heif
            ? image.ToByteArray(MagickFormat.Jpeg)
            : image.ToByteArray();
    }

    public static async Task<byte[]> ProcessImageFromUrlAsync(HttpClient httpClient, string url, uint? width = null, uint? height = null, uint? rotation = null)
    {
        var imageBytes = await httpClient.GetByteArrayAsync(url).ConfigureAwait(false);

        return ProcessImage(imageBytes, width, height, rotation);
    }

    public static byte[] ProcessImageFromFile(string filePath, uint? width = null, uint? height = null, uint? rotation = null)
    {
        var imageBytes = File.ReadAllBytes(filePath);
        return ProcessImage(imageBytes, width, height, rotation);
    }
}