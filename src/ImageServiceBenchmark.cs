using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;

namespace ImageProcessor;

[ShortRunJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "RatioSD")]
public class ImageServiceBenchmark
{
    private byte[]? _imageBytes;
    private readonly string _filePath = Path.Combine("static", "shelf-christmas-decoration.heic");
    private HttpClient? _httpClient;
    private const string _imageUrl = "https://picsum.photos/800/900"; // Use a valid image URL

    [GlobalSetup]
    public void Setup()
    {
        _imageBytes = File.ReadAllBytes(_filePath);
        _httpClient = new HttpClient();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _httpClient?.Dispose();
    }

    [Benchmark(Baseline = true)]
    public byte[] ProcessImage() =>
        ImageService.ProcessImage(_imageBytes!, 200, 200, 90);

    [Benchmark]
    public byte[] ProcessImageFromFile() =>
        ImageService.ProcessImageFromFile(_filePath, 200, 200, 90);

    [Benchmark]
    public async Task<byte[]> ProcessImageFromFileAsync() =>
        await ImageService.ProcessImageFromFileAsync(_filePath, 200, 200, 90);

    [Benchmark]
    public async Task<byte[]> ProcessImageFromUrlAsync() =>
        await ImageService.ProcessImageFromUrlAsync(_httpClient!, _imageUrl, 200, 200, 90);
}