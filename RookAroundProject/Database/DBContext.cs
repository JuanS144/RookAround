using Microsoft.EntityFrameworkCore;
using System;

namespace RookAroundProject
{
    public class RookAroundContext : DbContext
    {
        public virtual DbSet<Player> Players { get; set; }
        public DbSet<BasicPlayer> BasicPlayers { get; set; }
        public DbSet<GMPlayer> GMPlayers { get; set; }
        public virtual DbSet<Tournament> Tournaments { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }

        public RookAroundContext()
        {
            // Empty constructor for PostgreSQL
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string? postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
            string? postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PWD");

            options.UseNpgsql($"Host=localhost;Database=rookaround;Username={postgresUser};Password={postgresPassword}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure Player inheritance
            modelBuilder.Entity<Player>()
                .HasDiscriminator<string>("PlayerType")
                .HasValue<BasicPlayer>("BasicPlayer")
                .HasValue<GMPlayer>("GMPlayer");

            // Configure Match and MatchMode relationship
            modelBuilder.Entity<Match>()
                .Property(m => m.MatchMode)
                .HasConversion(
                    // Serialize IMatchMode to JSON when saving to DB
                    v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions { 
                        WriteIndented = true,
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                    }),
                    // Deserialize JSON back to IMatchMode when loading from DB
                    v => System.Text.Json.JsonSerializer.Deserialize<IMatchMode>(v, new System.Text.Json.JsonSerializerOptions { 
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                    })
                );

            // Configure navigation properties for Match
            modelBuilder.Entity<Match>()
                .Property<int?>("Player1Id");
            
            modelBuilder.Entity<Match>()
                .Property<int?>("Player2Id");
                
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId);
                
            // Configure Tournament relationships
            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Players)
                .WithMany(p => p.ParticipatingTournaments)
                .UsingEntity(j => j.ToTable("TournamentPlayers"));
                
            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Venue);

            base.OnModelCreating(modelBuilder);
        }

    }
}