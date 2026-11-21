# MyVidLib — Functional Scope

**Version :** 1.0  
**Statut :** Périmètre fonctionnel initial

## 1. Périmètre général

MyVidLib doit permettre de gérer une bibliothèque multimédia personnelle composée de fichiers vidéo stockés sur des supports locaux ou réseau.

Les fonctionnalités sont réparties en modules indépendants.

Le développement est progressif et suit une roadmap validée.

## 2. Gestion des sources

L'utilisateur peut :

- Ajouter plusieurs dossiers sources.
- Supprimer une source de l'index sans supprimer ses fichiers.
- Scanner récursivement les dossiers.
- Lancer un scan manuel.
- Surveiller les modifications.
- Détecter les nouveaux fichiers.
- Détecter les fichiers déplacés ou supprimés.
- Détection des déplacements et suppressions à sécuriser.
- Identifier les doublons.
- Consulter les erreurs de scan.

Les fichiers restent dans leur emplacement physique.

Formats pris en charge :

AVI, MP4, MKV, MPEG, MPG, MOV, WMV, FLV, M4V, TS, M2TS, WEBM, VOB.

### Comportement face aux sources indisponibles

Une source (disque externe, partage réseau, dossier local) peut être momentanément ou durablement indisponible. Le comportement fonctionnel attendu est le suivant :

- **Disque externe déconnecté** : les fichiers indexés restent dans l'index ; la source est marquée « indisponible » ; aucune suppression n'est déclenchée.
- **Partage réseau indisponible** : idem ; la source est conservée et son état d'indisponibilité est signalé, avec possibilité de réessayer.
- **Fichier temporairement inaccessible** (verrou, permission, lecture en cours) : le fichier n'est pas supprimé ; l'anomalie est journalisée.
- **Fichier réellement supprimé** : détection et signalement, jamais de suppression silencieuse ; l'utilisateur peut confirmer ou corriger.
- **Fichier déplacé** : détection (par empreinte / similarité) et proposition de réassociation à l'œuvre ou à l'épisode correspondant ; le fichier original n'est jamais déplacé par l'application.

> **VALIDÉ :** Une source indisponible ne doit jamais provoquer la suppression automatique de son contenu dans l'index. La représentation technique exacte de l'état d'une source et la temporisation avant un éventuel marquage « manquant » restent **À DÉFINIR**.

## 3. Indexation multimédia

Pour chaque fichier, récupérer :

- Nom.
- Chemin physique.
- Extension.
- Taille.
- Date de création ou modification lorsque disponible.
- Durée.
- Résolution.
- Codec vidéo.
- Codec audio.
- Nombre de pistes audio.
- Sous-titres disponibles.
- Fréquence d'images.

Utiliser FFprobe.

Prévoir une empreinte de fichier pour la détection des doublons.

> **Distinction (VALIDÉ) :** L'indexation et l'empreinte portent sur le **fichier physique**. Deux fichiers distincts (deux qualités, deux langues, deux copies) peuvent référencer la **même œuvre**. La détection de doublons doit donc distinguer :
>
> - un fichier strictement identique (même empreinte) rattaché à la même œuvre ;
> - des fichiers différents (qualités / langues / éditions) rattachés à la même œuvre en tant que **versions** ou **copies**.
>
> Le modèle technique exact (notion de version, de copie, de priorité de lecture) est **À DÉFINIR** : ici, seul le besoin fonctionnel est décrit.

## 4. Gestion des films

Une fiche film peut contenir :

- Titre.
- Titre original.
- Année.
- Synopsis.
- Affiche.
- Image de fond.
- Genres.
- Réalisateur.
- Distribution.
- Durée.
- Note.
- Identifiants externes.
- Fichiers associés.
- Statut de visionnage.

> **Œuvre / versions / fichiers (VALIDÉ) :** Un film référencé dans le catalogue est une **œuvre**. Les fichiers physiques associés et les versions (qualités, langues, éditions) constituent des éléments de cette œuvre, et non des œuvres distinctes. Plusieurs fichiers peuvent être associés à une même fiche film.

## 5. Gestion des séries

Une série contient :

- Informations générales.
- Saisons.
- Épisodes.
- Identifiants externes.
- Affiche et image de fond.
- Statut de diffusion.

Chaque épisode peut contenir :

- Numéro de saison.
- Numéro d'épisode.
- Titre.
- Synopsis.
- Date de diffusion.
- Durée.
- Miniature.
- Fichiers associés.
- Statut de visionnage par utilisateur.

Un épisode peut être référencé sans être possédé localement.

