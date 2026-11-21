# MyVidLib — Notes de revue du modèle de données V2.2

**Date :** 21/11/2026
**Statut :** passe corrective documentaire — en attente de validation humaine ; aucune implémentation réalisée.

## 1. Corrections effectuées

- `DATA_MODEL.md` porte maintenant la version conceptuelle V2.2.
- Le rattachement série/épisode est précisé : la recommandation rend `Episode.SeriesWorkId` obligatoire et `Episode.SeasonId` facultatif. Les associations épisode/saison/arc doivent rester dans la même série.
- `MediaFileAssignment` distingue `Segment` (plusieurs épisodes dans un fichier), `Part` (un épisode réparti sur plusieurs fichiers) et `Alternative` (fichier alternatif complet). Les rôles imposent des données cohérentes : ordre requis pour un segment, numéro requis pour une partie, et absence de doublon actif par paire œuvre/fichier.
- `PlaybackOrder` et `PartNumber` ont une portée selon le rôle, et des règles d'unicité des affectations actives sont proposées.
- `WatchProgress` et `WatchSession` peuvent préserver le dernier fichier et l'association utilisée. Les positions sont proposées relatives à l'œuvre, converties via le début de segment.
- Les sessions de lecture ont des états explicites et restent récupérables après arrêt brutal, sans créer de lignes par pause/checkpoint.
- Les métadonnées sont modélisées comme candidats distincts ; les corrections manuelles sont de nouveaux candidats prioritaires, et la sélection est unique par sujet/champ/langue.
- `RelativePath` d'affichage est séparé de `NormalizedRelativePath`, clé canonique indexée en comparaison binaire.
- Les portées de scan enregistrent les répertoires réellement énumérés. Un scan annulé est persisté ; les absences ne sont conclues que pour les portées complètes.
- Les saisons distinguent numéro métier, ordre de tri interne et type ; les règles de numéros internes et externes sont séparées.
- Les diagrammes ajoutent explicitement les liens Provider/identifiants, sujets, candidats de métadonnées, séries/épisodes, arcs et portées de scan.
- Les limites SQLite/EF Core, notamment les `NULL` dans les index uniques, sont signalées.

## 2. Entités et propriétés ajoutées ou précisées

### Nouvelle entité proposée

- `MediaScanScope` : trace le résultat de l'énumération par répertoire afin de savoir où une absence peut ou ne peut pas être conclue. Elle est justifiée par les scans partiels, les erreurs de sous-dossier et l'annulation.

### Propriétés ajoutées ou précisées

- `MediaWork` (type `Episode`) : `SeriesWorkId` requis selon proposition, `SeasonId` facultatif ; les épisodes réutilisent `MediaWorkMetadata`.
- `Season` : `SeasonNumber`, `SortOrder`, `SeasonKind`.
- `MediaFile` : `NormalizedRelativePath`.
- `MediaFileAssignment` : `SegmentStartMs`, `SegmentEndMs`, rôles et portée de `PlaybackOrder`/`PartNumber`.
- `WatchProgress` / `WatchSession` : référence facultative à `MediaFileAssignment`, en plus du fichier.
- `WatchSession` : état, checkpoint, date de fin.
- Métadonnées typées : `MediaWorkMetadata` (toutes les œuvres TPH, épisodes inclus) et `SeasonMetadata`, avec `RawValue`, `CandidateValue`, `IsSelected`, `LocaleCode` non nul, provenance et dates de sélection/validation.

Aucune nouvelle entité `EpisodeGroup`, table d'historique de métadonnées, ni table Movie/Series/Episode séparée n'est introduite.

## 3. Statut des décisions

### Décisions structurantes VALIDÉES conservées

`MediaWork` commun en TPH avec `Movie`, `Series`, `Episode`; Anime comme classification extensible; `Season` distincte; `MediaVersion`, `MediaFile` et `MediaFileAssignment` distincts; `StoryArc`; absence de `EpisodeGroup` en V1; identifiants externes typés; mappings de références externes d'épisodes; progression par utilisateur/œuvre; état de racine indépendant de celui des fichiers; archivage logique; préservation des fichiers; priorité fournisseur configurable.

### Recommandations PROPOSÉES en V2.2

- rattachement obligatoire de chaque épisode à une série et saison facultative ;
- clés composites assurant la série commune de l'épisode, la saison et l'arc ;
- association des limites de segment et règles de rôle/ordre ;
- position en millisecondes relative à l'œuvre et références à l'association pour la reprise ;
- états `InProgress`, `Completed`, `Interrupted`, `Error` et réconciliation d'une session ouverte ;
- sélection d'une valeur de métadonnée par champ/langue, avec historique conservé sous forme de candidats ;
- normalisation des chemins via `ToUpperInvariant` et index binaire SQLite ;
- numéro 0 comme saison `Special`, saison `Unknown` avec numéro nul ;
- index uniques partiels et valeurs sentinelles pour neutraliser les limites des colonnes nulles ;
- `MediaScanScope` pour la couverture, y compris les scans `Cancelled`.

Ces détails ne sont pas validés par le propriétaire simplement parce qu'ils figurent dans le modèle.

## 4. Contraintes d'intégrité renforcées

