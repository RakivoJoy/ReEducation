using System;
using System.IO;
using Xunit;

namespace SolutionSetupTests;

[Collection(FileIOCollection.Name)]
public class JsonGarageStorageSaveTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public JsonGarageStorageSaveTests() => Directory.CreateDirectory(_folder);

    public void Dispose()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, recursive: true);
        }
    }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Save_WritesFile() { }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Save_CreatesMissingDirectory() { }

    [Fact(Skip = "Placeholder: JsonGarageStorage not implemented yet.")]
    public void Save_OverwritesExistingFile_AndLeavesNoTempFile() { }
}
