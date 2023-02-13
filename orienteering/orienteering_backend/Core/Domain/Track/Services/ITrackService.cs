using orienteering_backend.Core.Domain.Authentication;
using System;


namespace orienteering_backend.Core.Domain.Track.Services;

public interface ITrackService
{
    public Task<List<Checkpoint>> GetCheckponitsForTrack(Guid trackId);


}
