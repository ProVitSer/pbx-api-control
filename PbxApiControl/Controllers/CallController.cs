using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PbxApiControl.Interface;
using PbxApiControl.DTOs.Calls;
using PbxApiControl.Models;

namespace PbxApiControl.Controllers;

#nullable enable

[ApiController]
[Route("call")]
public class CallController : ControllerBase
{
    private readonly ICallService _callService;

    public CallController(ICallService callService)
    {
        _callService = callService;
    }

    [HttpPost("make")]
    public ActionResult<IEnumerable<MakeCall>> MakeCall(MakeCallDto dto)
    {
        try
        {
            var result = _callService.MakeCall(dto);
            if (!result.Result) return BadRequest(new { ErrorMessage = result.Message });
            return Ok(result);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("hangup")]
    public IActionResult HangupCall(HangupCallDto dto)
    {
        try
        {
            var result = _callService.HangupCall(dto);
            if (!result.Result) return BadRequest(result);

            return Ok(result);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpPost("transfer")]
    public IActionResult TransferCall(TransferCallDto dto)
    {
        try
        {
            var result = _callService.TransferCall(dto);
            if (!result.Result) return BadRequest(result);

            return Ok(result);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }
}