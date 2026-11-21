# MyVidLib - Gestion des animés

## 1. Objectif

Gérer les particularités des catalogues d'animation avec plusieurs titres, catalogages, saisons, épisodes spéciaux et versions linguistiques.

## 2. Description fonctionnelle

Les animés utilisent une spécialisation de `Series` ou une catégorie dédiée permettant titres alternatifs, arcs, OAV, films associés et numérotations de fournisseurs.

## 3. Cas d'utilisation

- Ajouter un animé depuis TMDB ou un fournisseur spécialisé.
- Gérer les titres français, japonais et anglais.
- Associer des saisons, OAV et films.
- Gérer les épisodes spéciaux.
- Séparer ou associer VF et VOSTFR.

## 4. Données manipulées

`Series`, `Season`, `Episode`, `MediaMetadata`, `ExternalId`, `MediaFile`, alternatives de titre.

## 5. Règles métier

- Le contenu anime conserve une identité interne indépendante des fournisseurs.
- Les titres alternatifs sont conservés comme métadonnées, pas écrasés.
- VF/VOSTFR peuvent représenter des versions distinctes ou des pistes de fichier selon une décision ultérieure.
- Les OAV et films associés ne doivent pas être comptés comme épisodes standard.
- Les écarts de numérotation sont conservés et visibles.

## 6. Interactions

- [Fournisseurs](METADATA_PROVIDERS.md).
- [Comparateur d'épisodes](EPISODE_COMPARATOR.md).
- [Bibliothèque locale](LOCAL_LIBRARY.md).
- [Suivi](WATCH_TRACKING.md).

## 7. Contraintes techniques

- Modèle extensible pour plusieurs catalogages.
- Pas de hypothèse unique de numérotation.
- Recherche insensible aux accents recommandée.
- Catalogues externes utilisés comme sources distinctes.

## 8. Gestion des erreurs

- Fournisseur incomplet : conserver les informations locales.
- Numéros ambigus : marquer pour validation humaine.
- Doublons de titres : proposer plusieurs candidats.
- OAV non reconnu : conserver comme épisode spécial/inconnu.

## 9. Critères d'acceptation

- Titres alternatifs visibles.
- Saisons, épisodes spéciaux, OAV et films distingués.
- Plusieurs sources peuvent identifier le même animé.
- Aucun écrasement automatique d'un catalogue différent.

## 10. Dépendances

Animeka, Anime-Sama, modèle multi-sources, comparateur.

## 11. Version prévue

M3 pour Animeka, M4 pour Anime-Sama.