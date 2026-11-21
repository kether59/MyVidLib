# MyVidLib — Modèle de données conceptuel V2.2

**Version :** Conceptuelle V2.2
**Statut :** PROPOSÉ — en attente de validation humaine.
**Périmètre :** conception conceptuelle uniquement ; aucune implémentation EF Core ou base de données ne doit être créée avant arbitrage.

## 1. Statut et périmètre

Ce document est la référence conceptuelle consolidée du catalogue, du stockage physique, des métadonnées, des utilisateurs et du suivi de visionnage.

Il conserve les décisions validées suivantes :

- le catalogue est indépendant des fournisseurs externes ;
- une œuvre est distincte d'un fichier physique ;
- plusieurs fichiers peuvent représenter une même œuvre ;
- la disponibilité locale, l'existence du catalogue et le visionnage sont indépendants ;
- les fichiers originaux ne sont jamais déplacés, renommés ou supprimés automatiquement ;
- les utilisateurs sont intégrés dès V1 ;
- les catalogues et sources sont partagés ; les préférences et progressions sont individuelles ;
- les données locales restent accessibles hors ligne.

Le périmètre initial conserve `Movie`, `Series` et `Episode` comme valeurs du discriminateur `WorkType` de `MediaWork`. Anime est une classification explicite et extensible, non une spécialisation structurelle.

### Statuts de décision

- **VALIDÉ** — principes établis par le propriétaire dans les documents de référence : `MediaWork` et ses trois types TPH, séparation œuvres/versions/fichiers, associations plusieurs-à-plusieurs, classification Anime, `StoryArc`, suivi individuel par œuvre, identifiants typés, disponibilité indépendante des racines et fichiers, priorité fournisseurs configurable, archivage logique et protection des fichiers originaux.
- **PROPOSÉ** — recommandations de cette conception, notamment les champs de segment, les clés composites, les règles de sélection de métadonnées et les index Windows/SQLite ci-dessous ; elles attendent validation.
- **À ARBITRER** — décisions non prises, récapitulées à la section 19.

Les éléments ajoutés en V2.2 qui ne figurent pas dans la liste des principes VALIDÉS restent des recommandations PROPOSÉES ; leur présence dans le modèle ne vaut pas validation du propriétaire.

## 2. Principes de conception

1. **Identité stable** — `MediaWork.Id` identifie une œuvre dans MyVidLib. Il ne porte aucun identifiant fournisseur.
2. **Séparation du logique et du physique** — une œuvre, une version, une partie et un fichier sont des concepts distincts.
3. **Relations explicites** — toute relation fichier/œuvre ou épisode/fichier utilise une entité de liaison.
4. **Relations référentielles sûres** — aucune relation polymorphique ne sert d'ancre aux identifiants externes ou métadonnées.
5. **Traçabilité** — fournisseurs, récupérations, validations humaines et valeurs retenues restent distingués.
6. **Archivage par défaut** — les suppressions importantes utilisent un état actif/archivé ; elles ne déclenchent pas de cascade destructrice.
7. **États indépendants** — une racine, un fichier, une œuvre et un utilisateur possèdent chacun leur propre disponibilité ou cycle de vie.
8. **Indexabilité** — les chemins normalisés et les index sont définis pour éviter les duplications accidentelles sans imposer une empreinte coûteuse.

## 3. Diagramme conceptuel global

```mermaid
erDiagram
  MEDIA_WORK ||--o{ SEASON : series_work_id
  SEASON o|--o{ MEDIA_WORK : episode_season_id_when_episode
  MEDIA_WORK ||--o{ MEDIA_WORK : episode_series_work_id
  PROVIDER ||--o{ EPISODE_EXTERNAL_REFERENCE : provides
  EPISODE_EXTERNAL_REFERENCE ||--o{ EPISODE_EXTERNAL_MAPPING : maps_to
  MEDIA_WORK ||--o{ EPISODE_EXTERNAL_MAPPING : episode_work_id
  MEDIA_WORK ||--o{ STORY_ARC : series_work_id
  STORY_ARC ||--o{ EPISODE_STORY_ARC : arc_id
  MEDIA_WORK ||--o{ EPISODE_STORY_ARC : episode_work_id
  MEDIA_WORK ||--o{ MEDIA_VERSION : has
  MEDIA_WORK ||--o{ MEDIA_WORK_RELATION : source_work_id
  MEDIA_WORK ||--o{ MEDIA_WORK_RELATION : target_work_id
  MEDIA_WORK ||--o{ MEDIA_FILE_ASSIGNMENT : associated_with
  MEDIA_VERSION o|--o{ MEDIA_FILE_ASSIGNMENT : same_work_version
  MEDIA_ROOT ||--o{ MEDIA_FILE : contains
  MEDIA_FILE ||--o{ MEDIA_FILE_ASSIGNMENT : assigned
  MEDIA_FILE ||--o{ MEDIA_FINGERPRINT : fingerprinted
  MEDIA_ROOT ||--o{ MEDIA_SCAN : scanned
  MEDIA_SCAN ||--o{ MEDIA_SCAN_ERROR : reports
  MEDIA_SCAN ||--o{ MEDIA_SCAN_SCOPE : records
  USER ||--o{ WATCH_PROGRESS : owns
  USER ||--o{ WATCH_SESSION : creates
  MEDIA_WORK ||--o{ WATCH_PROGRESS : tracked
  MEDIA_WORK ||--o{ WATCH_SESSION : tracked
  MEDIA_FILE o|--o{ WATCH_SESSION : used
  MEDIA_FILE o|--o{ WATCH_PROGRESS : last_file
  MEDIA_FILE_ASSIGNMENT o|--o{ WATCH_PROGRESS : last_assignment
  MEDIA_FILE_ASSIGNMENT o|--o{ WATCH_SESSION : used_assignment
  PROVIDER ||--o{ MEDIA_WORK_EXTERNAL_ID : provides
  MEDIA_WORK ||--o{ MEDIA_WORK_EXTERNAL_ID : identifies
  PROVIDER ||--o{ SEASON_EXTERNAL_ID : provides
  SEASON ||--o{ SEASON_EXTERNAL_ID : identifies
  MEDIA_WORK ||--o{ MEDIA_WORK_METADATA : candidates
  SEASON ||--o{ SEASON_METADATA : candidates
  PROVIDER o|--o{ MEDIA_WORK_METADATA : provenance
  PROVIDER o|--o{ SEASON_METADATA : provenance
  CLASSIFICATION ||--o{ MEDIA_WORK_CLASSIFICATION : classifies
  MEDIA_WORK ||--o{ MEDIA_WORK_CLASSIFICATION : classified_as
```

`MediaWork` est une seule table avec `WorkType` comme discriminateur. Le diagramme ne suppose pas de tables `Movie`, `Series` ou `Episode` séparées. `Season` référence la ligne `MediaWork` dont `WorkType` vaut `Series`; les colonnes propres à Episode ne sont renseignées que lorsque `WorkType` vaut `Episode`.

## 4. Présentation des entités

### MediaWork

Entité commune de toutes les œuvres locales. Elle porte l'identité interne stable et les propriétés partagées : titre interne, titre original de travail, titre de tri, statut de catalogue, dates de création/modification, archive et provenance de la fiche. Dans la ligne TPH d'un épisode, **la recommandation PROPOSÉE** est que `SeriesWorkId` soit obligatoire et `SeasonId` facultatif.

Les propriétés propres à chaque spécialisation sont séparées :

