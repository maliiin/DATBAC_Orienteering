using Microsoft.AspNetCore.Mvc;
using orienteering_backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Microsoft.VisualBasic;
//using static System.Net.Mime.MediaTypeNames;
using System.Security.Authentication;
//Lisens: https://github.com/SixLabors/ImageSharp/blob/main/LICENSE (31.01.2023)
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Processing;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using orienteering_backend.Core.Domain.Track;
using MediatR;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;

namespace orienteering_backend.Controllers;

[ApiController]
[Route("api/qrcode")]
public class QRCodeController : ControllerBase
{
    private readonly IMediator _mediator;
    public QRCodeController(IMediator Mediator)
    {
        _mediator = Mediator;
    }
    
    [HttpGet("getqrcodes")]
    public async Task<List<CheckpointNameAndQRCodeDto>> GetQRCodes(string TrackId)
    {
        var track = new Guid(TrackId);

        var checkpointList = await _mediator.Send(new GetQRCodes.Request(track));
        return checkpointList;
    }
}