using orienteering_backend.Core.Domain.Authentication;
using System;
using orienteering_backend.Core.Domain.Checkpoint;


namespace orienteering_backend.Core.Domain.Track.Services;

public interface ITrackService
{
    public Task<List<Checkpoint.Checkpoint>> GetCheckpointsForTrack(Guid trackId);


}
