namespace PbxApiControl.Errors;

#nullable enable
public record ApiException(int Status, string? Message = null, string? Details = null);
#nullable disable
