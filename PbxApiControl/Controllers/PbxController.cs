using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PbxApiControl.Services;
using PbxApiControl.DTOs.Pbx;


namespace PbxApiControl.Controllers;

#nullable enable

[ApiController]
[Route("pbx")]
public class PbxController : ControllerBase
{

    private readonly IPbxService _pbxService;
    public PbxController(IPbxService pbxService)
    {
        _pbxService = pbxService;
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
            return BadRequest();

        }
    }

    [HttpGet("active-calls")]
    public IActionResult GetPbxActiveCalls()
    {
        try
        {

            var result = _pbxService.PbxActiveCalls();

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest();

        }
    }

}
