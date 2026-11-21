# MyVidLib - Lecteur vidéo

## 1. Objectif

Lire les fichiers locaux dans MyVidLib avec LibVLCSharp, tout en proposant l'ouverture dans un lecteur externe Windows.

## 2. Description fonctionnelle

Le lecteur gère lecture/pause, positions, volume, plein écran, pistes audio/sous-titres, reprise et épisode suivant.

## 3. Cas d'utilisation

- Lire un fichier.
- Mettre en pause ou reprendre.
- Avancer/reculer rapidement.
- Choisir une piste audio ou un sous-titre.
- Lire en plein écran.
- Reprendre la position.
- Passer à l'épisode suivant disponible.

## 4. Données manipulées

`MediaFile`, `WatchProgress`, `WatchHistory`, préférences de lecture, pistes VLC.

## 5. Règles métier

- Le lecteur ne modifie pas le fichier source.
- Une reprise utilise uniquement une progression appartenant à l'utilisateur actif.
- L'épisode suivant est limité aux épisodes associés et présents localement.
- Lecture externe Windows conservée comme alternative.
- HTTP Range est hors périmètre du lecteur intégré WPF.

## 6. Interactions

- [Suivi des visionnages](WATCH_TRACKING.md).
- [Bibliothèque locale](LOCAL_LIBRARY.md).
- [Comparateur d'épisodes](EPISODE_COMPARATOR.md).
- [Partage réseau](NETWORK_SHARING.md) pour la lecture HTTP ultérieure.

## 7. Contraintes techniques

- LibVLCSharp obligatoire pour M6.
- Runtime VLC distribué ou requis à définir.
- Lecture asynchrone et gestion du cycle de vie WPF.
- Support des formats pris en charge selon les codecs du runtime.
- Mise à jour de progression non bloquante.

## 8. Gestion des erreurs

- Codec absent : afficher une erreur lisible.
- Fichier supprimé : retirer ou marquer indisponible.
- VLC indisponible : proposer l'ouverture externe.
- Interruption : sauvegarder la dernière position connue.

## 9. Critères d'acceptation

- Lecture/pause/reprise opérationnelles.
- Volume, avance rapide, plein écran et sélection des pistes disponibles.
- Reprise automatique selon la préférence.
- Épisode suivant fonctionnel pour une série.
- Ouvrir avec un lecteur Windows disponible.

## 10. Dépendances

LibVLCSharp, MediaFile, suivi des visionnages, codec/licences VLC.

## 11. Version prévue

M6.