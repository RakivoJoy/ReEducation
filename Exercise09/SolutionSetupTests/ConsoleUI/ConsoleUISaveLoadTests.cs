using SolutionSetup;
using SolutionSetup.Persistence;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SolutionSetupTests;

// Uses a fake storage, so no file IO and no [Collection] needed.
public class ConsoleUISaveLoadTests
{
    private sealed class FakeStorage : IGarageStorage
    {
        public int SaveCount { get; private set; }
        public int LoadCount { get; private set; }
        public IGarage<IVehicle>? LoadedGarage { get; set; }

        public Task<OperationResult> SaveAsync(IGarage<IVehicle> garage, CancellationToken cancellationToken = default)
        {
            SaveCount++;
            return Task.FromResult(OperationResult.Ok("fake saved"));
        }

        public Task<GarageLoadResult> LoadAsync(CancellationToken cancellationToken = default)
        {
            LoadCount++;
            return Task.FromResult(LoadedGarage is null
                ? GarageLoadResult.Fail("fake load failed")
                : GarageLoadResult.Ok(LoadedGarage, "fake loaded"));
        }
    }

    [Fact]
    public void SaveMenuOption_CallsStorageSave()
    {
        // Arrange
        var storage = new FakeStorage();
        var handler = new GarageHandler(storage, new SampleDataProvider());
        handler.CreateGarage(5);
        using var output = new StringWriter();
        using var input = new StringReader(string.Empty);
        var ui = new ConsoleUI(input, output, handler);

        // Act
        ui.SaveGarage();

        // Assert
        Assert.Equal(1, storage.SaveCount);
        Assert.Contains("fake saved", output.ToString());
    }

    [Fact]
    public void LoadMenuOption_ReplacesGarage()
    {
        // Arrange
        var storage = new FakeStorage { LoadedGarage = new Garage<IVehicle>(7) };
        var handler = new GarageHandler(storage, new SampleDataProvider());
        using var output = new StringWriter();
        using var input = new StringReader(string.Empty);
        var ui = new ConsoleUI(input, output, handler);

        // Act
        ui.LoadGarage();

        // Assert
        Assert.Equal(1, storage.LoadCount);
        Assert.True(handler.HasGarage);
        Assert.Contains("fake loaded", output.ToString());
    }

    [Fact]
    public void LoadMenuOption_DeclinedConfirmation_KeepsCurrentGarage()
    {
        // Arrange
        var storage = new FakeStorage { LoadedGarage = new Garage<IVehicle>(7) };
        var handler = new GarageHandler(storage, new SampleDataProvider());
        handler.CreateGarage(3);
        handler.PopulateGarage();
        using var output = new StringWriter();
        using var input = new StringReader("n\n");
        var ui = new ConsoleUI(input, output, handler);

        // Act
        ui.LoadGarage();

        // Assert
        Assert.Equal(0, storage.LoadCount);
        Assert.NotEmpty(handler.ListVehicles());
        Assert.Contains("Load cancelled", output.ToString());
    }

    [Fact]
    public void LoadMenuOption_ConfirmedOverwrite_ReplacesGarage()
    {
        // Arrange
        var storage = new FakeStorage { LoadedGarage = new Garage<IVehicle>(7) };
        var handler = new GarageHandler(storage, new SampleDataProvider());
        handler.CreateGarage(3);
        handler.PopulateGarage();
        using var output = new StringWriter();
        using var input = new StringReader("y\n");
        var ui = new ConsoleUI(input, output, handler);

        // Act
        ui.LoadGarage();

        // Assert
        Assert.Equal(1, storage.LoadCount);
        Assert.Empty(handler.ListVehicles());
        Assert.Contains("fake loaded", output.ToString());
    }

    [Fact]
    public void SaveMenuOption_WithoutGarage_PrintsErrorAndSkipsStorage()
    {
        // Arrange
        var storage = new FakeStorage();
        var handler = new GarageHandler(storage, new SampleDataProvider());
        using var output = new StringWriter();
        using var input = new StringReader("9\n\n0\n");
        var ui = new ConsoleUI(input, output, handler);

        // Act
        ui.Run();

        // Assert
        Assert.Equal(0, storage.SaveCount);
        Assert.Contains(ConsoleUIText.NoGaragePleaseCreate, output.ToString());
    }

    [Fact]
    public void Run_MenuNine_SavesGarageAndExits()
    {
        // Arrange
        var storage = new FakeStorage();
        var handler = new GarageHandler(storage, new SampleDataProvider());
        handler.CreateGarage(5);
        using var output = new StringWriter();
        using var input = new StringReader("9\n\n0\n");
        var ui = new ConsoleUI(input, output, handler);

        // Act
        ui.Run();

        // Assert
        Assert.Equal(1, storage.SaveCount);
        Assert.Contains("fake saved", output.ToString());
    }

    [Fact]
    public void Run_MenuTen_LoadsGarageAndExits()
    {
        // Arrange
        var storage = new FakeStorage { LoadedGarage = new Garage<IVehicle>(7) };
        var handler = new GarageHandler(storage, new SampleDataProvider());
        using var output = new StringWriter();
        using var input = new StringReader("10\n\n0\n");
        var ui = new ConsoleUI(input, output, handler);

        // Act
        ui.Run();

        // Assert
        Assert.Equal(1, storage.LoadCount);
        Assert.True(handler.HasGarage);
        Assert.Contains("fake loaded", output.ToString());
    }
}