- Clés composites entre `Episode.SeriesWorkId`, `Episode.SeasonId`, `Season.SeriesWorkId` et `StoryArc.SeriesWorkId`.
- Unicité active d'une association par couple œuvre/fichier, plus contraintes de rôle pour `Part`, `Segment` et `Primary`.
- Clé candidate de `MediaFileAssignment` `(MediaWorkId, MediaFileId, Id)` pour valider les références de reprise.
- Clé composite entre `MediaFileAssignment.MediaWorkId` et `MediaVersion`.
- Contraintes `CHECK` locales sur les bornes de segment, et invariants applicatifs pour la durée et les chevauchements.
- Index uniques partiels sur les numéros de saison/épisode non nuls, la saison inconnue, les sélections de métadonnées et les associations actives.
- Traçabilité par portée : seules les entrées absentes d'un répertoire dont l'énumération a réussi peuvent être marquées `Missing`.
- Les séances `InProgress` et les candidats de métadonnées conservent leurs historiques au lieu de créer une ligne par checkpoint ou d'écraser une valeur fournisseur.

## 5. Vérifications techniques restantes pour SQLite / EF Core

- **TPH et type référencé :** prototype nécessaire pour vérifier les clés candidates composites `(Id, WorkType)` et leurs FKs EF Core/SQLite.
- **Relations épisode/saison/arc :** vérifier la traduction des FKs composites partiellement nulles et des clés candidates.
- **Index uniques :** SQLite accepte les index partiels, mais la configuration EF Core et la génération de migrations d'index d'expression doivent être vérifiées.
- **Colonnes nullables :** SQLite traite plusieurs `NULL` comme distincts dans une clé unique. Les clés d'affectation facultatives nécessitent une expression `COALESCE` et une sentinelle réellement interdite, ou un invariant métier testé.
- **Métadonnées :** unicité partielle de `IsSelected` à tester pour chacune des deux tables typées (`MediaWorkMetadata`, qui couvre aussi les épisodes, et `SeasonMetadata`) ; `LocaleCode` doit rester non nul (`und` si inconnu).
- **Segments :** un `CHECK` local garantit l'ordre des deux bornes ; le dépassement de durée fichier, les chevauchements et la cohérence de l'ordre nécessitent un invariant applicatif.
- **Scans :** aucune contrainte SQL ne peut prouver qu'un répertoire a été réellement énuméré ; la logique de rapprochement doit consommer `MediaScanScope` et préserver les états antérieurs.
- **Chemins :** `COLLATE NOCASE` SQLite ne couvre pas le comportement Unicode de Windows. La clé `ToUpperInvariant` stockée avec comparaison binaire doit être vérifiée contre les règles Windows, ainsi que les répertoires à casse activée.

## 6. Décisions restant À ARBITRER avec le propriétaire

- obligation d'un `SeriesWorkId` pour tout épisode ;
- sémantique définitive de la saison spéciale 0 et des saisons inconnues ;
- nécessité et rôle métier exacts des limites `SegmentStartMs`/`SegmentEndMs` ;
- autorisation ou non de segments qui se chevauchent et priorité entre `PlaybackOrder`, `PartNumber` et bornes ;
- unité milliseconde et position relative à l'œuvre ;
- conservation des références d'association après archivage, politique de sessions ouvertes et délai de réconciliation après crash ;
- sélection prioritaire d'une valeur manuelle validée et effets d'un changement de priorité fournisseur ;
- comparateur de chemins Windows exact, cas Unicode et répertoires sensibles à la casse ;
- seuil éventuel ou confirmation humaine avant le passage à `Missing` après une absence vérifiée ;
- politique de suppression/anonymisation de l'historique utilisateur ;
- seuil `Watched`, priorité de lecture, sauvegardes, secrets, expiration des valeurs et licences fournisseur ;
- hébergement WPF/API pour M7.

## 7. Contradictions ou écarts restant avec les autres documents

1. `METADATA_PROVIDERS.md` présente TMDB comme priorité générale, tandis que `PROJECT_VISION.md` exige une priorité configurable par contenu. L'orientation Animeka pour les anime doit rester possible ; aucune priorité globale ne la remplace.
2. `ANIME_MANAGEMENT.md` évoque une catégorie ou spécialisation Anime. La V2.2 maintient les trois types structurels et recommande la classification extensible `Anime`.
3. `MEDIA_SCANNER.md` parle de fichiers qui deviennent `Missing` ou restent indexés sans préciser l'énumération par sous-dossier. V2.2 ajoute les portées complètes et la règle conservatrice, à réconcilier lors d'une future mise à jour documentaire.
4. `WATCH_TRACKING.md` rattache la position au fichier. V2.2 recommande une progression relative à l'œuvre, tout en conservant fichier et association utilisés. Ce changement est proposé et doit être validé avant toute implémentation.
5. Les documents sources ne spécifient pas les numéros de saison internes, la saison 0 ou l'appartenance des épisodes sans saison ; les règles V2.2 sont des propositions.

## 8. Conclusion

V2.2 consolide les associations et contraintes, mais n'élève aucune recommandation nouvelle au statut VALIDÉ. Les deux documents restent conceptuels et attendent la validation humaine. Aucun code C#, objet EF Core, `DbContext`, migration ou base SQLite n'a été créé.
