# MyVidLib — Project Vision

**Nom du projet :** MyVidLib  
**Signification :** My Video Library (M.V.L.)  
**Type :** Gestionnaire personnel de bibliothèque multimédia Windows  
**Plateforme cible :** Windows 10 et Windows 11  
**Langage :** C#  
**Statut :** Projet en conception

---

## 1. Vision du produit

MyVidLib est une application Windows destinée à centraliser et organiser une bibliothèque personnelle de contenus multimédias.

Elle permet à un utilisateur de gérer ses films, séries télévisées et animés stockés sur différents supports locaux ou réseau, sans avoir à déplacer ou modifier leur emplacement d'origine.

L'application doit offrir une expérience moderne, visuelle et intuitive, inspirée des plateformes de streaming, tout en conservant la maîtrise complète de sa bibliothèque physique.

MyVidLib ne doit pas être un simple explorateur de fichiers multimédias.

Il doit devenir un véritable catalogue personnel permettant de savoir :

- Quels films et séries sont **référencés dans le catalogue** (existence d'une œuvre).
- Quels épisodes sont **possédés localement** (disponibilité d'un fichier physique).
- Quels épisodes restent à découvrir.
- Quels contenus ont été visionnés **par quel utilisateur** (état de visionnage individuel).
- Où se trouvent physiquement les fichiers (identification du fichier, support, chemin).
- Quelles informations sont associées à chaque œuvre (métadonnées enrichies, avec provenance).
- Quels contenus peuvent être consultés depuis d'autres appareils du réseau local.

> **Distinction fondamentale (VALIDÉ) :** MyVidLib sépare explicitement **l'œuvre** (entité logique du catalogue) du **fichier physique** (entité technique sur un support). Une même œuvre peut avoir plusieurs versions (qualités, langues, éditions) et plusieurs copies physiques. Un film ne devient pas une œuvre différente parce qu'il existe en plusieurs qualités ou langues.

## 2. Objectifs principaux

Les objectifs de MyVidLib sont :

1. **Centraliser** plusieurs sources multimédias dans une bibliothèque unique.
2. **Indexer automatiquement** les fichiers vidéo (via FFprobe / LibVLCSharp).
3. **Identifier et enrichir** les contenus avec des métadonnées externes (TMDB, IMDb, Animeka, Anime-Sama, etc.).
4. Organiser les films, séries, saisons et épisodes.
5. **Comparer** les épisodes référencés dans le catalogue avec les fichiers réellement possédés localement (présent / absent / non identifié / visionné).
6. **Suivre** les visionnages et les progressions individuelles par utilisateur.
7. **Lire** les vidéos directement depuis l'application (lecteur LibVLCSharp) et depuis un lecteur externe Windows.
8. **Partager** la bibliothèque sur le réseau local (serveur ASP.NET Core activable à la demande).
9. **Garantir la conservation** des fichiers originaux (jamais déplacé, renommé ou supprimé automatiquement).
10. Fournir une expérience moderne, visuelle et intuitive inspirée des plateformes de streaming, tout en conservant la maîtrise complète de la bibliothèque physique.

> **Ambiguïté résiduelle (PROPOSÉ) :** Les objectifs 5–6 doivent être opérationnels dès que le fichier correspondant est localisé ; ils ne s'appliquent pas si l'épisode n'est pas possédé.

## 3. Philosophie du produit

MyVidLib repose sur plusieurs principes fondamentaux.

### Maîtrise des fichiers

Les fichiers multimédias restent toujours à leur emplacement d'origine.

MyVidLib référence les fichiers et ne cherche pas à imposer une organisation physique particulière.

Aucun fichier original ne doit être déplacé, renommé ou supprimé automatiquement.

### Fonctionnement local prioritaire

Les fonctionnalités essentielles doivent rester disponibles sans connexion Internet.

Les services externes servent uniquement à enrichir et compléter les informations.

Une indisponibilité de TMDB, Animeka ou Anime-Sama ne doit jamais empêcher l'accès à la bibliothèque locale.

### Indépendance des fournisseurs

Les données internes de MyVidLib ne doivent pas dépendre d'un catalogue externe.

Une œuvre possède son propre identifiant interne et peut être associée à plusieurs identifiants externes.

### Fiabilité des correspondances

L'identification automatique doit privilégier la fiabilité.

Une correspondance incertaine doit être proposée à validation humaine plutôt que d'être enregistrée automatiquement.

### Traçabilité des données

Chaque information du catalogue (titre, synopsis, affiche, identifiant externe, etc.) doit conserver **sa provenance** : source externe (TMDB, Animeka, …), scan local, ou saisie manuelle.

MyVidLib doit distinguer les **données récupérées** (issues d'un fournisseur) des **données validées** (confirmées par l'utilisateur ou par une correspondance fiable).

En cas de **conflit entre plusieurs fournisseurs**, l'application doit appliquer une **priorité configurable** (par type de contenu), et conserver la **valeur précédente** ainsi que les différentes valeurs concurrentes en cas de correction manuelle ou de changement de priorité.

Toute valeur doit pouvoir être **corrigée manuellement** sans perdre la valeur d'origine (historique conservé).

### Robustesse des sources physiques

Une source physique (disque externe, partage réseau, dossier local) peut être **temporairement indisponible**. Dans ce cas :

- L'application doit **conserver les fichiers indexés** et ne jamais les supprimer automatiquement.
- L'indisponibilité doit être **signalée** clairement à l'utilisateur.
- Un fichier réellement supprimé doit être **détecté et signalé**, jamais effacé silencieusement.
- Un fichier déplacé doit être **proposé à l'association** (détection par empreinte), sans déplacer l'original.

> **Validé (confirmation implicite à demander) :** Le principe « une source indisponible ne doit jamais provoquer une suppression automatique de son contenu dans l'index » est considéré comme acquis. La modélisation technique exacte (état d'une source, temporisation avant marquage « supprimé ») reste **À DÉFINIR**.

### Progressivité

Le produit doit être construit par étapes successives.

Chaque fonctionnalité doit être réellement opérationnelle, testée et documentée avant de passer à la suivante.

## 4. Public et usages

MyVidLib est destiné à un usage personnel et familial.

Il doit permettre :

- Une gestion centralisée des contenus.
- Une consultation simple depuis un ordinateur Windows.
- Un accès aux contenus depuis d'autres appareils du réseau local.
- Un suivi de visionnage individuel.
- Une navigation confortable dans les grandes bibliothèques.

L'application doit rester accessible à un utilisateur lambda.

## 5. Catégories de contenus

MyVidLib gère trois grandes catégories.

### Films

Catalogue des films possédés, avec leurs informations, leurs affiches, leurs caractéristiques techniques et leur historique de visionnage.

> **Œuvre / fichier (VALIDÉ) :** Un film référencé dans le catalogue est une **œuvre**. Les différentes qualités (720p, 1080p, 4K), langues (VF, VOSTFR), éditions ou copies physiques multiples sont des **versions** ou des **copies** de cette même œuvre, et non des œuvres différentes. Chaque fichier physique reste rattaché à l'œuvre correspondante.

### Séries TV

Catalogue organisé en séries, saisons et épisodes.

Chaque épisode peut être associé à **un ou plusieurs fichiers physiques** (versions, parties multiples, copies), ainsi qu'à un état de visionnage **par utilisateur**.

Un épisode référencé mais **non possédé** reste dans le catalogue : il n'est ni supprimé ni considéré comme visionné.

### Animés et dessin animé

Catégorie dédiée aux animés japonais et productions assimilées.

Elle reprend l'organisation des séries tout en prenant en compte les particularités des catalogues d'animation :

- Titres français, japonais et anglais.
- Titres alternatifs.
- Saisons.
- Épisodes spéciaux.
- OAV.
- Films associés.
- Arcs narratifs.
- Versions VF et VOSTFR.

Les mêmes principes œuvre / version / fichier que pour les films et séries s'appliquent (versions multiples, épisodes découpés en parties, fichiers non identifiés).

## 6. Sources de métadonnées

MyVidLib doit pouvoir enrichir les informations locales avec plusieurs catalogues indépendants.

Fournisseurs envisagés :

- TMDB : catalogue généraliste de films et séries.
- IMDb : identifiants et références externes.
- Animeka : référence spécialisée importante pour les animés francophones.
- Anime-Sama : source complémentaire pour les animés.

> **Distinction des données (VALIDÉ) :** La provenance de chaque information doit être conservée. MyVidLib distingue les **données récupérées** (issues d'un fournisseur externe) des **données validées** (confirmées par l'utilisateur ou par une correspondance interne fiable). Toute donnée récupérée peut être corrigée manuellement, et la valeur précédente doit être conservée en historique.

La priorité des fournisseurs doit être **configurable selon le type de contenu** (par exemple, Animeka pour les animés, TMDB pour les films généraux).

Les informations doivent conserver leur provenance. Les droits d'utilisation, licences et conditions d'accès de chaque source doivent être respectés.

En cas de **conflit entre fournisseurs**, l'application doit appliquer la **priorité configurable** définie par l'utilisateur, tout en conservant les valeurs concurrentes potentielles et permettant une correction manuelle ultérieure.

## 7. Expérience utilisateur

L'application doit proposer une interface moderne inspirée des plateformes de streaming.

Principes visuels :

- Thème sombre.
- Affiches et images de fond.
- Navigation intuitive.
- Présentation sous forme de cartes.
- Recherche et filtres.
- Fiches détaillées.
- Indicateurs de progression.
- Navigation hiérarchique dans les séries et épisodes.

L'utilisateur doit pouvoir retrouver rapidement un contenu et comprendre son état de disponibilité et de visionnage.

## 8. Partage et utilisation multi-utilisateurs

MyVidLib doit pouvoir proposer un accès depuis plusieurs appareils du réseau local.

Chaque utilisateur possède son propre suivi de visionnage et son historique.

Deux profils sont prévus :

- Administrateur : configuration et gestion complète.
- Utilisateur : consultation et visionnage.

Le partage réseau est facultatif et doit être activé explicitement.

> **Séparation des responsabilités (VALIDÉ) :** Trois niveaux fonctionnels distincts doivent rester séparés :
>
> 1. **Configuration globale** : préférences communes (fournisseurs, priorités, format, apparence, comportements par défaut).
> 2. **Gestion commune de la bibliothèque** : sources déclarées, catalogue partagé, scan, enrichissement, associations.
> 3. **Préférences individuelles, historique individuel et progression individuelle** : chaque utilisateur possède son propre suivi de visionnage, son propre historique, ses propres marqueurs de progression et ses propres lectures.
>
> L'administrateur gère la partie 1 et 2 ; l'utilisateur gère la partie 3. Ce découpage doit être préservé dans l'implémentation (MVVM).

## 9. Principes techniques

MyVidLib est une application native Windows développée en C#.

Socle technique envisagé :

- C# 14.
- .NET 10 LTS.
- WPF.
- MVVM.
- ASP.NET Core.
- SQLite.
- Entity Framework Core.
- FFmpeg / FFprobe.
- LibVLCSharp.

L'architecture doit rester modulaire et évolutive.

La logique métier ne doit pas dépendre directement de l'interface graphique, du moteur de lecture ou d'un fournisseur externe.

## 10. Vision à long terme

MyVidLib doit progressivement devenir une solution complète de gestion multimédia personnelle.

La priorité reste la qualité de la bibliothèque locale, la fiabilité de l'indexation et la pertinence des métadonnées.

Les fonctionnalités avancées ne doivent pas compromettre la simplicité, les performances et la fiabilité du socle initial.

---

**Principe directeur : MyVidLib organise et valorise une bibliothèque multimédia existante sans jamais retirer à l'utilisateur la maîtrise de ses fichiers.**