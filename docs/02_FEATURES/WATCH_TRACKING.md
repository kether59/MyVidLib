# MyVidLib - Suivi des visionnages

## 1. Objectif

Enregistrer l'état de visionnage, la position de lecture et l'historique individuellement pour chaque utilisateur.

## 2. Description fonctionnelle

Le suivi accepte un marquage manuel ou une mise à jour du lecteur. Il conserve une progression par utilisateur pour les films, épisodes et éventuellement les fichiers associés.

## 3. Cas d'utilisation

- Marquer comme visionné/non visionné.
- Sauvegarder une position.
- Reprendre une lecture.
- Consulter l'historique.
- Voir la progression d'un épisode ou d'une œuvre.

## 4. Données manipulées

`User`, `WatchHistory`, `WatchProgress`, `MediaFile`, `Movie`, `Episode`.

## 5. Règles métier

- États : `Unwatched`, `InProgress`, `Watched`.
- Chaque profil dispose d'un suivi indépendant.
- La position doit être liée à l'utilisateur et au fichier réellement lu.
- Une reprise automatique peut être désactivée dans les paramètres.
- Le marquage manuel peut remplacer une progression calculée.
- Les seuils automatiques de visionnage restent à définir.

## 6. Interactions

- [Lecteur vidéo](VIDEO_PLAYER.md).
- [Comparateur](EPISODE_COMPARATOR.md).
- [API réseau](NETWORK_SHARING.md).
- [Bibliothèque](LOCAL_LIBRARY.md).

## 7. Contraintes techniques

- Mise à jour rapide et atomique.
- Progression persistée périodiquement pendant la lecture.
- Concurrence multi-utilisateurs préparée avant M7.
- Temps en ticks ou millisecondes, unité à définir.

## 8. Gestion des erreurs

- Échec de sauvegarde : conserver en mémoire temporairement puis retry ou informer.
- Fichier non associé : permettre le suivi du fichier seulement si explicitement décidé.
- Historique dupliqué : dédupliquer selon règles à définir.

## 9. Critères d'acceptation

- Deux profils ont des progressions distinctes.
- Position sauvegardée et reprise.
- Historique consultable.
- Marquage manuel persistent.
- Non visionnés filtrables.

## 10. Dépendances

Utilisateurs, lecteur, modèle de données.

## 11. Version prévue

M5.