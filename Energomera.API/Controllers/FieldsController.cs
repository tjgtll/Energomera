using Energomera.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Energomera.API.Controllers;

[ApiController]
[Route("/[controller]")]
public class FieldsController : ControllerBase
{
    private readonly IFieldService _fieldService;

    public FieldsController(IFieldService fieldService)
    {
        _fieldService = fieldService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var result = _fieldService.GetAll();
        return Ok(result);
    }

    [HttpGet("{id}/size")]
    public IActionResult GetSize(int id)
    {
        var result = _fieldService.GetSize(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("{id}/distance")]
    public IActionResult CalculateDistance(
        int id,
        [FromQuery, Required] double lat,
        [FromQuery, Required] double lon)
    {
        var result = _fieldService.CalculateDistance(id, lat, lon);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("contains")]
    public IActionResult PointInField([FromQuery, Required] double lat,
                                      [FromQuery, Required] double lon)
    {
        var result = _fieldService.FindFieldContainingPoint(lat, lon);
        if (result == null) return Ok(false);
        return Ok(result);
    }
}