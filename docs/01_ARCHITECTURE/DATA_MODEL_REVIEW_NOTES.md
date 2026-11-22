# MyVidLib — Notes de revue du modèle de données V2.3

**Date :** 21/11/2026  
**Statut :** consolidation documentaire — PROPOSÉE, en attente de validation humaine ; aucune implémentation réalisée.

## 1. Corrections V2.3

### Un seul fichier physique par œuvre

- Décision VALIDÉE : un film = un fichier physique ; un épisode = un fichier physique ; un OAV = un fichier physique ; une série = œuvre conteneur sans fichier vidéo directement associé.
- Aucun découpage multipartite ; aucune version alternative (1080p, 4K, VF, VOSTFR, etc.).
- Suppression des rôles `Alternative`, `Part`, `Segment` (et `Other`) et des propriétés `PartNumber`, `WorkStartMs`, `WorkEndMs`, `SegmentStartMs`, `SegmentEndMs`, `PlaybackOrder`.
- Position de lecture relative au fichier unique ; une même référence physique ne peut pas être associée à plusieurs œuvres.

### Représentation des saisons

- Décision VALIDÉE : `SeasonKind` (`Normal`, `Special`, `Unknown`) avec `SeasonNumber` (`1, 2, 3…`, `0`, `null`).
- Une saison `Special` et une saison `Unknown` au maximum par série.
- Un épisode dont la saison est inconnue est rattaché à la saison `Unknown`.
- Les épisodes spéciaux restent des épisodes du modèle commun `MediaWork`.

### Références externes couvrant plusieurs épisodes

- Décision VALIDÉE : une référence externe peut correspondre à un ou plusieurs épisodes MyVidLib.
- Correspondances certaines automatiques ; correspondances ambiguës soumises à validation humaine.
- La numérotation originale de chaque fournisseur est conservée.
- Aucun épisode artificiel n'est créé uniquement pour résoudre un conflit de numérotation.

### Sélection et actualisation des métadonnées

- Décision VALIDÉE : priorité configurable par fournisseur ; conservation des valeurs d'origine et de leur provenance ; dates de récupération conservées.
- Une donnée validée manuellement ne peut pas être écrasée automatiquement ; les conflits importants sont soumis à validation.
- Actualisation périodique mensuelle et manuelle possible ; aucun écrasement des valeurs validées manuellement ; aucune suppression en cas d'échec fournisseur.

### Images et licences

- Décision VALIDÉE : usage personnel uniquement, conservation locale lorsque les conditions du fournisseur le permettent, provenance conservée, pas de redistribution publique, respect des licences et conditions d'utilisation propres à chaque fournisseur.

### Stockage et disponibilité des fichiers

- Décision VALIDÉE : chemins absolus normalisés, séparateurs harmonisés, **Unicode NFC**, comparaison insensible à la casse par défaut, respect des dossiers Windows configurés avec une sensibilité à la casse, prise en charge des lecteurs amovibles et des chemins UNC, conservation du chemin d'affichage original.
- Détection des fichiers manquants : passage à `Missing` uniquement après **trois scans complets consécutifs** sans le retrouver **et au moins 24 heures** depuis la première absence confirmée. Un scan partiel ou une source inaccessible ne compte pas.
- Un fichier `Missing` reste en base (catalogue, métadonnées, historique, informations techniques conservés) et peut être réassocié ultérieurement.
- Suppression volontaire par l'administrateur : trois opérations distinctes (retirer une référence sans toucher au fichier ; supprimer physiquement le fichier après confirmation ; retirer une œuvre du catalogue selon règles de protection de l'historique). Après suppression physique, l'œuvre reste au catalogue, la référence au fichier est conservée avec statut `Missing`, métadonnées et historiques préservés. Motif et date de suppression enregistrés pour distinguer suppression volontaire et absence détectée par scan.
- Disponibilité des sources : plusieurs tentatives espacées, délai configurable avant `Inaccessible`, distinction disque débranché / partage indisponible / erreur d'accès, aucune suppression du catalogue causée par une source inaccessible.

### Visionnage et historique

- Décision VALIDÉE : session `InProgress` sans mise à jour pendant cinq minutes (délai configurable) → `Interrupted` ; vérification au démarrage et périodiquement ; dernière position de lecture conservée ; aucune validation automatique de `Watched`.
- `WatchSession.Status` : `InProgress`, `EndedNormally`, `Interrupted`, `Error`. `MediaEndReached` indique la fin du fichier. Ces informations ne sont pas interchangeables.
- Priorité de lecture globale configurable ; choix du fichier `Present` sur la source prioritaire ; repli sur une autre source ; choix manuel possible.
- Passage automatique à `Watched` à 90 % de la durée du fichier (seuil configurable), statut indépendant par utilisateur, modification manuelle possible.
- Conservation de l'historique illimitée par défaut, sans purge automatique ; purge manuelle possible par l'administrateur ; suppression des données d'un utilisateur sans toucher au catalogue commun ni aux autres utilisateurs.

### Sauvegardes et sécurité

- Décision VALIDÉE : sauvegarde complète (base SQLite, configuration, utilisateurs, progressions et historiques, métadonnées locales, associations et identifiants fournisseurs, images téléchargées et cache utile). Fichiers vidéo non inclus par défaut.
- Protection des secrets par DPAPI Windows ; aucun secret en clair dans Git ou les fichiers de configuration ; procédure de migration/restauration prévue. Le futur serveur Linux devra utiliser un mécanisme adapté.

### Architecture cible

