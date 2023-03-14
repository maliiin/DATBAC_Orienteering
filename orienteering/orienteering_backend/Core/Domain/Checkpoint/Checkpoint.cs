//using orienteering_backend.SharedKernel;

using System.ComponentModel.DataAnnotations.Schema;

namespace orienteering_backend.Core.Domain.Checkpoint
{
    public class Checkpoint
    {
        public Checkpoint(string title, int gameId, Guid trackId)
        {
            Title = title;
            GameId = gameId;
            TrackId = trackId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? QuizId { get; set; }
        public int GameId { get; set; }
        public byte[]? QRCode { get; set; }
        public Guid TrackId { get; set; }

        public int Order { get; set; }

        //track har tall som sier hvor mange checkpoints den har
        //så når ny checkpoint blir lagd økes denne med 1
        //når checkpoint slettes minsker denne
        //så når checkpoint addes beregnes order fra track sin teller
    }
}
