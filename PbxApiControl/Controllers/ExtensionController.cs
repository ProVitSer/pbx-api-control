using System;
using Microsoft.AspNetCore.Mvc;
using PbxApiControl.Interface;


namespace PbxApiControl.Controllers;

#nullable enable

[ApiController]
[Route("extension")]
public class ExtensionController : ControllerBase
{
    private readonly IExtensionService _extensionService;

    public ExtensionController(IExtensionService extensionService)
    {
        _extensionService = extensionService;
    }

    [HttpGet("all")]
    public ActionResult GetAllExtensions()
    {
        try
        {
            var ext = _extensionService.GetAllExtensions();

            return Ok(new { extensions = ext });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }


    [HttpGet("registered")]
    public ActionResult GetRegisteredExtensions()
    {
        try
        {
            var regExt = _extensionService.GetRegisteredExtensions();

            return Ok(new { registerExtensions = regExt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    [HttpGet("{extension}/status")]
    public ActionResult GetExtensionStatus(string extension)
    {
        try
        {
            var extStatus = _extensionService.GetExtensionStatus(extension);
            if (extStatus == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} отсутствует на АТС", extension) });

            return Ok(extStatus);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    [HttpGet("{extension}/info")]
    public ActionResult GetExtensionInfo(string extension)
    {
        try
        {
            var extInfo = _extensionService.GetExtensionInfo(extension);
            if (extInfo == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} отсутствует на АТС", extension) });

            return Ok(extInfo);

        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }
}

