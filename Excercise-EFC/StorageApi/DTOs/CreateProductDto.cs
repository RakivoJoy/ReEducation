using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "Produktnamn är obligatoriskt")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Produktnamn måste vara mellan 1 och 255 tecken")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Pris är obligatoriskt")]
    [Range(0, int.MaxValue, ErrorMessage = "Pris kan inte vara negativt")]
    public int Price { get; set; }

    [Required(ErrorMessage = "Kategori är obligatorisk")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Kategori måste vara mellan 1 och 100 tecken")]
    public string Category { get; set; }

    [Required(ErrorMessage = "Hylla är obligatorisk")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Hylla måste vara mellan 1 och 50 tecken")]
    public string Shelf { get; set; }

    [Required(ErrorMessage = "Antal är obligatoriskt")]
    [Range(0, int.MaxValue, ErrorMessage = "Antal kan inte vara negativt")]
    public int Count { get; set; }

    [StringLength(1000, ErrorMessage = "Beskrivning kan inte överskrida 1000 tecken")]
    public string Description { get; set; }
}
