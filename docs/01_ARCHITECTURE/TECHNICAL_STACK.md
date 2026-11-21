# MyVidLib - Pile technique

**Statut :** les choix du socle initial indiqués **VALIDÉ** sont définitifs. Les éléments marqués **PROPOSÉ** ou **À DÉFINIR** ne constituent pas des arbitrages.

## Plateforme et langage

| Élément | Choix | Statut |
|---|---|---|
| Langage | C# 14 | VALIDÉ |
| Runtime | .NET 10 LTS | VALIDÉ |
| OS | Windows 10 et Windows 11 | VALIDÉ |
| Interface | WPF | VALIDÉ |
| Architecture UI | MVVM | VALIDÉ |
| Base locale | SQLite | VALIDÉ |
| ORM | Entity Framework Core | VALIDÉ |
| Tests | xUnit | VALIDÉ |
| Toolkit MVVM | CommunityToolkit.Mvvm | VALIDÉ |
| Journalisation | Serilog | VALIDÉ |
| Publication initiale | .NET Self-contained | VALIDÉ |
| Serveur optionnel | ASP.NET Core pour le partage réseau, à partir de M7 | VALIDÉ pour M7 |
| Hôte du serveur API | Même processus que WPF ou processus séparé | À DÉFINIR |

## Dépendances

Les versions NuGet sont centralisées dans `Directory.Packages.props` et maîtrisées. Toute mise à jour doit être examinée et les licences des dépendances vérifiées.

### Dépendances du socle

- **WPF/.NET** : interface native Windows.
- **Microsoft.EntityFrameworkCore.Sqlite** : accès à SQLite.
- **Microsoft.EntityFrameworkCore.Design** : migrations EF Core.
- **Microsoft.Extensions.DependencyInjection** et **Microsoft.Extensions.Configuration** : composition et configuration.
- **CommunityToolkit.Mvvm** : implémentation de référence de MVVM.
- **Serilog.Extensions.Logging** : intégration de Serilog avec les abstractions de journalisation .NET.
- **Serilog.Sinks.File** : journaux fichier avec rotation et rétention configurées.
- **Serilog.Sinks.Console** : utile en développement ; son activation et sa configuration sont propres à l'environnement.
- **xUnit** : tests automatisés.

### Lecture vidéo

La solution distingue :

- **LibVLCSharp.WPF** : intégration .NET et contrôles WPF du lecteur.
- **VideoLAN.LibVLC.Windows** : moteur VLC natif pour Windows.

Le moteur et ses dépendances natives doivent être distribués avec l'application et testés dans la publication réelle. La publication Self-contained du runtime .NET ne garantit pas, à elle seule, l'inclusion des exécutables et bibliothèques natifs tiers.

### FFprobe

FFprobe est distribué avec MyVidLib et utilisé pour l'indexation des médias. L'intégration doit :

- maîtriser la version utilisée ;
- résoudre le chemin depuis une configuration centralisée, sans chemin d'installation codé en dur ;
- vérifier la disponibilité de l'exécutable avant son utilisation ;
- gérer explicitement les erreurs de lancement, de délai, de code de sortie et de sortie invalide.

La version exacte et la provenance des distributions FFmpeg/FFprobe restent **À DÉFINIR**.

### Dépendances optionnelles et futures

| Fonctionnalité | Dépendance ou approche | Statut |
|---|---|---|
| Métadonnées | `HttpClient` et bibliothèques JSON intégrées | PROPOSÉ |
| Images | Mécanismes natifs WPF, cache mémoire et cache disque | Orientation validée |
| Partage réseau | ASP.NET Core | VALIDÉ pour M7 |
| Authentification réseau | Solution adaptée aux profils et autorisations définis | À DÉFINIR |

Pour les images, privilégier les mécanismes natifs WPF de chargement et d'affichage, avec des caches mémoire et disque. Ne pas introduire `System.Drawing` comme dépendance générale de l'application.

## Contraintes techniques

- Activer NRT (`<Nullable>enable</Nullable>`) dans les projets C#.
- Utiliser des opérations asynchrones pour les I/O lorsque les API le permettent.
- Toute opération longue doit accepter un `CancellationToken`.
- Les fournisseurs externes ne doivent pas bloquer l'accès au catalogue local.
- SQLite reste encapsulé derrière les responsabilités d'Infrastructure.
- Vérifier les licences et conditions d'utilisation des dépendances et fournisseurs externes (TMDB, Animeka, Anime-Sama, IMDb).
- Ne jamais versionner de clé API, secret ou autre information sensible.
- Sous Windows, protéger les secrets nécessaires au moyen d'un mécanisme adapté, par exemple DPAPI ou le gestionnaire de secrets Windows.

## Publication Windows

- La première version utilise une publication .NET **Self-contained**.
- Le choix de l'installateur Windows est reporté à une étape ultérieure.
- Ne pas traiter Self-contained comme une garantie d'intégration automatique des exécutables et bibliothèques natives : vérifier explicitement les artefacts publiés, notamment FFprobe et le moteur VLC.
- Tester la publication sur une machine ne disposant pas du SDK .NET.
- L'architecture de distribution au-delà de la première cible retenue (x86/x64/ARM64) reste **À DÉFINIR**.

## Stockage local

Le répertoire par défaut est `%LOCALAPPDATA%\MyVidLib`, avec l'organisation proposée suivante :

```text
%LOCALAPPDATA%\MyVidLib\
  Data\
  Cache\
  Logs\
  Backups\
  Config\
```

Les chemins sont centralisés dans un service de chemins et configurables. Les fichiers vidéo originaux restent à leur emplacement : ils ne doivent jamais être déplacés automatiquement vers le répertoire de données.

La politique définitive de sauvegarde et de restauration reste **À DÉFINIR**.

## Journalisation

Serilog fournit des journaux structurés. La configuration doit couvrir les niveaux de gravité, la rotation et la rétention des fichiers de logs. Les journaux ne doivent contenir ni secrets ni données sensibles inutiles. Le sink console peut être activé pour le développement.

## Décisions restantes

- Architecture d'hébergement du serveur API (même processus ou processus séparé).
- Stratégie définitive de concurrence WPF/API.
- Format de l'installateur Windows.
- Architectures de distribution au-delà de la première cible.
- Politique définitive de sauvegarde et restauration.
- Version et provenance exactes des distributions FFmpeg/FFprobe et VLC.
- Choix détaillés d'authentification réseau et de protection des secrets en fonction des usages.

Voir [architecture logicielle](SOFTWARE_ARCHITECTURE.md), [conventions de code](CODING_CONVENTIONS.md) et [notes de revue technique](TECHNICAL_REVIEW_NOTES.md).
