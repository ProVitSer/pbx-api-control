using TCX.Configuration;
using PbxApiControl.Interface;
using PbxApiControl.Models.Ivr;

namespace PbxApiControl.Services.Pbx
{
    public class IvrService: IIvrService
    {
        private readonly ILogger<IvrService> _logger;

        public IvrService(ILogger<IvrService> logger)
        {
            _logger = logger;
        }

        public IvrInfoModel[] GetIvrList()
        {
            using (var disposer = PhoneSystem.Root.GetAll<IVR>().GetDisposer())
            {
               return disposer.Select(x => new IvrInfoModel
                           {
                               Name = x.Name,
                               Number = x.Number
                           }).ToArray(); 
            }
        }
    
    
    }
    
}

