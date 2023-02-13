using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Track.Services;

public class TrackService:ITrackService
{
    private readonly OrienteeringContext _db;

    public TrackService(OrienteeringContext db)
    {
        _db = db;
    } 

    public async Task<List<Checkpoint>> GetCheckponitsForTrack(Guid trackId)
    {
        //await _db.Tracks.FirstOrDefaultAsync(t => t.Id.Equals(trackId));
        var track = await _db.Tracks
            .Include(t=>t.CheckpointList)
            .FirstOrDefaultAsync(t => t.Id == trackId);
        //Console.WriteLine(track);
        return track.CheckpointList;


            
    }

}

