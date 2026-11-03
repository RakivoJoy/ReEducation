using Xunit;

namespace SolutionSetupTests;

/// <summary>
/// Tests in this collection run serially, and never in parallel with other collections, because they touch the file system.
/// Mark file IO test classes with [Collection(FileIOCollection.Name)].
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public class FileIOCollection
{
    public const string Name = "FileIO";
}