> **Relations épisode / fichiers (VALIDÉ) :** Fonctionnellement, un épisode doit permettre :
>
> - Plusieurs fichiers associés.
> - Plusieurs versions d'un même épisode.
> - Épisodes découpés en plusieurs parties.
> - Fichiers non identifiés.
> - Associations manuelles.
>
> Un épisode référencé mais non possédé ne doit pas être considéré comme supprimé ou visionné.

## 6. Gestion des animés

Les animés disposent d'une catégorie dédiée.

Prévoir :

- Titres français.
- Titres japonais.
- Titres anglais.
- Titres alternatifs.
- Saisons.
- Épisodes.
- OAV.
- Films associés.
- Arcs narratifs.
- Versions VF et VOSTFR.

La structure doit permettre de gérer les différences de catalogage entre fournisseurs.

> **Œuvre / versions / fichiers (VALIDÉ) :** Un anime référencé dans le catalogue est une **œuvre**. Les fichiers physiques et les versions (qualités, langues VF/VOSTFR, éditions) sont des composants de cette œuvre. Plusieurs fichiers peuvent être associés à une même fiche d'anime. Les principes œuvre / version / fichier s'appliquent de la même manière que pour les films et séries (versions multiples, épisodes découpés en parties, fichiers non identifiés).

## 7. Métadonnées externes

Fournisseurs prévus :

- TMDB.
- Animeka.
- Anime-Sama.
- IMDb.

Fonctionnalités :

- Recherche.
- Identification.
- Récupération des informations.
- Récupération des affiches.
- Récupération des images de fond.
- Récupération des saisons et épisodes.
- Conservation des identifiants externes.
- Conservation de la provenance.

Une association incertaine doit être soumise à validation.

Les fournisseurs peuvent être désactivés individuellement.

Les données déjà récupérées doivent rester accessibles hors ligne.

### Gestion de la provenance et des conflits

> **Traçabilité (VALIDÉ) :** Chaque information (titre, synopsis, affiche, identifiant externe, etc.) conserve **sa provenance** : fournisseur externe (TMDB, Animeka, …), scan local ou saisie manuelle. MyVidLib distingue les **données récupérées** (issues d'un fournisseur) des **données validées** (confirmées par l'utilisateur ou correspondance fiable).

> **Conflits entre fournisseurs (PROPOSÉ) :** En cas de conflit, l'application applique une **priorité configurable par type de contenu** (ex. Animeka pour les animés, TMDB pour les films). Les valeurs concurrentes et la valeur précédente sont conservées en cas de correction manuelle ou de changement de priorité. Toute valeur doit pouvoir être corrigée manuellement sans perdre l'historique.

> **Correction manuelle (PROPOSÉ) :** L'utilisateur peut corriger toute information ; la valeur précédente et sa provenance sont conservées en historique.

## 8. Comparateur d'épisodes

> **Indépendance catalogue / disponibilité / visionnage (VALIDÉ) :** L'existence d'une œuvre dans le catalogue, la disponibilité d'un fichier local et l'état de visionnage de chaque utilisateur sont des informations indépendantes. Un épisode référencé mais non possédé ne doit pas être considéré comme supprimé ou visionné.

Comparer les épisodes référencés avec les fichiers présents localement.

Statuts possibles :

- Présent localement.
- Absent localement.
- Présent mais non identifié.
- Visionné.
- Non visionné.
- Non encore diffusé.

Fonctionnalités :

- Liste des épisodes manquants.
- Filtrage des épisodes non visionnés.
- Comparaison des numérotations.
- Gestion des épisodes spéciaux.
- Gestion des épisodes doubles.
- Association manuelle.
- Validation des correspondances incertaines.

## 9. Suivi des visionnages

Statuts :

- Non visionné.
- En cours.
- Visionné.

> **Progression individuelle (VALIDÉ) :** Chaque utilisateur possède son propre historique de visionnage, ses propres marques, sa progression par épisode et par utilisateur. Les données de chaque utilisateur sont isolées dans l'historique individuel.

Fonctionnalités :

- Marquage manuel.
- Mise à jour pendant la lecture.
- Sauvegarde de la position.
- Reprise.
- Historique.
- Date du dernier visionnage.
- Progression par épisode.
- Progression par utilisateur.

## 10. Lecture vidéo

Lecteur intégré basé sur LibVLCSharp.

Fonctionnalités :

- Lecture.
- Pause.
- Reprise.
- Avance et retour rapides.
- Volume.
- Plein écran.
- Sélection audio.
- Sélection des sous-titres.
- Reprise automatique.
- Épisode suivant.

Prévoir également l'ouverture avec un lecteur externe Windows.