- Décision VALIDÉE : hébergement initial = application Windows WPF avec API ASP.NET Core intégrée au processus, architecture permettant la séparation ultérieure.
- Cible finale Raspberry Pi 4 : serveur central Linux ARM64 (.NET/ASP.NET Core, SQLite), catalogue et stockage centralisés, API réseau local, interface web responsive, accès aux fichiers sur disques USB ou stockages réseau. WPF reste l'interface Windows de première version ; le serveur Raspberry Pi ne dépend pas de WPF.

### Synchronisations de métadonnées (rappel inchangé)

- Clé d'équivalence : sujet typé, champ, langue, provenance/fournisseur, type et valeur canonique.
- Une synchronisation identique réutilise le candidat existant et rafraîchit `RawValue`, `LastFetchedAt` et, selon la réponse ou la politique de source, `ExpiresAt`. Elle ne crée pas une nouvelle ligne et ne modifie ni validation ni sélection.
- Une valeur canonique différente crée un nouveau candidat `Proposed`, sans effacer l'ancien ni changer automatiquement la sélection. Les corrections manuelles restent distinctes, prioritaires si validées/sélectionnées et jamais remplacées par une synchronisation.
- `ValueType` couvre `Text`, `Number`, `Date`, `List`, `Image` et `Structured`. `CandidateValue` est du JSON canonique ; `RawValue` ne garde que la dernière charge utile.
- Les dates de première et dernière observation sont conservées. Aucune table d'observations/historique n'est ajoutée.

## 2. Décisions préservées et statut

Les décisions structurelles antérieures restent inchangées : `MediaWork` commun en TPH, `Movie`/`Series`/`Episode`, Anime comme classification, saisons séparées, suppression des versions alternatives et des rôles multipartites, scans conservateurs, métadonnées avec provenance, correction manuelle prioritaire, progression par utilisateur, position en millisecondes relative à l'œuvre, distinction session/progression et choix initiaux SQLite/EF Core.

Le rattachement obligatoire de l'épisode par `SeriesWorkId` à une série (saison facultative) est VALIDÉ, conformément à la section 1 de `DATA_MODEL.md`. La normalisation des candidats, `EndedNormally`, les critères de sélection et les replis restent **PROPOSÉES** et attendent la validation humaine.

## 3. Propriétés ajoutées ou précisées

- `MediaFileAssignment.WorkStartMs`, `WorkEndMs` : **supprimés en V2.3** (aucune borne temporelle de partie ne subsiste).
- `WatchSession.MediaEndReached` : fin naturelle du média signalée, non signalée ou inconnue.
- Métadonnées : `ValueType`, `LastFetchedAt` et valeur `CandidateValue` en JSON canonique ; définition de l'idempotence par clé logique.
- Aucun nouvel objet conceptuel ni aucune nouvelle table d'historique ne sont ajoutés.

## 4. Contraintes et limites techniques à vérifier

- Les `CHECK` locaux vérifient les propriétés d'une seule ligne (`WorkType`, `SeasonKind`, etc.) ; les règles de sélection et la cohérence intertable avec `MediaFile.DurationMs` nécessitent des invariants applicatifs testés.
- Les index uniques partiels par source traitent séparément `ProviderId IS NOT NULL` et `ProviderId IS NULL`, car SQLite considère plusieurs `NULL` comme distincts. La clé candidate sélectionnée reste unique par sujet/champ/langue.
- L'idempotence concurrente doit être protégée par l'index d'équivalence et l'upsert doit être transactionnel ; les index de valeur JSON canonique peuvent être volumineux.
- `CandidateValue` est stocké comme texte JSON. La canonicalisation, validation de types et compatibilité éventuelle JSON/SQLite avec EF Core devront être testées avec les versions retenues.
- Le statut et l'accessibilité du fichier/racine et l'archivage sont des faits répartis sur plusieurs lignes/tables ; les contraintes SQL seules ne garantissent pas la décision complète de reprise.
- Les index d'expression `COALESCE`, filtres partiels et clés composites de V2.2 restent à valider avec EF Core et SQLite.

## 5. Arbitrages encore ouverts

Aucun arbitrage ouvert : tous les arbitrages de la décision V2.3 sont clôturés et appliqués au modèle. Les recommandations techniques restantes (index partiels, COALESCE, canonicalisation JSON/SQLite, validations fournisseur) sont marquées PROPOSÉES et attendent validation technique, mais aucun arbitrage métier ne reste en attente.

## 6. Conséquences documentaires

Les documents suivants peuvent nécessiter une harmonisation ultérieure, après validation de V2.2.1 :

1. `WATCH_TRACKING.md` : position relative à l'œuvre, états `EndedNormally`/`MediaEndReached`, règle de progression `Watched` et sélection d'association. Les références aux parties/segments doivent être supprimées.
2. `METADATA_PROVIDERS.md` : upsert idempotent, clé d'équivalence, format canonique et conservation de la correction manuelle.
3. `MEDIA_SCANNER.md` : la règle des portées de scan de V2.2 demeure à harmoniser ; cette passe n'en change pas le comportement.
4. `METADATA_PROVIDERS.md` conserve également une priorité TMDB générale qui reste à concilier avec la priorité configurable par contenu ; la priorité anime Animeka n'est pas modifiée.
5. `ANIME_MANAGEMENT.md` évoque une catégorie/spécialisation Anime ; V2.2.1 conserve Anime comme classification et ne modifie pas les types structurels.

Aucun de ces documents n'a été modifié.

## 7. Conclusion

V2.2.1 précise les quatre sujets demandés sans ajouter d'entité ni commencer l'implémentation. Les propositions attendent la validation humaine. Aucun code C#, objet EF Core, `DbContext`, migration ou base SQLite n'a été créé.
