using Microsoft.EntityFrameworkCore;
using MediatR;
using orienteering_backend.Core.Domain.Track;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Quiz;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Infrastructure.Data;

public class OrienteeringContext : IdentityUserContext<IdentityUser>
{
    // fix: fjern mediator
    private readonly IMediator _mediator;

    public OrienteeringContext(DbContextOptions<OrienteeringContext> configuration, IMediator mediator) : base(configuration)
    {
        _mediator = mediator;
    }

    public DbSet<Checkpoint> Checkpoints { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;
    public DbSet<Quiz> Quiz { get; set; } = null!;
    public DbSet<orienteering_backend.Core.Domain.Navigation.Navigation> Navigation { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //calls IdentityUserContext’s own OnModelCreating implementation
        //so that it can set itself up properly
        base.OnModelCreating(modelBuilder);

       

    }




}
