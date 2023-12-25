using Microsoft.AspNetCore.Mvc;
using PbxApiControl.Interface;
using PbxApiControl.DTOs.Queue;
using System;

namespace PbxApiControl.Controllers;

#nullable enable

[ApiController]
[Route("queue")]
public class QueueController : ControllerBase
{
    private readonly IQueueService _queueService;

    public QueueController(IQueueService queueService)
    {
        _queueService = queueService;
    }

    [HttpGet("list")]
    public ActionResult GetQueueList()
    {
        try
        {
            var queueNum = _queueService.GetQueueList();

            return Ok(new { queueNumbers = queueNum });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("{queueNumber}/agents")]
    public ActionResult GetQueueAgents(string queueNumber)
    {
        try
        {
            var queueAgents = _queueService.GetQueueAgents(queueNumber);

            return Ok(queueAgents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }


    [HttpGet("{queueNumber}/idle-agents")]
    public ActionResult GetFreeQueueAgents(string queueNumber)
    {
        try
        {
            var queueAgents = _queueService.GetFreeQueueAgents(queueNumber);

            return Ok(queueAgents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("{queueNumber}/busy-agents")]
    public ActionResult GetBusyQueueAgents(string queueNumber)
    {
        try
        {
            var queueAgents = _queueService.GetBusyQueueAgents(queueNumber);

            return Ok(queueAgents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("add/agents")]
    public ActionResult AddRingGroupMembers(AddQueueAgentsDto dto)
    {
        try
        {
            var queueAgents = _queueService.AddRingGroupMembers(dto);

            return Ok(queueAgents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("delete/agents")]
    public ActionResult DeleteRingGroupMembers(DeleteQueueAgentsDto dto)
    {
        try
        {
            var queueAgents = _queueService.DeleteRingGroupMembers(dto);

            return Ok(queueAgents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}