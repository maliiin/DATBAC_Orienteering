using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Track.Dto;

namespace orienteering_backend.Core.Domain.Track.Services
{

    public interface ISessionService
    {
        public void SetStartCheckpoint(string CheckpointId);
        public Task<TrackLoggingDto> CheckTrackFinished(string CurrentCheckpoint);

        public void AddScore(int score);
    }
}
