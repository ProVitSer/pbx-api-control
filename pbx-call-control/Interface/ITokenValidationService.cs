namespace PbxApiControl.Interface
{
    public interface ITokenValidationService
    
    {
        bool ValidateToken(string token);
    }
}
