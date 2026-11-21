# MyVidLib - Conventions de code

**Statut :** VALIDÉ pour le socle initial. Les décisions explicitement laissées ouvertes figurent dans [les notes de revue technique](TECHNICAL_REVIEW_NOTES.md).

## Nommage C#

- PascalCase : types, méthodes, propriétés, constantes.
- camelCase : variables locales et paramètres.
- Préfixes d'interfaces : `I` suivi d'un nom fonctionnel.
- Asynchrones : suffixe `Async`.
- Collections : noms pluriels.
- Noms explicites sans abréviation ambiguë.

Exemple : `ScanMediaRootAsync(MediaRootId rootId, CancellationToken cancellationToken)`.

## Organisation des namespaces

Les namespaces reflètent les dossiers et projets :

- `MyVidLib.Domain.Media`
- `MyVidLib.Application.Library`
- `MyVidLib.Infrastructure.Persistence`
- `MyVidLib.Infrastructure.Scanning`
- `MyVidLib.Wpf.ViewModels`
- `MyVidLib.Wpf.Views`

## Structure des classes

- Une responsabilité principale par classe.
- Ordre recommandé : champs, constructeurs, propriétés, commandes/méthodes publiques, méthodes privées.
- Préférer les petits services spécialisés aux classes de service « tout-en-un ».
- Les constructeurs utilisent l'injection de dépendances.
- Ne pas ajouter de couche ou abstraction sans justification.

## Interfaces

- Définir les ports dans `Application` ou `Domain`, les implémenter dans `Infrastructure`.
- Les interfaces représentent une capacité métier, pas une technologie.
- Nommage : `IMediaFileRepository`, `IMetadataProvider`, `IMediaScanner`.

## DTO et modèles

- Les entités ne doivent pas transiter directement dans les ViewModels ou API lorsqu'elles exposent trop de détails.
- DTO impérativement immuables ou records si adapté.
- DTO séparés des modèles de persistance et des ViewModels.
- Chaque champ externe doit être validé avant utilisation.

## ViewModels et MVVM

CommunityToolkit.Mvvm est la bibliothèque de référence. Utiliser selon le besoin :

- `ObservableObject` pour la notification de changements ;
- `ObservableProperty` pour générer les propriétés observables ;
- `RelayCommand` pour les commandes synchrones ;
- `AsyncRelayCommand` pour les commandes asynchrones.

Un ViewModel est prévu pour chaque écran principal. Ne pas ouvrir de fichiers, accéder à EF Core, appeler directement les fournisseurs ou manipuler les objets natifs VLC depuis une Vue/ViewModel. La résolution et la gestion des chemins physiques sont déléguées aux abstractions applicatives. Exposer des états explicites : chargement, vide, erreur et résultat.

## Journalisation

Serilog est la solution officielle. Les logs sont structurés et utilisent des niveaux de gravité adaptés. La configuration prévoit la rotation et la rétention des fichiers de logs.

Ne jamais enregistrer de secrets ni de données sensibles inutiles. Les chemins physiques, données personnelles et valeurs de configuration ne sont journalisés que si nécessaires au diagnostic et dans une forme appropriée.

## Exceptions

- Exceptions .NET standard pour les violations techniques.
- Exceptions métier spécifiques et limitées aux cas applicatifs.
- Ne jamais masquer une exception avec un retour silencieux.
- Les erreurs attendues peuvent utiliser un résultat typé.
- Ne jamais afficher une pile technique à l'utilisateur final.

## Async/await

- Suffixe `Async` sur toutes les méthodes asynchrones.
- Configurer `ConfigureAwait(false)` dans les bibliothèques internes si les conventions du projet le définissent.
- Ne pas utiliser `async void`, sauf pour les gestionnaires d'événements.
- Ne pas exécuter les traitements longs sur le thread UI.
- Utiliser `SemaphoreSlim`, des opérations parallèles limitées ou un flux asynchrone pour les scans, selon le besoin.

## CancellationToken

- Paramètre final lorsqu'une opération peut être annulée.
- Vérifier l'annulation aux étapes longues.
- Ne pas enregistrer une opération partielle comme complète après annulation.

## Nullable Reference Types

- NRT activé dans tous les projets.
- Éviter `!` sauf justification.
- Les domaines utilisent des garde-fous pour les valeurs obligatoires.
- Les API JSON externes doivent être validées avant création des modèles métier.

## Gestion des dépendances NuGet

- Centraliser les versions de packages dans `Directory.Packages.props`.
- Maîtriser les mises à jour et examiner leurs effets sur le socle et la publication.
- Vérifier les licences et conditions d'utilisation des dépendances.
- Ne pas ajouter de dépendance générale `System.Drawing` pour les images ; privilégier les mécanismes natifs WPF.

## Ressources natives et configuration

- Ne jamais coder en dur les chemins d'installation des outils ou bibliothèques natifs.
- Résoudre les chemins FFprobe/VLC depuis la configuration centralisée et vérifier leur disponibilité.
- Traiter explicitement les échecs d'exécution de FFprobe et l'absence de ressources natives VLC.
- La publication initiale est Self-contained, mais cela ne garantit pas l'inclusion des ressources natives tierces.
- Tester la publication sur une machine ne disposant pas du SDK .NET.
- Ne versionner aucune clé API ni information sensible. Protéger les secrets sous Windows avec une solution adaptée, par exemple DPAPI ou le gestionnaire de secrets Windows.

## Tests

- Convention : `XxxTests.cs`.
- xUnit, noms lisibles : `Method_State_ExpectedResult`.
- Arrange/Act/Assert.
- Préférer les données temporaires à SQLite en mémoire pour les tests de dépôt.
- Les tests externes doivent simuler HTTP et FFprobe.
- Ajouter des tests ciblés pour les chemins locaux, externes et UNC, les indisponibilités de sources, les erreurs FFprobe et la résolution de configuration.
- Tester qu'une source indisponible ne déclenche pas la suppression de ses fichiers de l'index.
- Tester la publication et les ressources natives dans le livrable réel.

## Git et informations sensibles

- Une mission par branche si le workflow Git est adopté.
- Messages impératifs et courts : `feat: add media root scanner`.
- Ne pas committer de bases, caches, clés API, secrets, médias ou fichiers temporaires.
- Ne pas versionner de configuration locale contenant des secrets.
- Mettre à jour le changelog et le handover uniquement pour les missions validées.

## Règles générales

- Ne pas ajouter de dépendance hors de l'écosystème C#/.NET sans justification.
- Pas de logique métier dans les Views ou le code-behind.
- Pas de noms ou chemins absolus codés en dur.
- Toute modification du périmètre doit être documentée et validée avant implémentation.
