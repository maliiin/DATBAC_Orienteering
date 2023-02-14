using orienteering_backend.Core.Domain.Authentication;
using System;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;


namespace orienteering_backend.Core.Domain.Track.Services;

public interface ITrackService
{
    public Task<List<CheckpointDto>> GetCheckpointsForTrack(Guid trackId);


}
