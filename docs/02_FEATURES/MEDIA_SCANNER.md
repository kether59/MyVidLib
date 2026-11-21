# MyVidLib - Scanner multimédia

## 1. Objectif

Découvrir, indexer et surveiller les fichiers vidéo déclarés dans les dossiers sources.

## 2. Description fonctionnelle

Le scanner parcourt récursivement les extensions autorisées, collecte les métadonnées techniques via FFprobe, détecte les modifications et conserve un historique d'exécution.

## 3. Cas d'utilisation

- Lancer un scan manuel.
- Planifier un scan automatique.
- Détecter les fichiers nouveaux, modifiés, déplacés ou supprimés.
- Identifier les doublons probables.
- Consulter les erreurs par source.
- Annuler un scan en cours.

## 4. Données manipulées

`MediaFile`, `MediaRoot`, `ScanHistory`, empreinte de fichier, métadonnées techniques.

## 5. Règles métier

- Extensions prises en charge : AVI, MP4, MKV, MPEG, MPG, MOV, WMV, FLV, M4V, TS, M2TS, WEBM, VOB.
- Un fichier reste associé à sa source tant qu'il n'est pas explicitement retiré.
- Un fichier absent devient `Missing` ou reste indexé selon la décision de suppression à valider.
- Une empreinte détecte les doublons mais ne doit pas écraser une association existante.
- FFprobe est utilisé pour la durée, résolution, codecs, pistes audio/sous-titres et fréquence d'images.

## 6. Interactions

- [Bibliothèque locale](LOCAL_LIBRARY.md) pour les sources.
- [Modèle de données](../01_ARCHITECTURE/DATA_MODEL.md) pour persister.
- [Comparateur d'épisodes](EPISODE_COMPARATOR.md) pour les associations.
- [Lecteur vidéo](VIDEO_PLAYER.md) pour ouvrir les résultats.

## 7. Contraintes techniques

- Écriture progressive dans SQLite pour limiter la mémoire.
- Traitement asynchrone et limité en concurrence.
- `CancellationToken` propagé jusqu'au filesystem et FFprobe.
- Détection de changement basée sur chemin, taille, date et empreinte à arbitrer.
- Monitoring de fichiers optionnel après M1; il ne doit pas remplacer le scan manuel.

## 8. Gestion des erreurs

- FFprobe indisponible : journaliser, continuer l'indexation minimale.
- Fichier verrouillé : consigner une erreur temporaire.
- Chemin inaccessible : marquer la source indisponible.
- Processus interrompu : l'exécution reste non terminée ou échouée et peut être reprise.

## 9. Critères d'acceptation

- Scan récursif sur les extensions autorisées.
- Ajout, mise à jour et absence détectés.
- Historique avec compteurs.
- Annulation fiable.
- Aucun fichier original modifié.
- Analyse FFprobe disponible et contrôlée.

## 10. Dépendances

FFprobe, EF Core/SQLite, gestion des sources, logs, tests temporaires.

## 11. Version prévue

M1 - Bibliothèque locale.