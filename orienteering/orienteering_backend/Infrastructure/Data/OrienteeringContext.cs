﻿using Microsoft.EntityFrameworkCore;
using MediatR;
using orienteering_backend.Core.Domain.Track;

namespace orienteering_backend.Infrastructure.Data;

public class OrienteeringContext : DbContext
{
    private readonly IMediator _mediator;

    public OrienteeringContext(DbContextOptions<OrienteeringContext> configuration, IMediator mediator) : base(configuration)
    {
        _mediator = mediator;
    }

    public DbSet<Checkpoint> Checkpoints { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;




}
