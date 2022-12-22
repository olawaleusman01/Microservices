using System.ComponentModel.DataAnnotations;

namespace CommandService.Dtos
{
    public class PlatformCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Publisher { get; set; }
    }
}