using System;
using Microsoft.AspNetCore.Mvc;
using PbxApiControl.Interface;
using PbxApiControl.DTOs.Extension;

namespace PbxApiControl.Controllers;

#nullable enable

[ApiController]
[Route("extension")]
public class RingGroupController : ControllerBase
{
    private readonly IRingGroupService _ringGroupService;

    public RingGroupController(IRingGroupService ringGroupService)
    {
        _ringGroupService = ringGroupService;
    }

}