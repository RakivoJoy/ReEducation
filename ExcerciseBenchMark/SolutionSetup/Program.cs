using BenchmarkDotNet.Running;
using SolutionSetup.Benchmarks;

namespace SolutionSetup
{
    /// <summary>
    /// Entry point. Run in Release: dotnet run -c Release --project SolutionSetup
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // Passing args allows BenchmarkDotNet filters, e.g. --filter *Lookup*
            BenchmarkRunner.Run<CollectionsBenchmarks>(args: args);
        }
    }
}
