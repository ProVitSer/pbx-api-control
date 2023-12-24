using System;
using Microsoft.AspNetCore.Mvc;
using PbxApiControl.Interface;
using PbxApiControl.DTOs.Extension;

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


    [HttpPost("")]
    public ActionResult CreateExtension(BaseExtensionDto dto)
    {
        try
        {
            var extInfo = _extensionService.CreateExtension(dto);
            if (extInfo == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} уже существует на АТС", dto.ExtensionNumber) });

            return Ok(extInfo);


        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    [HttpDelete("")]
    public ActionResult DeleteExtension(DeleteExtensionDto dto)
    {
        try
        {
            var extInfo = _extensionService.DeleteExtension(dto);
            if (extInfo == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} отсутствует на  АТС", dto.ExtensionNumber) });

            return Ok(new { result = extInfo });


        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    [HttpPatch("")]
    public ActionResult UpdateExtension(BaseExtensionDto dto)
    {
        try
        {
            var extInfo = _extensionService.UpdateExtension(dto);
            if (extInfo == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} отсутствует на  АТС", dto.ExtensionNumber) });

            return Ok(extInfo);

        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    [HttpPost("forward-status")]
    public ActionResult SetExtensionForwardStatus(SetForwardStatusDto dto)
    {
        try
        {
            var setExtStatus = _extensionService.SetExtensionForwardStatus(dto);
            if (setExtStatus == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} отсутствует на  АТС", dto.ExtensionNumber) });

            return Ok(new { result = setExtStatus });

        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    [HttpPost("queues-status")]
    public ActionResult SetQueuesStatus(SetQueuestatusDto dto)
    {
        try
        {
            var setQueuesStatus = _extensionService.SetExtensionQueuesStatus(dto);
            if (setQueuesStatus == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} отсутствует на  АТС", dto.ExtensionNumber) });

            return Ok(new { result = setQueuesStatus });

        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    [HttpPost("queue-status")]
    public ActionResult SetQueueStatus(SetQueuetatusDto dto)
    {
        try
        {
            var setQueuesStatus = _extensionService.SetExtensionQueueStatus(dto);
            if (setQueuesStatus == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} отсутствует на  АТС", dto.ExtensionNumber) });

            return Ok(new { result = setQueuesStatus });

        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }

    [HttpGet("{extension}/device")]
    public ActionResult GetDeviceInfo(string extension)
    {
        try
        {

            var extDevInfo = _extensionService.GetExtensionDeviceInfo(extension);
            if (extDevInfo == null) return BadRequest(new { ErrorMessage = string.Format("Добавочный {0} отсутствует на  АТС", extension) });

            return Ok(extDevInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);

        }
    }
}
