# MyVidLib - Bibliothèque locale

## 1. Objectif

Permettre à l'utilisateur d'ajouter, gérer et supprimer des dossiers sources sans déplacer ni modifier les fichiers multimédias.

## 2. Description fonctionnelle

L'utilisateur configure plusieurs `MediaRoot`, lance des scans et consulte les fichiers indexés. Une source peut être locale ou réseau. La suppression d'une source retire son contenu de l'index, pas du disque.

## 3. Cas d'utilisation

- Ajouter une source après vérification de son existence.
- Modifier le nom ou activer/désactiver une source.
- Retirer une source sans suppression physique.
- Lister les fichiers disponibles et manquants.
- Rechercher et filtrer la bibliothèque.

## 4. Données manipulées

`MediaRoot`, `MediaFile`, `ScanHistory`, `Movie`, `Series`, `Episode`, paramètres de scan.

## 5. Règles métier

- Les chemins physiques ne sont jamais modifiés.
- Une source désactivée n'est pas incluse dans les scans automatiques.
- Une suppression depuis MyVidLib ne supprime jamais un fichier source.
- Les chemins sont normalisés et ne doivent pas quitter les racines déclarées pour les opérations réseau.
- Les doublons peuvent référencer plusieurs œuvres selon décision du modèle de données.

## 6. Interactions

- [Scanner multimédia](MEDIA_SCANNER.md) pour découvrir les fichiers.
- [Vidéo player](VIDEO_PLAYER.md) pour ouvrir un fichier local.
- [Comparateur](EPISODE_COMPARATOR.md) pour associer les épisodes.
- [Suivi](WATCH_TRACKING.md) pour historiser la lecture.

## 7. Contraintes techniques

- Accès réseau asynchrone avec annulation et délai.
- Chemins de longueur Windows gérés selon les capacités du runtime.
- Indexation en base SQLite.
- Interface non bloquante pendant les longues opérations.

## 8. Gestion des erreurs

- Source inexistante ou inaccessible : enregistrer l'état et afficher un message explicite.
- Permission refusée : continuer les autres dossiers.
- Chemins invalides : rejeter la configuration avant ajout.
- Changement réseau : marquer les fichiers comme indisponibles, sans suppression immédiate.

## 9. Critères d'acceptation

- Plusieurs sources peuvent coexister.
- Une source supprimée ne modifie aucun fichier.
- Les fichiers sont retrouvés après redémarrage.
- Le scan d'une source indisponible n'empêche pas les autres.
- La recherche et les filtres de base fonctionnent.

## 10. Dépendances

Architecture, modèle de données, scanner, interface WPF, EF Core/SQLite.

## 11. Version prévue

M1 - Bibliothèque locale.