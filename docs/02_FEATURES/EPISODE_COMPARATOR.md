# MyVidLib - Comparateur d'épisodes

## 1. Objectif

Comparer les épisodes du catalogue externe avec les fichiers locaux et permettre une validation humaine des correspondances.

## 2. Description fonctionnelle

Le comparateur produit des états de possession, de visionnage et de numérotation pour chaque épisode. Il permet l'association manuelle et la résolution des épisodes doubles ou spéciaux.

## 3. Cas d'utilisation

- Lister les épisodes présents et absents.
- Filtrer les épisodes non visionnés.
- Comparer plusieurs catalogages.
- Associer un fichier à un épisode.
- Rejeter une association automatique.
- Représenter les épisodes doubles et spéciaux.

## 4. Données manipulées

`Episode`, `MediaFile`, `ExternalId`, métadonnées fournisseurs, `WatchProgress`.

## 5. Règles métier

- État local : `Present`, `Missing`, `PresentUnidentified`.
- État utilisateur : `Unwatched`, `InProgress`, `Watched`.
- État diffusion : `NotAired` lorsque connu.
- Une association incertaine reste à valider.
- Les différences de numérotation ne sont jamais déduites avec certitude sans règle documentée.
- Les épisodes doubles peuvent correspondre à plusieurs épisodes internes selon décision.

## 6. Interactions

- [Scanner](MEDIA_SCANNER.md).
- [Gestion des animés](ANIME_MANAGEMENT.md).
- [Fournisseurs](METADATA_PROVIDERS.md).
- [Suivi des visionnages](WATCH_TRACKING.md).

## 7. Contraintes techniques

- Algorithmes de correspondance séparés des règles UI.
- Scores de confiance visibles mais non persistés comme vérité absolue.
- Transitions d'association transactionnelles.

## 8. Gestion des erreurs

- Fichier sans métadonnée : état `PresentUnidentified`.
- Plusieurs candidats : demander validation.
- Catalogue fournisseur incomplet : afficher les limites sans supprimer les données locales.
- Interruption : ne pas enregistrer d'association partielle.

## 9. Critères d'acceptation

- Épisodes présents, absents et non identifiés distingués.
- Association manuelle persistante.
- Validation possible d'une correspondance incertaine.
- Épisodes spéciaux et doubles représentés sans perte d'information.
- Non visionnés filtrables.

## 10. Dépendances

Scanner, fournisseurs, données d'épisodes, suivi utilisateur.

## 11. Version prévue

M4.