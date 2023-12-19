using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.Calls
{
    public class HangupCallDto
    {
        [Required]
        public string Extension { get; init; }

    }
}