## 11. Interface Windows

Application WPF avec MVVM.

Écrans prévus :

1. Accueil.
2. Films.
3. Séries.
4. Animés.
5. Épisodes à regarder.
6. Épisodes manquants.
7. Historique.
8. Bibliothèque.
9. Statistiques.
10. Paramètres.

Fonctionnalités d'interface :

- Navigation latérale.
- Recherche.
- Filtres.
- Tri.
- Affiches.
- Fiches détaillées.
- Progression de visionnage.
- Indicateurs de disponibilité locale.

## 12. Partage réseau

Serveur ASP.NET Core activable à la demande.

Fonctionnalités :

- Interface web responsive.
- Consultation des catalogues.
- Recherche.
- Consultation des fiches.
- Lecture vidéo HTTP.
- Support des requêtes HTTP Range.
- Gestion des utilisateurs.
- Suivi individuel.

Sécurité :

- Authentification.
- Permissions.
- Restriction aux dossiers déclarés.
- Aucun accès arbitraire au système de fichiers.
- Aucun accès Internet exposé automatiquement.

## 13. Utilisateurs

Deux profils :

Administrateur :
- Configuration.
- Gestion des sources.
- Gestion des utilisateurs.
- Gestion complète de la bibliothèque.

Utilisateur :
- Consultation.
- Lecture.
- Gestion de son suivi de visionnage.

> **Séparation des responsabilités (VALIDÉ) :** Trois niveaux fonctionnels distincts doivent rester séparés :
>
> 1. **Configuration globale** : préférences communes (fournisseurs, priorités, format, apparence, comportements par défaut).
> 2. **Gestion commune de la bibliothèque** : sources déclarées, catalogue partagé, scan, enrichissement, associations.
> 3. **Préférences individuelles, historique individuel et progression individuelle** : chaque utilisateur possède son propre suivi de visionnage, son propre historique, ses propres marqueurs de progression et ses propres lectures.
>
> L'administrateur gère la partie 1 et 2 ; l'utilisateur gère la partie 3. Ce découpage doit être préservé dans l'implémentation (MVVM).

Chaque utilisateur possède son propre historique.

## 14. Configuration et maintenance

Fonctionnalités :

- Gestion des dossiers.
- Configuration TMDB.
- Activation des fournisseurs.
- Configuration réseau.
- Gestion des utilisateurs.
- Choix du lecteur.
- Scan manuel.
- Synchronisation.
- Consultation des logs.
- Sauvegarde.
- Restauration.

### Objectifs de sauvegarde et de restauration

> **Sauvegarde (VALIDÉ) :** Les objectifs de sauvegarde et de restauration sont :
>
> 1. **Base de données** (catalogue, références, associations).
> 2. **Configuration** (préférences globales, fournisseurs, priorités).
> 3. **Historique et progression** individuels (visionnage, marqueurs par utilisateur).
> 4. **Métadonnées locales** (données enrichies, provenance, identifiants externes).
> 5. **Images mises en cache** (affiches, images de fond, miniatures).
>
> La sauvegarde ne doit **pas** supposer que les fichiers vidéo eux-mêmes sont copiés : ils restent à leur emplacement physique d'origine et ne font pas l'objet d'une copie de sauvegarde par l'application.

> **Restauration (PROPOSÉ) :** Restaurer la base de données, la configuration et les métadonnées depuis une sauvegarde doit reconstruire l'index sans toucher aux fichiers vidéo originaux.

## 15. Contraintes non fonctionnelles

- Fonctionnement local hors ligne.
- Interface non bloquante.
- Traitements asynchrones.
- Gestion des annulations.
- Gestion centralisée des erreurs.
- Journalisation.
- Tests automatisés.
- Protection des fichiers originaux.
- Robustesse face aux interruptions.
- Architecture évolutive.
- Performances adaptées aux grandes bibliothèques.

## 16. Hors périmètre

MyVidLib n'a pas vocation à :

- Télécharger automatiquement des films ou séries.
- Télécharger des flux vidéo depuis des sites tiers.
- Remplacer les fichiers originaux.
- Organiser physiquement les dossiers de l'utilisateur sans demande explicite.
- Exposer automatiquement la bibliothèque sur Internet.
- Dépendre d'un service cloud pour fonctionner localement.

## 17. Règles de réalisation

Chaque module doit être développé progressivement.

Une fonctionnalité ne peut être déclarée terminée que si elle est réellement implémentée et testée.

Les fonctionnalités futures peuvent être préparées architecturalement, mais ne doivent pas être simulées.

Les modifications du périmètre doivent être documentées et validées.