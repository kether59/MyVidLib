# MyVidLib — Notes de revue technique

Ce document consolide les arbitrages du propriétaire et distingue les décisions acquises des recommandations et questions encore ouvertes. Il ne lance pas le jalon M0 et ne modifie pas le périmètre fonctionnel.

## Décisions VALIDÉES

### Socle initial

- C# 14, .NET 10 LTS, WPF et MVVM.
- SQLite avec Entity Framework Core.
- xUnit pour les tests.
- CommunityToolkit.Mvvm pour les ViewModels et commandes.
- Serilog pour la journalisation structurée.
- Publication .NET Self-contained pour la première version.
- Données locales par défaut dans `%LOCALAPPDATA%\MyVidLib`.
- FFprobe distribué avec MyVidLib.
- Runtime VLC distribué avec MyVidLib et lecteur basé sur LibVLCSharp.
- Dépendances lecteur à distinguer : package `LibVLCSharp.WPF` et moteur natif `VideoLAN.LibVLC.Windows`.

### Règles d'architecture et d'exploitation

- Conserver `MyVidLib.Domain`, `MyVidLib.Application`, `MyVidLib.Infrastructure` et `MyVidLib.Wpf`. Créer `MyVidLib.Api` uniquement au jalon M7.
- Séparer l'abstraction applicative du lecteur, son implémentation LibVLCSharp et le composant visuel WPF. Les ViewModels ne manipulent pas directement les objets natifs VLC.
- Combiner `FileSystemWatcher`, scans périodiques, scans manuels et réconciliation avec l'index. Une source indisponible ne doit jamais entraîner la suppression de ses fichiers indexés.
- Centraliser la gestion des chemins physiques locaux, externes et UNC ; les ViewModels ne les gèrent pas directement.
- Utiliser un `DbContext` par unité de travail et ne jamais partager la même instance entre opérations concurrentes.
- Conserver les extensions d'enregistrement de services par module, sans multiplier les couches injustifiées.
- Utiliser Serilog et ne pas journaliser de secrets ou données sensibles inutiles.
- Centraliser et configurer les chemins de stockage. Ne jamais déplacer automatiquement les fichiers vidéo originaux.
- Tester la disponibilité et l'exécution des ressources FFprobe/VLC ainsi que la publication sur une machine sans SDK .NET.

## Propositions restantes

Les exigences ci-dessus sont validées ; les pistes suivantes décrivent des détails d'implémentation proposés, sans imposer de bibliothèque auxiliaire ni de paramètre encore non arbitré :

- Organisation suggérée des sous-répertoires en `Data`, `Cache`, `Logs`, `Backups` et `Config`.
- Mise en œuvre des caches d'images mémoire et disque avec les mécanismes natifs WPF.
- Activation du sink console Serilog en développement lorsque cela est utile.
- Choix concret de la technologie Windows de protection des secrets (par exemple DPAPI ou gestionnaire de secrets Windows).
- Détails du résolveur de ressources natives et de la configuration de chemin FFprobe.
- Mécanisme de planification des scans périodiques et de réconciliation avec les événements `FileSystemWatcher`.

## Points À DÉFINIR

- Hébergement de l'API : même processus que WPF ou processus séparé.
- Stratégie définitive de concurrence et de partage des données entre WPF et API avant M7.
- Format de l'installateur Windows ; son choix est reporté à une étape ultérieure.
- Architectures de distribution x86/x64/ARM64 au-delà de la première cible.
- Politique définitive de sauvegarde et de restauration.
- Versions exactes et provenance des distributions FFmpeg/FFprobe et VLC.
- Valeurs opérationnelles de rotation et de rétention des logs et politique de conservation des données de diagnostic.
- Modèle précis d'état/temporisation pour les fichiers dont la disparition est constatée, en cohérence avec la règle de non-suppression automatique en cas de source indisponible.
- Solution définitive de protection des secrets et configuration de l'authentification réseau.

## Risques techniques

- **Ressources natives et publication :** Self-contained ne garantit pas le conditionnement correct de FFprobe, LibVLC et de leurs dépendances. Une publication peut réussir tout en échouant à l'exécution sur une machine propre.
- **Compatibilité des composants :** les versions de .NET, LibVLCSharp et du moteur VLC doivent être compatibles entre elles et avec l'architecture cible.
- **Sources locales et réseau :** `FileSystemWatcher` peut perdre des événements et les lecteurs amovibles ou partages UNC peuvent être temporairement inaccessibles. Des scans de réconciliation et une distinction fiable entre indisponibilité et suppression sont indispensables.
- **SQLite et accès concurrents :** SQLite et EF Core imposent de gérer explicitement les unités de travail, les verrous et les accès potentiels depuis WPF et l'API.
- **Chemins externes :** chemins déconnectés, autorisations et variations de disponibilité demandent une gestion centralisée et des tests réels.
- **Secrets et logs :** configuration incorrecte ou journalisation trop détaillée peut exposer des clés API, informations de compte ou chemins privés.
- **Stockage et sauvegardes :** une politique encore non définie peut compliquer la restauration cohérente de la base, des configurations et des caches.

## Modifications effectuées

- `TECHNICAL_STACK.md` : arbitrages du socle déclarés validés ; dépendances MVVM, Serilog et LibVLCSharp précisées ; FFprobe, ressources natives, images, publication, stockage, secrets et sujets ouverts documentés.
- `SOFTWARE_ARCHITECTURE.md` : projets et jalon M7 clarifiés ; séparation du lecteur, scanner, chemins, unités de travail EF Core, concurrence, DI, logs et tests complétée.
- `CODING_CONVENTIONS.md` : conventions NuGet, MVVM, logs, ressources natives, secrets et tests ciblés ajoutées.
- Cohérence vérifiée avec `PROJECT_VISION.md` et `FUNCTIONAL_SCOPE.md` : conservation des fichiers originaux, fonctionnement local prioritaire, distinction œuvre/fichier, sources indisponibles, partage réseau et suivi individuel préservés.

## Questions nécessitant encore une décision

Les décisions explicites du socle initial n'appellent pas de confirmation. Les questions qui subsistent sont celles listées dans « Points À DÉFINIR », notamment l'hébergement de l'API, la stratégie de concurrence avant M7, l'installateur, les architectures de distribution, les sauvegardes et les versions/provenances FFmpeg et VLC.
