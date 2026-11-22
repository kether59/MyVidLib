# MyVidLib — Notes de revue du modèle de données V2.2.1

**Date :** 21/11/2026  
**Statut :** passe corrective documentaire — PROPOSÉE, en attente de validation humaine ; aucune implémentation réalisée.

## 1. Corrections V2.2.1

### Reprise d'un épisode multipartite

- Ajout proposé de `WorkStartMs` et `WorkEndMs` à `MediaFileAssignment` pour décrire les bornes logiques de chaque partie sur la timeline de l'œuvre. Les bornes physiques `SegmentStartMs`/`SegmentEndMs` restent relatives au fichier.
- `PartNumber` identifie et ordonne par défaut les parties ; `PlaybackOrder` explicite doit rester cohérent avec lui. Ces valeurs ne déterminent pas les offsets temporels.
- Les offsets sont enregistrés explicitement. La somme des durées FFprobe peut suggérer des bornes initiales uniquement lorsque la continuité a été vérifiée ; elle n'est pas recalculée implicitement à la reprise.
- Les intervalles peuvent contenir des discontinuités, mais pas de chevauchements confirmés. Une durée physique inconnue, une incohérence ou une partie absente interdit un seek intérieur présenté comme exact.
- Le repli est déterministe : prochaine partie exploitable après la position, à son début, avec indication d'ajustement ; à défaut, proposer un redémarrage depuis la première partie exploitable. Les associations `Alternative`, archivées, rejetées, proposées ou indisponibles ne satisfont pas une partie.
- **Durée d'épisode (`DurationMs`)**: sur un épisode, `DurationMs` représente la durée totale en millisecondes relative au début de l'épisode (déterminée par FFprobe), et non la durée du fichier. Une valeur nulle indique inconnue. Cette durée peut différer des bornes `WorkStartMs`/`WorkEndMs` d'une association `Part` spécifique : une partie d'un épisode ne couvre pas nécessairement toute sa durée. Un épisode sans fichier associé n'a pas de durée connue.

- **Clé d'équivalence des métadonnées**: `(sujet typé, FieldName, LocaleCode, Provenance, ProviderId, ValueType, CandidateValue canonique)` définit l'équivalence. Deux candidats équivalents ne créent pas de doublon ; la synchronisation réutilise la ligne existante, met à jour `RawValue` et `LastFetchedAt`, sans modifier validation ni sélection. Une valeur canonique différente crée un nouveau candidat `Proposed`.

- **État de session (`WatchSession.Status`)**: `WatchSession.Status` utilise les valeurs `InProgress`, `EndedNormally`, `Interrupted` et `Error`. `EndedNormally` signifie arrêt contrôlé, pas fin du média ni œuvre visionnée. `MediaEndReached` distingue fin naturelle signalée par le lecteur.

- **Sélection d'association**: Les règles de sélection privilégient la dernière association exploitables ; `Primary` puis `Alternative` pour œuvre sur fichier complet, avec départage stable par version et identifiant fichier. Les parties suivent règle d'intervalle dédiée : `PartNumber` obligatoire et positif, `WorkStartMs >= 0` et `WorkEndMs > WorkStartMs` requis pour association confirmée/manuelle. Aucun historique n'est supprimé lors changement d'association ; le nouveau pointeur de progression mis à jour au début effective de lecture.

### Synchronisations de métadonnées

- Définition d'une clé d'équivalence : sujet typé, champ, langue, provenance/fournisseur, type et valeur canonique.
- Une synchronisation identique réutilise le candidat existant et rafraîchit `RawValue`, `LastFetchedAt` et, selon la réponse ou la politique de source, `ExpiresAt`. Elle ne crée pas une nouvelle ligne et ne modifie ni validation ni sélection.
- Une valeur canonique différente crée un nouveau candidat `Proposed`, sans effacer l'ancien ni changer automatiquement la sélection. Les corrections manuelles restent distinctes, prioritaires si validées/sélectionnées et jamais remplacées par une synchronisation.
- `ValueType` couvre `Text`, `Number`, `Date`, `List`, `Image` et `Structured`. `CandidateValue` est du JSON canonique ; `RawValue` ne garde que la dernière charge utile, pas chaque observation identique.
- Les dates de première et dernière observation sont conservées. Aucune table d'observations/historique n'est ajoutée ; un audit immuable de chaque réponse réseau reste un besoin éventuel à justifier.

### Session et progression

- `EndedNormally` est l'état terminal de `WatchSession.Status` (arrêt contrôlé de la session, pas fin du média ni œuvre visionnée).
- `MediaEndReached` distingue la fin naturelle signalée par le lecteur de la terminaison de session ; il peut être inconnu.
- Transitions proposées : `InProgress` vers `EndedNormally`, `Error` ou `Interrupted`. Un arrêt normal après cinq minutes donne `EndedNormally` sans faire passer `WatchProgress` à `Watched`.
- `Watched` nécessite une confirmation utilisateur ou une politique automatique explicitement définie. La fin de session et la fin naturelle du média ne suffisent pas seules. Une session ouverte après crash reste réconciliable sans changement automatique de progression.

### Sélection des associations

- Les références `WatchSession` sont historiques ; les pointeurs de `WatchProgress` restent ceux de la dernière association réellement utilisée, même si elle devient invalide. Ils ne constituent pas une sélection automatique.
- Une nouvelle lecture exclut les associations archivées, rejetées ou `Proposed`, ainsi que les fichiers non `Present` et les racines non `Accessible`.
- La sélection proposée privilégie la dernière association encore exploitable ; sinon `Primary`, puis `Alternative` pour une œuvre sur fichier complet, avec départage stable par version et identifiant de fichier. Les parties suivent la règle d'intervalle dédiée.
- Aucun historique n'est supprimé lors d'un changement d'association ; le nouveau pointeur de progression n'est mis à jour qu'au début effectif de la lecture.

