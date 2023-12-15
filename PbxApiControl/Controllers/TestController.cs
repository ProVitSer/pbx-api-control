using Microsoft.AspNetCore.Mvc;
using System;
using TCX.Configuration;


namespace PbxApiControl.Controllers
{

    [ApiController]
    [Route("api")]
    public class TestController : ControllerBase
    {


        [HttpGet("ext.info.get", Name = "GetExtInfo")]
        public void GetExtInfo()
        {

            using (DN dnByNumber = PhoneSystem.Root.GetDNByNumber((string)"101"))
            {

                Console.WriteLine(dnByNumber is Extension extension);
                if (dnByNumber is Extension ext)
                {
                    Console.WriteLine(ext.EmailAddress);
                };
            }

        }

        [HttpGet("makecall", Name = "MakeCall")]
        public void MakeCall()
        {

            PhoneSystem.Root.MakeCall((string)"101", "79104061420");
        }
    }
}