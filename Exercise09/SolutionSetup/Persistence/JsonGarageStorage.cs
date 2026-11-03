using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SolutionSetup.Vehicles;

namespace SolutionSetup.Persistence;

/// <summary>
/// Saves and loads a garage as JSON. A save is written to a temporary file, the previous save is kept as a
/// ".bak" copy, and then the temporary file replaces the save.
/// </summary>
public sealed class JsonGarageStorage : IGarageStorage
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// The default save location: %LOCALAPPDATA%\Garage\garage.json.
    /// </summary>
    public static string DefaultFilePath { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Garage", "garage.json");

    private readonly string _filePath;

    /// <summary>
    /// Creates storage that reads and writes the given file.
    /// </summary>
    public JsonGarageStorage(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        _filePath = filePath;
    }

    /// <inheritdoc />
    public async Task<OperationResult> SaveAsync(Garage<IVehicle> garage, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(garage);

        string tempPath = _filePath + ".tmp";
        string backupPath = _filePath + ".bak";
        try
        {
            string? directory = Path.GetDirectoryName(Path.GetFullPath(_filePath));
            if (directory is not null)
            {
                Directory.CreateDirectory(directory);
            }

            GarageDto dto = GarageMapper.ToDto(garage);
            await using (FileStream stream = new(tempPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                await JsonSerializer.SerializeAsync(stream, dto, Options, cancellationToken);
            }

            if (File.Exists(_filePath))
            {
                File.Copy(_filePath, backupPath, overwrite: true);
            }

            File.Move(tempPath, _filePath, overwrite: true);
            return OperationResult.Ok($"Garage saved to {_filePath}.");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return OperationResult.Fail($"Could not save the garage: {ex.Message}");
        }
        finally
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }

    /// <inheritdoc />
    public async Task<GarageLoadResult> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_filePath))
        {
            return GarageLoadResult.Fail($"No saved garage was found at {_filePath}.");
        }

        try
        {
            await using FileStream stream = new(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
            GarageDto? dto = await JsonSerializer.DeserializeAsync<GarageDto>(stream, Options, cancellationToken);
            if (dto is null)
            {
                return GarageLoadResult.Fail("The garage file is empty.");
            }

            return GarageLoadResult.Ok(GarageMapper.ToGarage(dto), $"Garage loaded from {_filePath}.");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException or ArgumentException or InvalidDataException)
        {
            return GarageLoadResult.Fail($"Could not load the garage: {ex.Message}");
        }
    }
}
