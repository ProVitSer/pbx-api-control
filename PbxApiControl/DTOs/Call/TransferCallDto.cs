using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.Calls
{
    public class TransferCallDto
    {
        [Required]
        public string Extension { get; init; }
        public string DestinationNumber { get; init; }

    }
}
