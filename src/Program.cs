using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Diagnosers;
using ImageProcessor;

BenchmarkRunner.Run<ImageServiceBenchmark>();

[ShortRunJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
[HideColumns("Error", "StdDev", "RatioSD")]
public class ImageServiceBenchmark
{
    private byte[]? _imageBytes;
    private const string _filePath = @"/Users/paragraut/Downloads/shelf-christmas-decoration.heic"; // Use a valid image path
    private HttpClient? _httpClient;
    private const string _imageUrl = "https://picsum.photos/800/900"; // Use a valid image URL

    [GlobalSetup]
    public void Setup()
    {
        _imageBytes = File.ReadAllBytes(_filePath);
        _httpClient = new HttpClient();
    }

    [Benchmark(Baseline = true)]
    public byte[] ProcessImage() =>
        ImageService.ProcessImage(_imageBytes!, 200, 200, 90);

    [Benchmark]
    public byte[] ProcessImageFromFile() =>
        ImageService.ProcessImageFromFile(_filePath, 200, 200, 90);

    [Benchmark]
    public async Task<byte[]> ProcessImageFromUrlAsync() =>
        await ImageService.ProcessImageFromUrlAsync(_httpClient!, _imageUrl, 200, 200, 90);
}