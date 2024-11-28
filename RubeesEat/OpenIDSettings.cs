using System.ComponentModel.DataAnnotations;

namespace RubeesEat;

public class OpenIDSettings
{
    [Required]
    public string ClientId { get; set; }

    [Required]
    public string ClientSecret { get; set; }

    [Required]
    public string MetadataAddress { get; set; }
}