| Spécialisation | Propriétés propres principales |
|---|---|
| `Movie` | date ou année de sortie, durée de référence |
| `Series` | statut de diffusion, nombre de saisons, période de diffusion |
| `Episode` | **PROPOSÉ** : `SeriesWorkId` obligatoire, `SeasonId` facultatif ; numéro interne éventuel, date de diffusion, durée de référence, type d'épisode |

`Episode` est toujours une œuvre selon le modèle. Il ne représente pas un fichier, une partie ni une version.

`WorkType` vaut `Movie`, `Series` ou `Episode`. Les colonnes propres à un type restent conceptuellement séparées et nulles pour les autres types. Pour une ligne `Episode`, selon la recommandation proposée, `SeriesWorkId` désigne une ligne `MediaWork` de type `Series`; la saison peut rester inconnue (`SeasonId = null`) sans perdre l'appartenance à la série. Une classification contrôlée et extensible (`Classification`, `MediaWorkClassification`) identifie notamment `Anime` sans ajouter de type structurel.

### MediaWorkRelation

Relation narrative ou éditoriale entre deux œuvres. Elle porte le type de relation et permet plusieurs relations pour une même œuvre.

Types de départ recommandés : `Adaptation`, `Prequel`, `Sequel`, `SpinOff`, `DerivedFilm`, `Related`, `Parent`, `Child`. La relation porte deux clés étrangères distinctes et obligatoires, `SourceWorkId` et `TargetWorkId`. Une œuvre ne peut pas être sa propre relation.

Une relation ne remplace pas l'appartenance d'un épisode à une saison. Une relation entre une série et un film dérivé reste possible.

### Season

Regroupe les épisodes d'une série. Elle porte un identifiant interne stable, `SeriesWorkId`, `SeasonKind` (`Normal`, `Special`, `Unknown`), `SeasonNumber` (numéro métier local et nullable uniquement pour `Unknown`), `SortOrder` (ordre de tri interne obligatoire), son titre et ses métadonnées.

La recommandation **PROPOSÉE** est de représenter les épisodes spéciaux couramment catalogués comme saison 0 par `SeasonKind = Special`, `SeasonNumber = 0`. Une seule saison spéciale 0 est permise par série. Une saison `Unknown` a `SeasonNumber = null` et son propre `SortOrder`; une seule est permise par série. Les numéros de saison des fournisseurs restent distincts dans `SeasonExternalId`.

### Episode

**Recommandation PROPOSÉE :** spécialisation de `MediaWork` toujours rattachée à une série par `SeriesWorkId` obligatoire et éventuellement à une saison connue par `SeasonId`. Elle peut exister sans fichier ou sans saison connue.

Conserver `SeriesWorkId` sur l'épisode permet le rattachement même lorsque la saison est inconnue. Lorsque `SeasonId` est renseigné, la clé composite `(SeriesWorkId, SeasonId)` garantit que la saison appartient à cette même série ; l'identifiant direct de série évite de déduire une relation obligatoire depuis une saison qui peut être absente.

Elle distingue au minimum : `Standard`, `Special`, `OAV`, `ONA` et d'autres types justifiés. Un arc narratif n'est pas un type d'épisode. Les numéros internes sont propres à MyVidLib ; les identifiants, numéros et regroupements externes sont conservés dans des références de catalogue par fournisseur.

`EpisodeGroup` / `EpisodeGroupMember` ne sont pas retenus : un fichier réunissant plusieurs épisodes est représenté par plusieurs `MediaFileAssignment`, et un épisode découpé est représenté par plusieurs associations portant des numéros de partie. Les regroupements propres à un fournisseur relèvent de `EpisodeExternalReference` et de ses mappings, pas de la hiérarchie locale des saisons ni des affectations de fichiers.

Cas de modélisation :

1. **Un fichier contient plusieurs épisodes complets** : le même `MediaFileId` est associé à chaque `Episode` par une `MediaFileAssignment` distincte, avec les limites `SegmentStartMs` et `SegmentEndMs` lorsque connues. Les bornes permettent le seek dans le fichier ; le numéro de lecture peut ordonner les associations du même fichier.
2. **Un épisode est découpé en plusieurs fichiers** : plusieurs `MediaFileAssignment` pointent vers le même épisode ; `PartNumber` identifie les parties et `PlaybackOrder` donne leur ordre de lecture dans la même œuvre/version. Chaque fichier conserve son identité physique.
3. **Les fournisseurs regroupent ou numérotent différemment** : chaque référence externe garde ses numéros et son identifiant ; `EpisodeExternalMapping` relie la référence à un ou plusieurs épisodes internes. Les correspondances restent distinctes des fichiers et peuvent demander validation humaine.

`EpisodeGroup` n'est donc pas nécessaire pour exprimer le contenu d'un fichier ni une numérotation fournisseur. Un futur besoin métier de regroupement éditorial interne autonome devrait être arbitré séparément, sans détourner cette relation de fichier.

### StoryArc

Regroupement narratif facultatif d'épisodes appartenant à une série. `StoryArc.SeriesWorkId` référence la série ; `EpisodeStoryArc` relie un arc à un ou plusieurs épisodes. Un épisode peut participer à plusieurs arcs. Un arc ne remplace jamais une saison et n'est pas un type d'épisode. Chaque association impose que l'épisode ait le même `SeriesWorkId` que l'arc.

### Classification

Taxonomie contrôlée et extensible (code stable, libellé et éventuellement parent) reliée à `MediaWork` par `MediaWorkClassification`. Le code `Anime` permet recherche, filtres, priorité de fournisseur et règles de catalogue sans recourir à une chaîne libre.

### MediaRoot

Racine de stockage configurée : dossier local, lecteur externe ou partage réseau. Elle possède un état unique `AvailabilityStatus` : `Accessible`, `Inaccessible`, `Disabled`, `Unknown`. `Disabled` représente une désactivation volontaire, pas une panne.

La désactivation ou l'archivage est préférable à la suppression définitive.

### MediaFile

