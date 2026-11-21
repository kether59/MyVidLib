# MyVidLib - Fournisseurs de métadonnées

## 1. Objectif

Enrichir les œuvres locales depuis plusieurs fournisseurs tout en conservant la provenance et la capacité de fonctionnement hors ligne.

## 2. Description fonctionnelle

Le module fournit une recherche, des propositions d'identification, la récupération des fiches, saisons, épisodes et images, ainsi qu'une confirmation humaine.

## 3. Cas d'utilisation

- Rechercher une œuvre.
- Comparer plusieurs candidats.
- Confirmer ou rejeter une correspondance.
- Lier plusieurs identifiants externes.
- Désactiver un fournisseur.
- Consulter les données mises en cache hors ligne.

## 4. Données manipulées

`Provider`, `ExternalId`, `MediaMetadata`, métadonnées de films/séries/épisodes, cache d'images.

## 5. Règles métier

- TMDB est prioritaire pour les métadonnées générales jusqu'à arbitrage.
- IMDb fournit principalement des identifiants/références tant qu'une intégration complète n'est pas validée.
- Animeka et Anime-Sama sont associés aux animés.
- Toute correspondance incertaine reste `Proposed` jusqu'à validation.
- Les données locales précédemment récupérées restent disponibles hors ligne.
- La provenance doit rester liée à chaque valeur.
- Priorités configurables par type de contenu.

## 6. Interactions

- [Bibliothèque locale](LOCAL_LIBRARY.md).
- [Gestion des animés](ANIME_MANAGEMENT.md).
- [Comparateur d'épisodes](EPISODE_COMPARATOR.md).
- [Modèle de données](../01_ARCHITECTURE/DATA_MODEL.md).

## 7. Contraintes techniques

- Architecture par interface `IMetadataProvider`.
- `HttpClient` avec timeout, retries limités et cancellation.
- Cache HTTP/données à définir.
- Respect des licences, quotas et clés API.
- Identifiants externes séparés des entités internes.

## 8. Gestion des erreurs

- Fournisseur indisponible : conserver les données locales.
- Clé invalide : désactiver explicitement ou demander une configuration.
- Limite de requêtes : différer puis informer.
- Réponse invalide : journaliser et ne pas écraser les valeurs existantes.

## 9. Critères d'acceptation

- Recherche possible pour un fournisseur activé.
- Une proposition incertaine n'est pas enregistrée automatiquement comme certaine.
- Plusieurs fournisseurs peuvent coexister.
- Les données mises en cache restent accessibles hors ligne.
- Désactiver un provider ne casse pas la bibliothèque locale.

## 10. Dépendances

HttpClient, configuration, cache, Animeka/Anime-Sama selon M3/M4, consentement utilisateur.

## 11. Version prévue

M2 pour TMDB, M3 pour Animeka, M4 pour Anime-Sama.