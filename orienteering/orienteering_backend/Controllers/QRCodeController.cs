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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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
    //private byte[] generateQR()
    //{
    //    //Kilder: https://www.c-sharpcorner.com/article/create-qr-code-using-google-charts-api-in-vb-net/ (31.01.2023)
    //    //Lisens quickchart api: https://github.com/typpo/quickchart (31.01.2023)

    //    string url = "https://www.nrk.no/hallo/";
    //    string QrLink = "https://quickchart.io/qr?text=";
    //    QrLink = QrLink + url;
    //    using (System.Net.WebClient webClient = new System.Net.WebClient())
    //    {
    //        const SslProtocols _Tls12 = (SslProtocols)0xC00;
    //        const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
    //        ServicePointManager.SecurityProtocol = Tls12;
    //        byte[] data = webClient.DownloadData(QrLink);
    //        //Console.WriteLine(data);
    //        //Image newImage = byteArrayToImage(data);
    //        //PictureBox1.Image = newImage;
    //        //string? result = System.Text.Encoding.UTF8.GetString(data);
    //        return data;
    //    }

    //}
    //private Image byteArrayToImage(byte[] byteArray)
    //{
    //    var image = Image.Load<Rgba32>(byteArray);
    //    image.Mutate(x => x.Grayscale());
    //    return image;
    //}


    //[HttpGet]
    //public byte[] Get()
    //{
    //    var result = generateQR();
    //    return result;
    //}

    [HttpGet("getqrcodes")]
    public async Task<List<CheckpointNameAndQRCodeDto>> GetQRCodes(string TrackId)
    {
        var track = new Guid(TrackId);

        var checkpointList = await _mediator.Send(new GetQRCodes.Request(track));
        return checkpointList;
    }
}