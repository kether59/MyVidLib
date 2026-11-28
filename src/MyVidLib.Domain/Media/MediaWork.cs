// Copyright (c) 2026 MyVidLib. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace MyVidLib.Domain.Media;

/// <summary>
/// Entity commune de toutes les �uvres locales en TPH (Table Per Hierarchy).
/// Porte le discriminateur <see cref="WorkType"/> et les colonnes conditionnelles
/// selon la sp�cialisation (Movie, Series, Episode).
/// </summary>
public class MediaWork
{
    /// <summary>
    /// Identit� interne stable de l'�uvre.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Discriminateur de sp�cialisation : Movie, Series, Episode.
    /// Obligatoire, limit� � ces trois valeurs.
    /// </summary>
    public WorkType WorkType { get; set; }

    /// <summary>
    /// Identifiant de la s�rie parente. Obligatoire pour Episode et Series.
    /// Pour un �pisode, r�f�re � la ligne MediaWork de type Series.
    /// </summary>
    public Guid? SeriesWorkId { get; set; }

    /// <summary>
    /// Identifiant de la saison. Obligatoire pour un �pisode (r�f�renc� � la saison Unknown de sa s�rie si saison inconnue).
    /// </summary>
    public Guid? SeasonId { get; set; }

    /// <summary>
    /// Titre interne de l'�uvre.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Titre original de travail.
    /// </summary>
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Titre de tri normalis�.
    /// </summary>
    public string? SortTitle { get; set; }

    /// <summary>
    /// Statut de catalogue (Accessible, Inaccessible, Disabled, Unknown).
    /// </summary>
    public CatalogueStatus CatalogueStatus { get; set; }

    /// <summary>
    /// Date de cr�ation.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date de derni�re modification.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// Indique si l'�uvre est archiv�e (suppression logique).
    /// </summary>
    public bool IsArchived { get; set; }

    /// <summary>
    /// Identifiant du fournisseur ayant fourni les m�tadonn�es.
    /// Null pour les valeurs manuelles/local.
    /// </summary>
    public Guid? ProviderId { get; set; }

    /// <summary>
    /// Navigation vers la s�rie parente (lorsque WorkType = Series ou Episode).
    /// </summary>
    public virtual MediaWork? Series { get; set; }

    /// <summary>
    /// Navigation vers la saison (lorsque SeasonId est d�fini).
    /// </summary>
    public virtual Season? Season { get; set; }

    /// <summary>
    /// Navigation vers les fichiers associ�s via MediaFileAssignment.
    /// </summary>
    public virtual ICollection<MediaFileAssignment> MediaFileAssignments { get; set; } = new List<MediaFileAssignment>();

    /// <summary>
    /// Navigation vers les relations (en source ou cible).
    /// </summary>
    public virtual ICollection<MediaWorkRelation> SourceRelations { get; set; } = new List<MediaWorkRelation>();

    /// <summary>
    /// Navigation vers les relations (en cible).
    /// </summary>
    public virtual ICollection<MediaWorkRelation> TargetRelations { get; set; } = new List<MediaWorkRelation>();

    /// <summary>
    /// Navigation vers les arcs narratifs cette �uvre participe.
    /// </summary>
    public virtual ICollection<EpisodeStoryArc> EpisodeStoryArcs { get; set; } = new List<EpisodeStoryArc>();

    /// <summary>
    /// Navigation vers les identifiants externes fournisseur.
    /// </summary>
    public virtual ICollection<MediaWorkExternalId> ExternalIds { get; set; } = new List<MediaWorkExternalId>();

    /// <summary>
    /// Navigation vers les m�tadonn�es candidates.
    /// </summary>
    public virtual ICollection<MediaWorkMetadata> MetadataCandidates { get; set; } = new List<MediaWorkMetadata>();

    /// <summary>
    /// Navigation vers les doublons physiques de cette �uvre.
    /// </summary>
    public virtual ICollection<MediaFileDuplicate> Duplicates { get; set; } = new List<MediaFileDuplicate>();
    /// <summary>
    /// Navigation vers le suivi de visionnage de l'utilisateur.
    /// </summary>
    public virtual ICollection<WatchProgress> WatchProgresses { get; set; } = new List<WatchProgress>();

    /// <summary>
    /// Navigation vers les sessions de visionnage.
    /// </summary>
    public virtual ICollection<WatchSession> WatchSessions { get; set; } = new List<WatchSession>();
}

