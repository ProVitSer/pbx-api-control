using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.Calls
{
    public class MakeCallDto
    {
        [Required]
        public string To { get; init; }
        [Required]
        public string From { get; init; }

    }
}