Représentation d'un fichier physique unique dans une racine. Ses attributs comprennent le chemin relatif, le nom, l'extension, la taille, les dates source, les résultats FFprobe, `AvailabilityStatus`, `LastSeenAt` (dernière observation) et `LastVerifiedAt` (dernière vérification fiable de présence ou d'absence).

`RelativePath` préserve le chemin observé pour affichage ; `NormalizedRelativePath` est la clé canonique de comparaison et d'unicité décrite à la section 7.

Les états sont `Present`, `Missing`, `Unknown`. `Missing` n'est permis qu'après vérification fiable de l'absence dans une zone complètement énumérée d'une racine accessible.

La suppression d'une racine n'affecte ni l'enregistrement du fichier ni sa méthode d'archivage, sauf à avoir été explicitement confirmée par une politique distincte.

En V1, `MediaFile` représente les fichiers vidéo indexés ; les informations de pistes audio/sous-titres intégrées sont des données techniques du fichier. Les fichiers de sous-titres externes ne sont pas assimilés à des œuvres ou à des versions vidéo et restent hors périmètre de l'indexation initiale. **Recommandation :** les modéliser ultérieurement comme ressources annexes reliées au fichier vidéo (ou à sa version si réellement éditoriales), plutôt que comme `MediaFile` vidéo.

### MediaVersion

Version éditoriale appartenant à une œuvre : VF, VOSTFR, VO, remaster, édition alternative ou autre variante. La résolution, les codecs, les pistes et les propriétés mesurées appartiennent au fichier, pas à la version.

Une version n'est pas une œuvre indépendante. Elle peut être inconnue ou partiellement décrite. Plusieurs fichiers peuvent appartenir à une même version. Une langue/variante éditoriale stable appartient à la version ; les pistes réellement détectées appartiennent au fichier. Une langue propre à une association n'est conservée sur celle-ci que lorsqu'elle diffère de la version.

### MediaFileAssignment

Lien explicite plusieurs-à-plusieurs entre une œuvre et un fichier physique. `MediaWorkId` et `MediaFileId` sont obligatoires ; `MediaVersionId` est facultatif. Il contient uniquement les données propres à l'association :

- version concernée, si connue ;
- rôle contrôlé : `Primary`, `Alternative`, `Part`, `Segment`, `Other` ;
- `PlaybackOrder`, facultatif hors rôle `Segment` ;
- `PartNumber`, requis pour le rôle `Part` et réservé à une partie d'une œuvre découpée ;
- `SegmentStartMs` et `SegmentEndMs`, facultatifs ;
- langue ou variante seulement si elle n'est pas déjà définie par la version ;
- date d'association ;
- `MatchStatus` de correspondance et score éventuel ;
- `IsArchived`.

Le statut de rapprochement est contrôlé : `Proposed`, `Confirmed`, `Rejected`, ou `Manual`. **Règles PROPOSÉES :** `SegmentStartMs` et `SegmentEndMs` sont renseignés ensemble ou restent tous deux nuls ; le début est positif ou nul et la fin est strictement supérieure au début. Si la durée du fichier est connue, la fin ne peut la dépasser. La cohérence des segments entre eux (chevauchement ou limites de durée) est également vérifiée avant validation de l'association.

Quand `MediaVersionId` est renseigné, une contrainte référentielle composite doit garantir que la version appartient à la même œuvre que l'association : le couple `(MediaWorkId, MediaVersionId)` de l'association référence `(MediaWorkId, Id)` de `MediaVersion`. Une simple validation de l'interface ne suffit pas.

Portée de l'ordre et des parties :

- `Segment` : `PlaybackOrder` est obligatoire et positif, relatif aux segments du même `MediaFileId`; si les bornes sont présentes, l'ordre doit être compatible avec `SegmentStartMs`. Il ne remplace pas les limites nécessaires à une reprise précise.
- `Part` : `PartNumber` est obligatoire et positif ; il identifie la partie et détermine l'ordre par défaut pour le même `(MediaWorkId, MediaVersionId)`. `PlaybackOrder`, s'il est renseigné, est unique dans cette portée et doit être cohérent avec `PartNumber`.
- `Alternative` : représente un autre fichier complet pour la même œuvre/version ; il n'est pas une partie et ne nécessite ni segment ni ordre de séquence.
- `Primary` : au plus une association active par œuvre/version ; les autres fichiers complets utilisent `Alternative`.
- Une association de fichier complet (`Primary` ou `Alternative`) n'a pas de bornes de segment. Si un fichier contient plusieurs œuvres et que les limites de ses segments sont inconnues, l'association peut être enregistrée comme proposée, mais la lecture segmentée/reprise ne peut pas supposer un offset.

## 5. Diagramme catalogue et fichiers

```mermaid
erDiagram
  MEDIA_WORK ||--o{ MEDIA_WORK : episode_series_work_id
  MEDIA_WORK ||--o{ SEASON : series_work_id
  SEASON o|--o{ MEDIA_WORK : episode_season_id_when_episode
  MEDIA_WORK ||--o{ STORY_ARC : series_work_id
  STORY_ARC ||--o{ EPISODE_STORY_ARC : arc_id
  MEDIA_WORK ||--o{ EPISODE_STORY_ARC : episode_work_id
  PROVIDER ||--o{ MEDIA_WORK_EXTERNAL_ID : provides
  MEDIA_WORK ||--o{ MEDIA_WORK_EXTERNAL_ID : subject
  PROVIDER ||--o{ SEASON_EXTERNAL_ID : provides
  SEASON ||--o{ SEASON_EXTERNAL_ID : subject
  PROVIDER ||--o{ EPISODE_EXTERNAL_REFERENCE : provides
  EPISODE_EXTERNAL_REFERENCE ||--o{ EPISODE_EXTERNAL_MAPPING : maps_to
  MEDIA_WORK ||--o{ EPISODE_EXTERNAL_MAPPING : episode_work_id
  MEDIA_WORK ||--o{ MEDIA_WORK_METADATA : candidates
  SEASON ||--o{ SEASON_METADATA : candidates
  PROVIDER o|--o{ MEDIA_WORK_METADATA : provenance
  PROVIDER o|--o{ SEASON_METADATA : provenance
  MEDIA_WORK ||--o{ MEDIA_VERSION : has
  MEDIA_WORK ||--o{ MEDIA_FILE_ASSIGNMENT : work_id
  MEDIA_VERSION o|--o{ MEDIA_FILE_ASSIGNMENT : matching_work_id_version_id
  MEDIA_FILE ||--o{ MEDIA_FILE_ASSIGNMENT : participates
  MEDIA_ROOT ||--o{ MEDIA_FILE : contains
  MEDIA_FILE ||--o{ MEDIA_FINGERPRINT : fingerprinted
  MEDIA_WORK ||--o{ MEDIA_WORK_RELATION : source_work_id
  MEDIA_WORK ||--o{ MEDIA_WORK_RELATION : target_work_id
```

`MEDIA_WORK` porte le discriminateur `WorkType`; les détails de Movie, Series et Episode sont des colonnes conditionnelles de cette même table TPH.
Dans ces diagrammes, les lignes vers `MEDIA_WORK` indiquées `episode_*` ne concernent que les lignes dont `WorkType = Episode`; les lignes `series_*` ne concernent que `WorkType = Series`. Elles sont contraintes par les clés composites et invariants décrits ci-dessous.

## 6. Relations et cardinalités

- `MediaRoot` 0,N `MediaFile` ; chaque fichier appartient à une racine
- `MediaWork` 0,N `MediaVersion` ; chaque version appartient à une œuvre
- `MediaWork` 0,N `MediaFileAssignment`
- `MediaFile` 0,N `MediaFileAssignment`
- `MediaFileAssignment` 0,1 `MediaVersion`
- `MediaWork` de type `Series` 0,N `Season`
- **PROPOSÉ** : `MediaWork` de type `Series` 0,N `Episode` ; chaque `Episode` appartient à exactement une série
- **PROPOSÉ** : `Season` 0,N épisodes ; chaque `Episode` appartient à 0 ou 1 saison de sa série
- `EpisodeExternalReference` 0,N `EpisodeExternalMapping` (une référence proposée peut rester non rapprochée) ; chaque mapping vise exactement un épisode interne
- `MediaWork` de type `Episode` 0,N `EpisodeExternalMapping`
- `MediaWork` 0,N `MediaWorkRelation` comme source ou cible
- `MediaFile` peut être associé à plusieurs épisodes par plusieurs `MediaFileAssignment`
- `MediaFile` 0,N `MediaFingerprint` ; chaque empreinte appartient à un fichier unique
- `StoryArc` 1,N `EpisodeStoryArc` ; `Episode` 0,N `EpisodeStoryArc`
- `Provider` 0,N identifiants externes et valeurs candidates ; chaque identifiant a exactement un fournisseur
- `MediaWork` de type `Movie`/`Series` 0,N `MediaWorkExternalId`
- `Season` 0,N `SeasonExternalId`
- `MediaWork` (y compris les épisodes) et `Season` 0,N candidats de métadonnées dans leur table typée respective

La relation fichier/œuvre est donc toujours plusieurs-à-plusieurs via `MediaFileAssignment`. Un fichier non identifié existe sans association ; une association ne peut pas exister sans fichier ni œuvre identifiés.

## 7. Chemins physiques

La représentation recommandée sépare :

- `MediaRoot.AbsolutePath` : chemin physique configuré de la racine ;
- `MediaFile.RelativePath` : forme conservée pour l'affichage ;
- `MediaFile.NormalizedRelativePath` : forme canonique produite par l'application et utilisée pour la comparaison et l'unicité ;
- chemin absolu du fichier : calculé à la demande à partir de la racine et du chemin relatif.

`MediaFile.AbsolutePath` ne doit pas être dupliqué comme colonne persistante. Cette approche évite les incohérences lors d'un changement de lettre de lecteur, du montage UNC ou du déplacement autorisé de la racine. L'application fournit une fonction centralisée de résolution et normalisation.

Une base peut conserver exceptionnellement une copie observée du chemin absolu à des fins de diagnostic, mais elle ne doit pas être utilisée comme identité ni chemin calculé de référence.

L'unicité physique est `(MediaRootId, NormalizedRelativePath)`. `RelativePath` ne participe pas directement à la comparaison unique.

### Normalisation Windows

- Pour un fichier, convertir `/` en `\`, réduire les séparateurs répétés et retirer les segments `.` ; rejeter un chemin vide, absolu/enraciné, tout segment `..`, une syntaxe de lecteur/flux alternatif (`:`), les caractères interdits par Windows, les noms de périphériques réservés et les composants terminant par un point ou un espace.
- Préserver la casse de `RelativePath` pour affichage. Pour `NormalizedRelativePath`, la recommandation précise est de séparer sur `\`, appliquer `ToUpperInvariant` à chaque composant, puis reconstruire la clé avec `\`; conserver les points Unicode tels quels (ne pas appliquer NFC/NFD). Stocker et indexer cette clé avec comparaison binaire. Ne pas s'appuyer sur `COLLATE NOCASE`, limité à l'ASCII dans SQLite.
- Un chemin absolu local est canonisé en tenant compte de la lettre de lecteur ; UNC conserve son préfixe `\\serveur\partage` et normalise les segments relatifs selon la même règle. Un changement de lettre ou de point de montage met à jour `MediaRoot.AbsolutePath` sans changer son `Id` ; il ne réécrit pas les identifiants de fichiers avant une réconciliation.
- Le chemin configuré conserve ses séparateurs canoniques Windows (`\`). Les séparateurs finaux ne sont retirés qu'aux chemins de fichiers ; préserver les formes racines comme `C:\` et `\\serveur\partage\`.
- Deux chemins textuellement différents peuvent être des alias (lettre mappée/UNC, jonction, lien symbolique). Ne pas fusionner automatiquement les racines ni prétendre reconnaître l'identité physique par la seule chaîne du chemin.
- La sensibilité à la casse activée sur un dossier Windows peut contredire la comparaison Windows usuelle. Recommandation V1 : détecter et signaler ce cas plutôt que fusionner silencieusement des chemins distincts ; la prise en charge complète des répertoires sensibles à la casse reste à arbitrer.

La règle `ToUpperInvariant` est **PROPOSÉE** ; sa conformité aux comparaisons réelles Windows pour les cas Unicode doit être validée sur les plateformes cibles. La fonction de normalisation applicative, l'index SQLite binaire et les comparaisons de chemins doivent partager la même règle et être vérifiés sur les cas Unicode, UNC, lecteurs mappés et racines. Un index unique sur la seule forme affichée ne suffit pas.

Le changement de chemin physique d'une racine conserve l'identité interne de `MediaRoot`. Le chemin précédent peut être historisé pour diagnostic ; un changement ne réécrit pas les identifiants de fichiers avant une réconciliation réussie.

## 8. Disponibilité et indexation

`MediaRoot` et `MediaFile` ont des états distincts :

| Objet | États conceptuels |
|---|---|
| Racine | `Accessible`, `Inaccessible`, `Disabled`, `Unknown` |
| Fichier | Present, Missing, Unknown |

Règles impératives :

- une racine inaccessible ne modifie aucune disponibilité de ses fichiers ;
- `Disabled` signifie que l'indexation est volontairement suspendue ; cet état n'indique pas une panne et ne change aucun état de fichier ;
- `Unknown` signifie qu'aucune vérification fiable récente n'est disponible ;
- `MediaFile.AvailabilityStatus` représente la dernière disponibilité établie de façon fiable. La possibilité de la vérifier maintenant est déterminée par l'état courant de la racine et la couverture du dernier scan, sans effacer cette dernière observation ;
- une exception FFprobe est une erreur d'analyse du fichier, pas une preuve d'absence ;
- les événements du système de fichiers peuvent suggérer une modification, mais ne remplacent pas la réconciliation par scan.

### Scan

`MediaScan` représente une exécution avec son type, son état, ses horodatages, ses compteurs et son identifiant de corrélation. Ses états sont exactement `Completed`, `Partial`, `Cancelled`, `Failed`. `MediaScanScope` (nouvelle trace de couverture nécessaire à la réconciliation) conserve pour chaque répertoire concerné son chemin relatif normalisé et le résultat de l'énumération : `Completed`, `Inaccessible`, `Failed`, `NotReached`.

Une portée contient au minimum `MediaScanId`, `RelativeDirectoryPath`, `EnumerationStatus` et ses horodatages. Le chemin vide désigne la racine. Une portée `Completed` indique que les entrées immédiates de ce dossier ont été énumérées ; elle ne prétend pas que tous ses descendants sont complets. L'unicité `(MediaScanId, RelativeDirectoryPath)` empêche de dupliquer le résultat d'une portée dans un scan.

- `Completed` : l'énumération de toutes les zones attendues a terminé sans erreur d'accès compromettant la couverture. Les erreurs FFprobe peuvent exister et sont comptées séparément.
- `Partial` : une partie des zones a été énumérée, mais au moins une zone (racine ou sous-dossier) n'a pas pu l'être ; seules les zones complètement énumérées peuvent contribuer à confirmer une absence.
- `Cancelled` : annulation demandée ; la session de scan est persistée avec ce statut. Les zones terminées et marquées complètes avant l'annulation peuvent être réconciliées ; aucune absence n'est conclue dans les zones incomplètes ou `NotReached`.
- `Failed` : l'opération n'a pas produit une énumération exploitable.

Une portée `Completed` confirme que les entrées immédiates de ce répertoire ont été énumérées sans erreur d'accès, d'autorisation ou d'I/O. Le parcours global est `Completed` seulement si toutes les portées attendues sont complètes. Un échec de sous-dossier marque sa portée `Inaccessible` ou `Failed` et exclut son contenu du périmètre de réconciliation, mais n'invalide pas les autres portées complètes.

Un fichier observé prend `Present` et met à jour `LastSeenAt`. `LastVerifiedAt` change uniquement lorsqu'une vérification fiable établit sa présence ou son absence dans une portée complète. L'absence peut faire passer à `Missing` uniquement si la racine était accessible et le répertoire contenant le fichier énuméré avec succès. Une erreur FFprobe ne compromet pas l'observation de présence ; une erreur d'accès au sous-dossier n'autorise aucun marquage `Missing` dans celui-ci.

Lorsqu'une portée est inaccessible, échoue ou n'est pas atteinte, les fichiers déjà indexés dans cette portée conservent leur état, `LastSeenAt` et `LastVerifiedAt`. Leur vérifiabilité actuelle est inconnue, mais la dernière disponibilité connue n'est pas effacée ; la possibilité de vérifier maintenant est dérivée de l'état de la racine et de la portée de scan la plus récente. `Unknown` est utilisé à la création/import d'un enregistrement tant qu'aucune preuve fiable de présence ou d'absence n'existe. Une interruption, indisponibilité temporaire ou modification du chemin de racine ne fait jamais passer automatiquement un état déjà connu à `Unknown`.

`MediaScanError` conserve les erreurs par fichier ou racine : code technique, message sûr, phase, horodatage et éventuel chemin relatif. Les erreurs de portée sont aussi reflétées par `MediaScanScope`, afin de connaître explicitement les zones non vérifiées.

Le scan doit permettre l'annulation. Le statut global `Cancelled` est persisté ; les observations réellement faites sont conservées et les états d'absence ne sont recalculés que dans les portées complètement énumérées.

## 9. Empreintes et doublons

L'empreinte doit être facultative, différée et calculée sur demande ou par lot.

Stratégie recommandée :

1. identification légère par taille et dates locales ;
2. empreinte partielle optionnelle sur des segments du fichier ;
3. hash complet différé, demandé explicitement ou exécuté sur des candidats ;
4. confirmation de doublon uniquement avec empreinte pleine et taille compatibles ;
5. conservation séparée de l'algorithme, du résultat, de la date et du statut du calcul.

Cette combinaison évite une empreinte complète systématique sur tous les fichiers lors du premier scan. Elle reste capable de proposer une association à un fichier déplacé.

Les informations techniques de rapprochement ne doivent pas écraser une association existante : elles produisent une proposition vérifiable.

### MediaFingerprint

`MediaFingerprint` est une empreinte calculée d'un `MediaFile`, distincte de sa taille. Elle contient `MediaFileId`, `Algorithm`, `FingerprintKind` (`Partial` ou `Full`), `HashValue`, `CalculatedAt` et `CalculationStatus` (`Pending`, `Succeeded`, `Failed`). La valeur de hash est absente tant que le calcul n'a pas réussi.

Un fichier peut avoir zéro à plusieurs empreintes, par exemple plusieurs algorithmes ou une empreinte partielle puis complète. Une même empreinte ne doit pas être dupliquée pour le même fichier, algorithme et type ; plusieurs empreintes de types/algorithmes différents restent permises. La taille est un attribut de `MediaFile` et un critère de préfiltrage/comparaison, jamais une empreinte. Les empreintes restent facultatives et leur calcul est différé.

## 10. Fournisseurs et identifiants externes

`Provider` décrit une source externe : code, nom, activation, priorité configurable par classification/type de contenu, paramètres non secrets et état du dernier accès. Les fournisseurs envisagés sont TMDB, Animeka, Anime-Sama et IMDb, uniquement selon les accès et données officiellement autorisés.

La priorité n'est pas fixée globalement à TMDB. Animeka reste un candidat prioritaire pour le catalogue anime selon l'arbitrage configurable ; Anime-Sama peut être une source complémentaire. TMDB est envisagé pour le catalogue général et IMDb principalement comme identifiant/référence tant qu'une intégration de données plus large n'est pas autorisée et validée.

Pour garantir l'intégrité, les identifiants sont modélisés par tables dédiées :

- `MediaWorkExternalId`
- `SeasonExternalId`
- `EpisodeExternalReference` (référence externe de catalogue, avec table de correspondance vers un ou plusieurs épisodes internes)

`MediaWorkExternalId` référence un `MediaWork` de type `Movie` ou `Series` ; `SeasonExternalId` référence une `Season` et conserve, entre autres, `ProviderSeasonNumber` sans le confondre avec le numéro local. `EpisodeExternalReference` référence un `Provider`, et ses liens vers les épisodes passent par `EpisodeExternalMapping`. Ces tables typées et leurs mappings conservent les informations de rapprochement pertinentes :

- fournisseur ;
- identifiant externe ;
- type d'entité fournisseur ;
- statut (`Proposed`, `Confirmed`, `Rejected`, `Manual`) ;
- confiance éventuelle ;
- date de création ;
- date de validation ;
- utilisateur ayant validé ;
- date d'archivage éventuelle.

`EpisodeExternalReference` conserve le fournisseur, l'identifiant externe de catalogue et les numéros de saison/épisode propres à ce fournisseur. `EpisodeExternalMapping` relie cette référence à zéro ou plusieurs `MediaWork` de type `Episode`, et inversement un épisode peut être relié à plusieurs références fournisseurs. Une référence non rapprochée reste ainsi représentable. Cela permet qu'un catalogue désigne un regroupement tandis qu'un autre distingue ses épisodes, sans confondre numéros internes et externes.

Pour une référence d'épisode, le statut de rapprochement, la confiance éventuelle, les dates de validation et l'utilisateur validateur appartiennent au mapping. Les identifiants d'œuvre/saison portent ces mêmes informations sur leur propre ligne typée.

La relation externe ne doit jamais avoir une combinaison `(SubjectType, SubjectId)` polymorphique. Les tables épisode sont typées ; une référence d'épisode ne peut cibler qu'une œuvre dont `WorkType` vaut `Episode`.

Une combinaison `(ProviderId, ExternalKind, ProviderValue)` est unique au sein du type d'entité adéquat. Une combinaison `(ProviderId, SubjectId)` est limitée selon la règle métier pour éviter plusieurs identifiants actifs du même type sur une même entité. Les numéros externes ne sont jamais indexés comme numéros internes uniques.

## 11. Métadonnées

Les tables `MediaWorkMetadata` et `SeasonMetadata` préservent les clés étrangères typées. `MediaWorkMetadata` s'applique à toutes les œuvres de la table TPH, y compris les épisodes ; `SeasonMetadata` s'applique aux saisons. Chaque ligne représente une valeur candidate indépendante pour un champ et une langue ; aucune table d'historique supplémentaire n'est nécessaire tant que les candidats remplacés restent conservés.

Chaque candidat conserve :

- nom de champ ;
- code langue/locale non nul (`und` si inconnu) ;
- valeur brute reçue (`RawValue`, nulle pour une valeur saisie manuellement) ;
- valeur candidate exploitable (`CandidateValue`) ;
- provenance (`Provider`, `Manual`, `Local`) et fournisseur éventuel ;
- validation (`Proposed`, `Validated`, `Rejected`), utilisateur et date de validation ;
- indicateur `IsSelected` et date de sélection ;
- date de récupération (`FetchedAt`) ;
- date d'expiration ;
- date de création.

Chaque réception fournisseur crée un candidat distinct avec valeur brute, valeur interprétée, fournisseur, langue et dates. Les corrections manuelles créent de nouvelles lignes candidates `Manual`, validées et sélectionnées ; elles ne modifient ni n'effacent les anciennes valeurs fournisseurs. Le catalogue conserve ainsi l'historique des changements sans entité d'historique séparée.

Le mode de sélection proposé :

- la sélection est faite séparément par sujet, champ et langue ;
- une valeur manuelle validée peut être sélectionnée prioritairement jusqu'à une nouvelle validation humaine ;
- sinon, un candidat fournisseur validé/sélectionné selon la priorité configurable peut servir de référence ;
- les valeurs candidates concurrentes restent conservées et non sélectionnées ;
- une valeur vide, expirée ou rejetée ne remplace pas silencieusement la valeur actuellement sélectionnée.

Pour chaque sujet typé, champ et code langue/locale, au maximum une ligne peut avoir `IsSelected = true`. Une unicité partielle SQLite est recommandée séparément : `(MediaWorkId, FieldName, LocaleCode) WHERE IsSelected = 1` et `(SeasonId, FieldName, LocaleCode) WHERE IsSelected = 1`. Le code langue n'étant pas nul (utiliser `und`), la contrainte n'est pas contournée par la sémantique SQLite des valeurs `NULL`.

La provenance couvre à la fois les textes, traductions, identifiants, images et données techniques locales. Les licences ou conditions d'accès doivent être conservables, même si les champs exacts restent à préciser.

## 12. Utilisateurs

`User` porte l'identité locale, le nom normalisé, le rôle, l'état actif, les informations de connexion et les dates de création/modification.

Les rôles initiaux sont `Administrator` et `User`. Le catalogue reste partagé ; les préférences individuelles et suivis restent isolés.

Les utilisateurs peuvent être désactivés ou archivés. Anonymisation ou conservation de l'historique relève d'une décision de gestion des données explicitement configurée.

## 13. Suivi de lecture

### WatchProgress

Progression courante unique par utilisateur et œuvre. Elle porte :

- utilisateur ;
- œuvre (`MediaWork`) ;
- `PositionMs` relative à l'œuvre lue ;
- `DurationMs` de référence ;
- état `Unwatched`, `InProgress`, `Watched` ;
- `LastMediaFileId` et, facultativement, `LastMediaFileAssignmentId` ;
- date de mise à jour ;
- date du dernier visionnage ;
- source `Manual` ou `Player`.

Unicité recommandée : `(UserId, MediaWorkId)`.

L'unité milliseconde et une position relative à l'œuvre sont **PROPOSÉES**. Pour un épisode, la position est relative au début de l'épisode, non au début du fichier. Si l'association a `SegmentStartMs`, la position absolue du fichier vaut `SegmentStartMs + PositionMs`; la fin effective est bornée par `SegmentEndMs` si défini. Sans limites de segment, un offset nul n'est valable que lorsque l'association représente le fichier entier pour cette œuvre. Chaque épisode possède sa propre progression. Une progression de série (si retenue) est une information d'ensemble séparée : elle ne remplace ni ne résume par écrasement les progressions d'épisodes.

Pour une progression associée à la `Series` abstraite, `PositionMs` et `DurationMs` restent nuls ; son état est un indicateur d'ensemble non substitutif. Les progressions `Movie` et `Episode` peuvent porter position et durée.

Le dernier fichier utilisé est facultatif. L'association peut également être conservée pour reprendre dans le bon segment/version. Un fichier ou une association peuvent devenir `Missing` ou archivés sans supprimer la progression ; aucune suppression en cascade n'est permise. Lorsque l'association est renseignée, `MediaWorkId`, `LastMediaFileId` et `LastMediaFileAssignmentId` doivent identifier la même association.

### WatchSession

`WatchSession` est l'enregistrement principal de l'historique consultable : une ligne par session de lecture, commencée par le lecteur ou déclarée manuellement. Elle porte début, fin, état, dernier checkpoint, `PositionMs`/`DurationMs` finales si connues, source, utilisateur et œuvre. La position reste relative à l'œuvre (épisode ou film), et non au fichier. Les états sont `InProgress`, `Completed`, `Interrupted` et `Error`. `Error` est réservé à une session qui n'a pas pu se terminer normalement pour une erreur de lecture ; les erreurs techniques détaillées restent dans les journaux.

Une session référence facultativement `MediaFileId` et `MediaFileAssignmentId`. Si l'association est renseignée, elle doit correspondre à la même œuvre et au même fichier. Ses références restent conservées si le fichier ou l'association devient `Missing` ou archivé ; aucune suppression en cascade n'est permise.

Une session `InProgress` conserve son dernier checkpoint et peut rester ouverte après un arrêt brutal de l'application. Au démarrage, les sessions non terminées sont réconciliées ; leur passage à `Interrupted` dépend d'une règle de péremption/confirmation encore **À ARBITRER**. Les mises à jour périodiques modifient la progression et le checkpoint de la session existante ; elles ne créent pas de nouvelles lignes historiques pour les pauses ou sauvegardes de position.

Une lecture manuelle peut produire une session sans fichier, position ou durée connus. Les sessions de lecture concernent les œuvres lisibles (`Movie` et `Episode`), pas la `Series` abstraite.

**Règle PROPOSÉE :** une session `InProgress` n'a pas encore `EndedAt`; les états `Completed`, `Interrupted` et `Error` sont terminaux et portent une date de fin. `LastCheckpointAt` indique la dernière persistance d'état, sans générer une ligne d'historique additionnelle.

```mermaid
erDiagram
  USER ||--o{ WATCH_PROGRESS : owns
  USER ||--o{ WATCH_SESSION : creates
  MEDIA_WORK ||--o{ WATCH_PROGRESS : target
  MEDIA_WORK ||--o{ WATCH_SESSION : target
  MEDIA_FILE o|--o{ WATCH_SESSION : used_file
  MEDIA_FILE o|--o{ WATCH_PROGRESS : last_file
  MEDIA_FILE_ASSIGNMENT o|--o{ WATCH_SESSION : used_assignment
  MEDIA_FILE_ASSIGNMENT o|--o{ WATCH_PROGRESS : last_assignment
```

## 14. Suppression et archivage

Les suppressions physiques ne sont jamais effectuées par défaut.

| Entité | Comportement recommandé |
|---|---|
| MediaWork | archivage logique ; conserver associations, identifiants, métadonnées et historique selon la politique |
| MediaRoot | désactivation ou archivage ; ne jamais supprimer les `MediaFile` physiquement indexés |
| User | désactivation ou archivage ; anonymiser ou conserver selon règle validée |
| Provider/Source | désactivation ; conserver les données récupérées hors ligne |
| MediaFile | conserver l'enregistrement ; `Missing` seulement après scan réussi |
| MediaFileAssignment | archivage ou rejet ; ne pas supprimer l'œuvre ou le fichier |

Aucune suppression d'œuvre ne supprime les fichiers physiques. Une suppression logique peut archiver les associations pour éviter des données orphelines.

## 15. Paramètres et secrets

Les réglages sont séparés en quatre classes :

- `ApplicationSettings` : paramètres globaux applicatifs ;
- `UserSettings` : préférences individuelles ;
- `ProviderSettings` : activation, priorités, quotas, paramètres non secrets ;
- coffre de secrets : clés API, jetons et informations d'identification.

Un secret ne doit jamais être stocké en clair dans une table générique `ApplicationSettings`.

Sous Windows, une solution adaptée est DPAPI ou le gestionnaire de secrets Windows. Les emplacements chiffrés restent sous `%LOCALAPPDATA%\MyVidLib`, notamment `Data`, `Config`, `Cache`, `Logs` et `Backups`.

## 16. Contraintes d'intégrité

- `MediaWork.Id` est interne et obligatoire.
- `WorkType` est requis et limité à `Movie`, `Series`, `Episode` ; colonnes conditionnelles cohérentes avec ce type ;
- les contraintes conditionnelles de TPH limitent les colonnes Movie, Series et Episode au `WorkType` correspondant ;
- **PROPOSÉ** : chaque `Episode.SeriesWorkId` est obligatoire et référence un `MediaWork` de type `Series` ; `Episode.SeasonId` est facultatif ;
- `Season` a une clé candidate `(SeriesWorkId, Id)` ; si `Episode.SeasonId` est renseigné, la paire `(Episode.SeriesWorkId, Episode.SeasonId)` référence `(Season.SeriesWorkId, Season.Id)`, garantissant la même série ;
- les références typées de fournisseur et d'arc vers épisode ne peuvent référencer qu'un `MediaWork` de type `Episode` ;
- une `StoryArc` référence une seule série ; chaque `EpisodeStoryArc` contient le `SeriesWorkId` et référence par deux clés composites l'arc et l'épisode de cette même série ;
- chaque `EpisodeExternalMapping` référence exactement une `EpisodeExternalReference` et une œuvre de type `Episode` ;
- `MediaWorkExternalId` ne cible que des œuvres `Movie`/`Series` ; `SeasonExternalId` référence une saison et `EpisodeExternalMapping` une œuvre `Episode` ;
- une relation de travail possède `SourceWorkId` et `TargetWorkId` distincts et ne peut pas être dupliquée pour le même couple source/cible/type ;
- une œuvre ne peut pas être sa propre relation ;
- `(MediaRootId, NormalizedRelativePath)` est unique ; la valeur normalisée est calculée par la même fonction applicative pour tous les accès ;
- `MediaFileAssignment.MediaWorkId` et `.MediaFileId` sont obligatoires ; `.MediaVersionId` est facultatif ;
- `MediaFileAssignment(MediaWorkId, MediaVersionId)` référence une clé candidate `MediaVersion(MediaWorkId, Id)` afin de garantir en base l'appartenance de la version à la même œuvre ;
- `MediaFileAssignment` expose la clé candidate `(MediaWorkId, MediaFileId, Id)` pour que les références de reprise garantissent la cohérence œuvre/fichier/association ;
- `SegmentStartMs` et `SegmentEndMs` sont tous deux nuls ou tous deux renseignés ; `SegmentStartMs >= 0`, `SegmentEndMs > SegmentStartMs` ;
- le segment ne dépasse pas `MediaFile.DurationMs` si la durée est connue ; cette comparaison intertable est vérifiée par invariant applicatif ;
- le rôle et le statut de rapprochement proviennent de valeurs contrôlées ; `Part` exige `PartNumber > 0`, `Segment` exige `PlaybackOrder > 0`, et un `PlaybackOrder` renseigné est positif ;
- les données propres aux rôles sont exclusives : `Part` ne porte pas de bornes de segment, `Segment` ne porte pas de `PartNumber`, et `Primary`/`Alternative` représentent des fichiers complets sans bornes ni numéro de partie ;
- une seule association active existe par paire `(MediaWorkId, MediaFileId)`, indépendamment du rôle/version ; une association `Rejected` ou archivée ne compte pas comme active. Un même fichier peut ainsi être associé à plusieurs œuvres/épisodes, mais pas deux fois à la même œuvre ;
- pour `Part`, `(MediaWorkId, COALESCE(MediaVersionId, sentinel), PartNumber)` et, si renseigné, `(MediaWorkId, COALESCE(MediaVersionId, sentinel), PlaybackOrder)` sont uniques parmi les associations actives ;
- pour `Segment`, `(MediaFileId, PlaybackOrder)` est unique parmi les associations actives ; les segments confirmés d'un fichier ne se chevauchent pas, sauf arbitrage explicite ;
- pour `Primary`, `(MediaWorkId, COALESCE(MediaVersionId, sentinel))` est unique parmi les associations actives ;
- la taille d'un fichier ne constitue pas une empreinte ;
- une racine désactivée, inaccessible ou inconnue ne peut pas faire passer ses fichiers à `Missing` ;
- seul un scan dont la portée du répertoire contenant le fichier est complètement énumérée peut vérifier l'absence ; `LastSeenAt` et `LastVerifiedAt` ont des significations séparées ;
- les empreintes référencent un fichier et sont uniques par fichier, algorithme et type d'empreinte ;
- pour une saison : `Normal` a un numéro métier positif ; `Special` a le numéro 0 ; `Unknown` a `SeasonNumber = null` ;
- une série possède au plus une saison `Unknown` et au plus une saison `Special` 0 ;
- les numéros de saison internes non nuls sont uniques par série ; `SortOrder` est unique par série ;
- les numéros d'épisode internes non nuls sont uniques dans une saison connue ; pour les épisodes sans saison, ils sont uniques par série ;
- les identifiants externes et numéros fournisseurs sont uniques dans leur portée fournisseur sans imposer l'égalité aux numéros internes ;
- `WatchProgress` est unique par utilisateur et œuvre ;
- `WatchProgress` et `WatchSession` peuvent référencer `MediaFileAssignment` ; si renseigné, le triplet `(MediaWorkId, MediaFileId, MediaFileAssignmentId)` doit désigner la même association ;
- les sessions de visionnage ne ciblent que des `MediaWork` de type `Movie` ou `Episode` ; pour `Series`, `PositionMs` et `DurationMs` restent nuls ;
- `WatchProgress.PositionMs` est non négatif ; `DurationMs` est positif lorsqu'il est connu ; une session `InProgress` n'a pas de fin, une session terminale en a une ;
- `WatchSession` référence obligatoirement l'utilisateur et l'œuvre ; ses références au fichier/association sont facultatives et ne cascade pas ;
- chaque table de candidats de métadonnées autorise au plus un `IsSelected = true` par identifiant de sujet, `FieldName` et `LocaleCode` ;
- les sessions en cours peuvent exister en plusieurs exemplaires si la politique de concurrence multi-appareils le permet ; aucune unicité active n'est supposée avant arbitrage ;
- les identifiants externes restent associés à une entité fortement typée ;
- les métadonnées restent associées à une entité fortement typée ;
- les suppressions physiques sont exclues des opérations normales.

### Limites SQLite / EF Core

SQLite et EF Core peuvent garantir les clés étrangères, les unicités, les `CHECK` locaux à une ligne et les clés composites vers la version, la série, la saison et l'association. Les clés candidates doivent être explicites. Les index uniques partiels (par exemple `WHERE SeasonNumber IS NOT NULL`) sont disponibles dans SQLite ; la configuration EF Core des filtres/index d'expression doit être vérifiée.

La recommandation TPH place `WorkType` et toutes les colonnes spécialisées dans `MediaWork`. Des `CHECK` peuvent exiger les propriétés conditionnelles appropriées selon `WorkType`; les relations typées peuvent utiliser des clés candidates composites `(Id, WorkType)` avec un type attendu. Les relations entre saisons, épisodes et arcs exigent des clés composites tenant compte de la série parente. Cette conception doit être validée par un prototype EF Core/SQLite ultérieur avant toute migration, sans remettre en cause le choix TPH.

Les contraintes interlignes et intertables non directement couvertes par clé composite/unique (segment ne dépassant pas la durée, chevauchement de segments, contrôle du type des colonnes TPH, transition `Missing` basée sur `MediaScanScope`, cohérence des horodatages et validations humaines) doivent être protégées par des invariants applicatifs testés. Un scan `Cancelled` est persisté ; seules les portées `Completed` peuvent réconcilier l'absence. Aucun invariant ne doit être laissé à l'interface seule.

SQLite traite plusieurs `NULL` comme des valeurs distinctes dans un index unique. Les index actifs sur une association contenant une version facultative doivent donc normaliser cette clé (expression `COALESCE` et valeur réservée interdite aux identifiants réels) ou être complétés par un invariant applicatif testable. Les numéros et limites facultatifs peuvent utiliser des sentinelles exclues par `CHECK`; les index partiels restent préférables pour les contraintes de type/état. Ne pas prétendre qu'un index unique ordinaire couvre les lignes comportant `NULL`.

## 17. Index recommandés

Index obligatoires ou fortement recommandés :

- `MediaRoot(AvailabilityStatus)`
- `MediaFile(MediaRootId, NormalizedRelativePath)` unique (index binaire)
- `MediaFile(AvailabilityStatus, LastVerifiedAt)`
- `MediaFile(SizeBytes)`
- `MediaFingerprint(MediaFileId, Algorithm, FingerprintKind)` unique
- `MediaFingerprint(Algorithm, FingerprintKind, HashValue)` pour les candidats ; inclure `MediaFile.SizeBytes` dans la vérification
- `MediaFileAssignment(MediaWorkId, MediaVersionId, IsArchived)`
- `MediaFileAssignment(MediaFileId, IsArchived)`
- `MediaFileAssignment(MediaWorkId, MediaFileId)` unique partiel avec `IsArchived = 0 AND MatchStatus <> 'Rejected'` ; interdit de dupliquer le lien actif quel que soit son rôle/version
- `MediaFileAssignment(MediaWorkId, COALESCE(MediaVersionId, sentinel), PartNumber)` unique partiel pour `Role = 'Part'` et associations actives (`PartNumber` est non nul par règle de rôle)
- `MediaFileAssignment(MediaWorkId, COALESCE(MediaVersionId, sentinel), PlaybackOrder)` unique partiel pour `Role = 'Part' AND PlaybackOrder IS NOT NULL` et associations actives
- `MediaFileAssignment(MediaFileId, PlaybackOrder)` unique partiel pour `Role = 'Segment'` et associations actives (`PlaybackOrder` est non nul par règle de rôle)
- `MediaFileAssignment(MediaWorkId, COALESCE(MediaVersionId, sentinel))` unique partiel pour `Role = 'Primary'` et associations actives
- `Season(SeriesWorkId, SeasonNumber)` unique partiel lorsque `SeasonNumber IS NOT NULL`
- `Season(SeriesWorkId)` unique partiel lorsque `SeasonKind = Unknown`
- `Season(SeriesWorkId, SortOrder)` unique
- `MediaWork(SeasonId, InternalEpisodeNumber)` unique partiel pour saison connue et numéro interne non nul
- `MediaWork(SeriesWorkId, InternalEpisodeNumber)` unique partiel pour saison inconnue et numéro interne non nul
- `EpisodeExternalReference(ProviderId, ExternalKind, ProviderValue)` unique
- `EpisodeExternalMapping(EpisodeExternalReferenceId, EpisodeWorkId)` unique
- `MediaWorkExternalId(ProviderId, ExternalKind, ProviderValue)` unique
- `SeasonExternalId(ProviderId, ExternalKind, ProviderValue)` unique
- `MediaWorkMetadata(MediaWorkId, FieldName, LocaleCode, IsSelected)`
- `SeasonMetadata(SeasonId, FieldName, LocaleCode, IsSelected)`
- unicité sélectionnée partielle par `(MediaWorkId, FieldName, LocaleCode)` ou `(SeasonId, FieldName, LocaleCode)`, `WHERE IsSelected = 1`
- `WatchProgress(UserId, MediaWorkId)` unique
- `WatchProgress(UserId, UpdatedAt)`
- `WatchSession(UserId, MediaWorkId, StartedAt)`
- `WatchSession(Status, LastCheckpointAt)` pour la réconciliation des sessions ouvertes
- clé candidate et index composites sur `MediaFileAssignment(MediaWorkId, MediaFileId, Id)` pour vérifier les reprises par association
- `MediaScan(MediaRootId, StartedAt)`
- `MediaScanError(MediaScanId, Severity, OccurredAt)`
- `MediaScanScope(MediaScanId, RelativeDirectoryPath)` unique
- `StoryArc(SeriesWorkId, SortOrder)`
- `EpisodeStoryArc(StoryArcId, EpisodeWorkId)` unique

## 18. Choix EF Core recommandés

### Spécialisation MediaWork

**TPH — recommandé pour V1**

- une table commune simplifie les requêtes sur toutes les œuvres ;
- les trois types initiaux sont peu nombreux ;
- les migrations et les jointures sont simples ;
- les propriétés propres restent bien séparées conceptuellement ;
- plusieurs colonnes spécialisées peuvent être nulles ;
- les données propres doivent être maintenues par invariants métier.

Le discriminateur `WorkType` identifie les colonnes conditionnelles. Les contraintes locales et clés candidates composites visent à empêcher qu'une saison ou une référence typée pointe vers la mauvaise spécialisation ; les invariants applicatifs vérifient également que les champs et relations correspondent au type.

Limites : la table commune peut devenir large et les contraintes par spécialisation sont moins visibles que dans TPT. Certaines contraintes interlignes et inter-tables peuvent nécessiter des clés composites ou des triggers SQLite et une validation de compatibilité EF Core.

**TPT**

- normalise les propriétés communes et spécialisées ;
- offre des contraintes et relations plus explicites ;
- demande des jointures pour toute lecture spécialisée ;
- ajoute plusieurs clés étrangères et migrations plus lourdes.

**TPC**

- produit des tables entièrement indépendantes ;
- évite les jointures mais peut complexifier les identifiants communs ;
- les requêtes polymorphiques sur toutes les œuvres sont plus coûteuses ;
- les relations depuis `MediaWorkRelation`, les métadonnées et les progressions nécessitent une clé commune réellement stable.

**Recommandation : TPH.** Les identifiants internes GUID `MediaWork.Id` et les trois spécialisation initiales rendent TPH adapté à la simplicité de MyVidLib. Si le nombre de spécialisations augmente ou si les champs propres deviennent volumineux, une migration vers TPT ou une projection fortement typée pourra être arbitrée. Le choix n'autorise aucun code EF dans cette étape.

### Identifiants externes et métadonnées

La solution recommandée reste des tables dédiées par sujet. Elle apporte des clés étrangères fiables et évite les relations polymorphiques. La duplication de structure entre œuvres, saisons et épisodes est volontairement limitée pour sécuriser l'intégrité.

### Chemins

Stocker `MediaRoot.AbsolutePath` et `MediaFile.RelativePath`. Ne pas rendre un chemin absolu calculé persistant obligatoire. La normalisation Windows et le maintien de l'ID d'une racine lors d'un changement de montage sont définis à la section 7.

## 19. Décisions encore à arbitrer

Les éléments suivants restent `À ARBITRER` :

1. obligation d'un `SeriesWorkId` pour chaque épisode ;
2. représentation canonique de la saison spéciale 0 et de la saison inconnue ;
3. adoption des segments et des règles d'ordre/partie proposées ;
4. rapprochement des références externes qui couvrent plusieurs épisodes ;
5. normalisation exacte des chemins Windows, notamment Unicode et dossiers sensibles à la casse ;
6. seuil et règle confirmant qu'un fichier est réellement supprimé après un échec temporaire ;
7. passage d'une session `InProgress` à `Interrupted` après arrêt brutal, et utilité de l'état `Error` ;
8. priorité de lecture globale ou individuelle ;
9. validation de l'unité milliseconde et de la position relative à l'œuvre ;
10. seuil automatique de passage à `Watched` ;
11. politique de sélection des métadonnées et de validation des valeurs fournisseur ;
12. politique de conservation ou d'anonymisation de l'historique utilisateur ;
13. politique de sauvegarde et restauration des métadonnées et images ;
14. méthode détaillée de protection des clés API ;
15. règle précise d'expiration des valeurs fournisseurs ;
16. licences et droits d'affichage des images récupérées selon fournisseur ;
17. temporisation avant affichage d'une source comme `Inaccessible` ;
18. hébergement WPF/API lorsque le partage réseau sera implémenté.