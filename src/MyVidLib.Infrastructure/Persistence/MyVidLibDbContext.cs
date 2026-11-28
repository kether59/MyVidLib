using Microsoft.EntityFrameworkCore;
using MyVidLib.Domain.Media;

namespace MyVidLib.Infrastructure.Persistence;

/// <summary>
/// Contexte EF Core pour la base SQLite MyVidLib.
/// </summary>
public sealed class MyVidLibDbContext : DbContext
{
    public MyVidLibDbContext(DbContextOptions<MyVidLibDbContext> options) : base(options) { }

    // Catalogue
    public DbSet<MediaWork> MediaWorks => Set<MediaWork>();
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<MediaWorkRelation> MediaWorkRelations => Set<MediaWorkRelation>();
    public DbSet<StoryArc> StoryArcs => Set<StoryArc>();
    public DbSet<EpisodeStoryArc> EpisodeStoryArcs => Set<EpisodeStoryArc>();
    public DbSet<Classification> Classifications => Set<Classification>();
    public DbSet<MediaWorkClassification> MediaWorkClassifications => Set<MediaWorkClassification>();

    // Fichiers physiques
    public DbSet<MediaRoot> MediaRoots => Set<MediaRoot>();
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
    public DbSet<MediaFingerprint> MediaFingerprints => Set<MediaFingerprint>();
    public DbSet<MediaFileAssignment> MediaFileAssignments => Set<MediaFileAssignment>();
    public DbSet<MediaFileDuplicate> MediaFileDuplicates => Set<MediaFileDuplicate>();

    // Scans
    public DbSet<MediaScan> MediaScans => Set<MediaScan>();
    public DbSet<MediaScanScope> MediaScanScopes => Set<MediaScanScope>();
    public DbSet<MediaScanError> MediaScanErrors => Set<MediaScanError>();

    // Fournisseurs et identifiants externes
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<MediaWorkExternalId> MediaWorkExternalIds => Set<MediaWorkExternalId>();
    public DbSet<SeasonExternalId> SeasonExternalIds => Set<SeasonExternalId>();
    public DbSet<EpisodeExternalReference> EpisodeExternalReferences => Set<EpisodeExternalReference>();
    public DbSet<EpisodeExternalMapping> EpisodeExternalMappings => Set<EpisodeExternalMapping>();

    // Métadonnées
    public DbSet<MediaWorkMetadata> MediaWorkMetadata => Set<MediaWorkMetadata>();
    public DbSet<SeasonMetadata> SeasonMetadata => Set<SeasonMetadata>();

    // Utilisateurs et visionnage
    public DbSet<User> Users => Set<User>();
    public DbSet<WatchProgress> WatchProgresses => Set<WatchProgress>();
    public DbSet<WatchSession> WatchSessions => Set<WatchSession>();

    // Paramètres
    public DbSet<ApplicationSettings> ApplicationSettings => Set<ApplicationSettings>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<ProviderSettings> ProviderSettings => Set<ProviderSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyVidLibDbContext).Assembly);

        // Configure TPH for MediaWork
        ConfigureMediaWorkTPH(modelBuilder);

        // Configure composite keys and complex relationships
        ConfigureCompositeKeys(modelBuilder);
    }

    private static void ConfigureMediaWorkTPH(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MediaWork>(entity =>
        {
            entity.HasDiscriminator<WorkType>(m => m.WorkType)
                .HasValue<MediaWork>(WorkType.Movie)
                .HasValue<MediaWork>(WorkType.Series)
                .HasValue<MediaWork>(WorkType.Episode);
        });
    }

    private static void ConfigureCompositeKeys(ModelBuilder modelBuilder)
    {
        // Episode has composite FK to Season: (SeriesWorkId, SeasonId) -> (Season.SeriesWorkId, Season.Id)
        // This is configured in SeasonConfiguration
    }
}