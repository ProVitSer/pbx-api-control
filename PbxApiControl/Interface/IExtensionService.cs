using PbxApiControl.Models;

namespace PbxApiControl.Interface;
public interface IExtensionService
{
     NewExtensionStatus GetExtensionStatus(string ext); 
    
}