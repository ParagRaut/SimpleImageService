using BenchmarkDotNet.Running;

namespace ImageProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ImageServiceBenchmark>();
    }
}