## 2. Décisions préservées et statut

Les décisions structurelles antérieures restent inchangées : `MediaWork` commun en TPH, `Movie`/`Series`/`Episode`, Anime comme classification, saisons séparées, versions/fichiers/affectations distincts, rôles d'affectation, scans conservateurs, métadonnées avec provenance, correction manuelle prioritaire, progression par utilisateur, position en millisecondes relative à l'œuvre, distinction session/progression et choix initiaux SQLite/EF Core.

Le rattachement obligatoire de l'épisode par `SeriesWorkId` à une série (saison facultative) est VALIDÉ, conformément à la section 1 de `DATA_MODEL.md`. Les propriétés et politiques introduites dans cette passe — offsets des parties, normalisation des candidats, `EndedNormally`, critères de sélection et replis — restent **PROPOSÉES** et attendent la validation humaine.

## 3. Propriétés ajoutées ou précisées

- `MediaFileAssignment.WorkStartMs`, `WorkEndMs` : intervalle logique de la partie sur la timeline de l'œuvre.
- `WatchSession.MediaEndReached` : fin naturelle du média signalée, non signalée ou inconnue.
- Métadonnées : `ValueType`, `LastFetchedAt` et valeur `CandidateValue` en JSON canonique ; définition de l'idempotence par clé logique.
- Aucun nouvel objet conceptuel ni aucune nouvelle table d'historique ne sont ajoutés.

## 4. Contraintes et limites techniques à vérifier

- Les `CHECK` locaux peuvent vérifier le couple de bornes logiques, leur ordre et les bornes physiques. Les chevauchements entre parties, la cohérence intertable avec `MediaFile.DurationMs` et les règles de sélection nécessitent des invariants applicatifs testés.
- Les index uniques partiels par source traitent séparément `ProviderId IS NOT NULL` et `ProviderId IS NULL`, car SQLite considère plusieurs `NULL` comme distincts. La clé candidate sélectionnée reste unique par sujet/champ/langue.
- L'idempotence concurrente doit être protégée par l'index d'équivalence et l'upsert doit être transactionnel ; les index de valeur JSON canonique peuvent être volumineux.
- `CandidateValue` est stocké comme texte JSON. La canonicalisation, validation de types et compatibilité éventuelle JSON/SQLite avec EF Core devront être testées avec les versions retenues.
- Le statut et l'accessibilité du fichier/racine, les bornes temporelles des parties et l'archivage sont des faits répartis sur plusieurs lignes/tables ; les contraintes SQL seules ne garantissent pas la décision complète de reprise.
- Les index d'expression `COALESCE`, filtres partiels et clés composites de V2.2 restent à valider avec EF Core et SQLite.

## 5. Arbitrages encore ouverts

- Source et validation humaine des `WorkStartMs`/`WorkEndMs`, traitement des intervalles incohérents et acceptation du repli vers une partie ultérieure.
- Utilisation de bornes physiques sur `Part` et condition de correspondance exacte entre durée physique et intervalle logique.
- Délai de réconciliation d'une session `InProgress` après crash ; utilité exacte de l'état `Error`.
- Seuil éventuel d'automatisation de `Watched`.
- Règles de canonicalisation par champ (notamment listes, dates, images), validation fournisseur et sélection lorsque la valeur fournisseur change ; nécessité éventuelle d'un audit de chaque observation.
- Priorités de version/fournisseur et préférences utilisateur, sans modifier la priorité configurable existante par type de contenu.
- Décisions V2.2 toujours ouvertes : saison spéciale 0 et saison inconnue, seuil avant `Missing`, politique d'historique, sauvegardes, secrets, expiration, licences, chemins Windows Unicode/sensibles à la casse et hébergement WPF/API.
- Adresses exactes de comparaison des candidats JSON (dont limites de taille des index texte) et éventuelle règle par champ pour traiter des listes comme ensembles.
- Validation humaine/délai d'affichage de `Inaccessible`, rapprochements d'épisodes externes couvrant plusieurs épisodes et seuil de suppression après absence vérifiée.

## 6. Conséquences documentaires

Les documents suivants peuvent nécessiter une harmonisation ultérieure, après validation de V2.2.1 :

1. `WATCH_TRACKING.md` : position relative à l'œuvre, parties avec offsets, états `EndedNormally`/`MediaEndReached`, règle de progression `Watched` et sélection d'association.
2. `METADATA_PROVIDERS.md` : upsert idempotent, clé d'équivalence, format canonique et conservation de la correction manuelle.
3. `MEDIA_SCANNER.md` : la règle des portées de scan de V2.2 demeure à harmoniser ; cette passe n'en change pas le comportement.
4. `METADATA_PROVIDERS.md` conserve également une priorité TMDB générale qui reste à concilier avec la priorité configurable par contenu ; la priorité anime Animeka n'est pas modifiée.
5. `ANIME_MANAGEMENT.md` évoque une catégorie/spécialisation Anime ; V2.2.1 conserve Anime comme classification et ne modifie pas les types structurels.

Aucun de ces documents n'a été modifié.

## 7. Conclusion

V2.2.1 précise les quatre sujets demandés sans ajouter d'entité ni commencer l'implémentation. Les propositions attendent la validation humaine. Aucun code C#, objet EF Core, `DbContext`, migration ou base SQLite n'a été créé.
