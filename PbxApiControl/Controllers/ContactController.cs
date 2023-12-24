using System;
using Microsoft.AspNetCore.Mvc;
using PbxApiControl.Interface;
using PbxApiControl.DTOs.Contact;

namespace PbxApiControl.Controllers;

#nullable enable

[ApiController]
[Route("contact")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService constartService)
    {
        _contactService = constartService;
    }

    [HttpGet("{contactId}")]
    public ActionResult GetContactInfoById(string contactId)
    {
        try
        {
            var contactInfo = _contactService.GetContactInfoById(contactId);

            return Ok(contactInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("")]
    public ActionResult CreateContact(CreateContactDto dto)
    {
        try
        {
            var contactInfo = _contactService.CreateContact(dto);

            return Ok(contactInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpDelete("{contactId}")]
    public ActionResult DeteleContact(string contactId)
    {
        try
        {
            var contactInfo = _contactService.DeleteContactById(contactId);

            return Ok(contactInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPatch("{contactId}")]
    public ActionResult UpdateContact(string contactId, UpdateContactDto dto)
    {
        try
        {
            var contactInfo = _contactService.UpdateContactById(contactId, dto);

            return Ok(contactInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}