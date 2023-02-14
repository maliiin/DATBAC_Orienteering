using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using System.Diagnostics;

namespace orienteering_backend.Core.Domain.Track.Services;

public class TrackService:ITrackService
{
    private readonly OrienteeringContext _db;

    public TrackService(OrienteeringContext db)
    {
        _db = db;
    } 

    public async Task<List<CheckpointDto>> GetCheckpointsForTrack(Guid trackId)
    {
        //await _db.Tracks.FirstOrDefaultAsync(t => t.Id.Equals(trackId));
        //var track = await _db.Tracks
        //    .Include(t=>t.CheckpointList)
        //    .FirstOrDefaultAsync(t => t.Id == trackId);
        //Console.WriteLine(track);
        var checkpointList = await _db.Checkpoints.Where(c => c.TrackId == trackId).ToListAsync();
        var checkpointDtoList = new List<CheckpointDto>();
        for (var i = 0; i < checkpointList.Count; i++)
        {
            var checkpoint = checkpointList[i];
            var dtoElement = new CheckpointDto(checkpoint.Title, checkpoint.TrackId);
            checkpointDtoList.Add(dtoElement);
        }
        return checkpointDtoList;


    }

}

