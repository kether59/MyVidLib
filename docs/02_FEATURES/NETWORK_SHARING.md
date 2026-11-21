# MyVidLib - Partage réseau

## 1. Objectif

Activer un accès local optionnel aux catalogues et fichiers via ASP.NET Core, limité au réseau configuré.

## 2. Description fonctionnelle

Le serveur expose une interface web responsive avec authentification, autorisations, consultation du catalogue, recherche et lecture vidéo HTTP avec support Range.

## 3. Cas d'utilisation

- Activer/désactiver le serveur.
- Se connecter avec un profil.
- Consulter les films, séries et animés.
- Rechercher les fichiers disponibles.
- Lire une vidéo depuis un autre appareil.
- Administer les utilisateurs et sources.

## 4. Données manipulées

`User`, catalogue, `MediaFile`, `WatchProgress`, `WatchHistory`, configuration réseau.

## 5. Règles métier

- Le partage est désactivé par défaut.
- L'administrateur gère les sources, utilisateurs et configuration.
- L'utilisateur consulte, lit et gère uniquement son suivi.
- Aucun chemin arbitraire n'est exposé.
- La lecture est limitée aux MediaRoots autorisés.
- Aucun port Internet n'est ouvert automatiquement.
- HTTP Range obligatoire pour les fichiers volumineux.

## 6. Interactions

- Tous les modules métier via les cas d'utilisation partagés.
- Base SQLite locale.
- Configuration locale et journalisation.
- Interface WPF pour activation et administration.

## 7. Contraintes techniques

- ASP.NET Core.
- Authentification et permissions à définir.
- Protection contre le traversal de chemin.
- Timeouts, taille de cache et streaming à définir.
- Limitation d'accès réseau à l'interface locale recommandée.
- HTTPS local à valider.

## 8. Gestion des erreurs

- Port indisponible : conserver le serveur arrêté et afficher l'erreur.
- Fichier absent : réponse lisible et mise à jour du statut.
- Session invalide : refuser l'accès.
- Permission insuffisante : ne pas révéler l'existence du fichier.

## 9. Critères d'acceptation

- Interface web responsive.
- Consultation et recherche opérationnelles.
- HTTP Range vérifié par tests.
- Utilisateur isolé de la configuration administrative.
- Aucun accès hors des dossiers déclarés.
- Serveur local désactivable.

## 10. Dépendances

ASP.NET Core, authentification, modèle utilisateur, catalogue, suivi, lecteur web futur.

## 11. Version prévue

M7.