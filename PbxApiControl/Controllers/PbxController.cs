using System;
using Microsoft.AspNetCore.Mvc;
using PbxApiControl.Interface;

namespace PbxApiControl.Controllers;

#nullable enable

[ApiController]
[Route("pbx")]
public class PbxController : ControllerBase
{

    private readonly IPbxService _pbxService;
    private readonly IGetActiveConnectionService _getActiveConnectionService;

    public PbxController(
        IPbxService pbxService,
        IGetActiveConnectionService getActiveConnectionService
        )
    {
        _pbxService = pbxService;
        _getActiveConnectionService = getActiveConnectionService;

    }

    [HttpGet("count-calls")]
    public IActionResult GetPbxCalls()
    {
        try
        {
            var result = _pbxService.PbxCountCalls();

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest(ex);

        }
    }

    [HttpGet("active-calls")]
    public IActionResult GetPbxActiveCalls()
    {
        try
        {
            return Ok((object)new GetActiveConnectionsService());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest();

        }
    }

}
