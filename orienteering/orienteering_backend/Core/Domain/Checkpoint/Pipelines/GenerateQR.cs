using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using System.Net;
using System.Security.Authentication;
using System.Security.Policy;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GenerateQR
{
    public record Request(
        Guid CheckpointId
        ) : IRequest<byte[]>;


    public class Handler : IRequestHandler<Request, byte[]>
    {
        private readonly OrienteeringContext _db;

        public Handler(OrienteeringContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<byte[]> Handle(Request request, CancellationToken cancellationToken)
        {

            //Kilder: https://www.c-sharpcorner.com/article/create-qr-code-using-google-charts-api-in-vb-net/ (31.01.2023)
            //Lisens quickchart api: https://github.com/typpo/quickchart (31.01.2023)
            string url = "http://152.94.160.74/checkpoint/";
            var checkpoint = await _db.Checkpoints.SingleOrDefaultAsync(c => c.Id == request.CheckpointId);
            if (checkpoint != null)
            {
                if (checkpoint.QuizId == null)
                {
                    url += "game/" + request.CheckpointId.ToString();
                }
                else
                {
                    url += "quiz/" + request.CheckpointId.ToString();
                }
            }



            string QrLink = "https://quickchart.io/qr?text=";
            QrLink = QrLink + url;
            using (WebClient webClient = new WebClient())
            {
                const SslProtocols _Tls12 = (SslProtocols)0xC00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                byte[] data = webClient.DownloadData(QrLink);
                
                if (checkpoint != null)
                {
                    checkpoint.QRCode = data;
                }
                await _db.SaveChangesAsync();
                return data;
            }

        }
    }
}


