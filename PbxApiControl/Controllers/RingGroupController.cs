using System;
using Microsoft.AspNetCore.Mvc;
using PbxApiControl.Interface;
using PbxApiControl.DTOs.RingGroup;

namespace PbxApiControl.Controllers;

#nullable enable

[ApiController]
[Route("ring-group")]
public class RingGroupController : ControllerBase
{
    private readonly IRingGroupService _ringGroupService;

    public RingGroupController(IRingGroupService ringGroupService)
    {
        _ringGroupService = ringGroupService;
    }

    [HttpGet("list")]
    public ActionResult GetRingGroupList()
    {
        try
        {
            var ringGroupNum = _ringGroupService.GetRingGroupList();

            return Ok(new { ringGroupNumbers = ringGroupNum });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("{ringGroupNumber}/members")]
    public ActionResult GetRingGroupList(string ringGroupNumber)
    {
        try
        {
            var ringGroupMembers = _ringGroupService.GetRingMembers(ringGroupNumber);

            return Ok(ringGroupMembers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("add/members")]
    public ActionResult AddRingGroupMembers(AddRingGroupMembersDto members)
    {
        try
        {
            var ringGroupMembers = _ringGroupService.AddRingGroupMembers(members);

            return Ok(ringGroupMembers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("delete/members")]
    public ActionResult DeleteRingGroupMembers(DeleteRingGroupMembersDto members)
    {
        try
        {
            var ringGroupMembers = _ringGroupService.DeleteRingGroupMembers(members);

            return Ok(ringGroupMembers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }


}