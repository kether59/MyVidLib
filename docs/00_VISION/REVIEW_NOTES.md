# MyVidLib — Review Notes (Revue des documents fondateurs)

**Date :** 21/11/2026  
**Auteur :** Particle-1 (revue fonctionnelle assistée)  
**Périmètre :** Consolidation des documents fondateurs `PROJECT_VISION.md` et `FUNCTIONAL_SCOPE.md`  
**Nature :** Revue fonctionnelle et documentaire — aucun développement logiciel, aucune modification des autres documents d'architecture.

---

## 1. Statuts utilisés

- **VALIDÉ** : décision explicitement confirmée (ou reconduction d'un principe déjà acquis).
- **PROPOSÉ** : recommandation à examiner.
- **À DÉFINIR** : décision fonctionnelle encore nécessaire.
- **FUTUR** : fonctionnalité envisagée pour une étape ultérieure.

> Aucune proposition n'a été transformée en décision validée sans confirmation explicite. Les éléments marqués « VALIDÉ » dans les documents correspondent à des principes déjà présents / confirmés par le propriétaire.

## 2. Philosophie préservée

Les consolidations respectent les principes initiaux :

- Fonctionnement local prioritaire (hors ligne).
- Protection des fichiers originaux (jamais déplacés / renommés / supprimés).
- Choix technique conservé : C# / .NET 10 / WPF / MVVM.
- Aucune fonctionnalité nouvelle non demandée n'a été ajoutée.
- Aucune décision d'implémentation n'a été imposée dans les documents fonctionnels.

## 3. Modifications réalisées — PROJECT_VISION.md

1. **Vision (§1)** : états « référencé / possédé / visionné par utilisateur / identifié » explicités ; ajout d'un encadré **« Distinction fondamentale (VALIDÉ) »** œuvre ↔ fichier physique (versions, copies).
2. **Objectifs (§2)** : reformulation en 10 objectifs précis ; ajout d'une note sur le caractère conditionnel de la comparaison / du suivi quand un fichier n'est pas possédé.
3. **Philosophie (§3)** : ajout des sections :
   - **Traçabilité des données** (provenance, récupérées vs validées, conflits, priorité configurable, correction manuelle).
   - **Robustesse des sources physiques** (source indisponible ≠ suppression ; fichier supprimé / déplacé signalé).
4. **Catégories de contenus (§5)** : précision œuvre / version / fichier pour les films, séries et animés ; note sur les épisodes non possédés.
5. **Sources de métadonnées (§6)** : distinction données récupérées / validées, priorité configurable par type de contenu, gestion des conflits.
6. **Partage multi-utilisateurs (§8)** : ajout de l'encadré **« Séparation des responsabilités (VALIDÉ) »** (configuration globale / gestion commune / préférences-historique-progression individuels).

## 4. Modifications réalisées — FUNCTIONAL_SCOPE.md

1. **§2 Gestion des sources** : ajout de « Comportement face aux sources indisponibles » (5 cas : disque déconnecté, partage réseau indisponible, fichier inaccessible, fichier supprimé, fichier déplacé) ; règle VALIDÉ : aucune suppression automatique.
2. **§3 Indexation multimédia** : encadré « Distinction (VALIDÉ) » fichiers / versions / copies, empreinte sur le fichier physique, modèle technique À DÉFINIR.
3. **§4 Films** : « Fichier associé » → « Fichiers associés » ; encadré œuvre / versions / fichiers.
4. **§5 Séries** : « Fichiers associés », « Statut de visionnage par utilisateur » ; ajout de « Relations épisode / fichiers (VALIDÉ) » (multi-fichiers, versions, parties multiples, fichiers non identifiés, associations manuelles).
5. **§6 Animés** : encadré œuvre / versions / fichiers analogue.
6. **§7 Métadonnées externes** : ajout de « Gestion de la provenance et des conflits » (traçabilité, conflits, correction manuelle).
7. **§8 Comparateur d'épisodes** : encadré d'indépendance catalogue / disponibilité / visionnage.
8. **§9 Suivi des visionnages** : encadré « Progression individuelle ».
9. **§13 Utilisateurs** : ajout de « Séparation des responsabilités (VALIDÉ) » (3 niveaux).
10. **§14 Configuration et maintenance** : ajout de « Objectifs de sauvegarde et de restauration » (BDD, config, historique / progression, métadonnées locales, images en cache ; PAS les fichiers vidéo).

## 5. Couverture des points demandés (1.1 → 1.7)

| Point | Objet | Où traité |
|---|---|---|
| 1.1 | Œuvres vs fichiers vs versions vs copies | Vision §1, §5 ; Scope §3, §4, §5, §6 |
| 1.2 | Catalogue / disponibilité / visionnage indépendants | Vision §1, §2 ; Scope §5, §8 |
| 1.3 | Relations épisodes ↔ fichiers (multi, versions, parties, non identifiés, manuel) | Vision §5 ; Scope §5 |
| 1.4 | Sources physiques (déconnecté, réseau indisponible, inaccessible, supprimé, déplacé) | Vision §3 ; Scope §2 |
| 1.5 | Métadonnées (provenance, conflits, priorité, correction, historique, récupéré vs validé) | Vision §3, §6 ; Scope §7 |
| 1.6 | Multi-utilisateurs (global / commun / individuel) | Vision §8 ; Scope §13 |
| 1.7 | Sauvegarde (BDD, config, historique, métadonnées, cache ; pas les vidéos) | Scope §14 |

## 6. Ambiguïtés restantes

1. **Modèle œuvre / version / copie / fichier** : le besoin est décrit, mais la nomenclature exacte reste **À DÉFINIR** (qu'est-ce qu'une « version » vs une « copie » ? Une qualité différente est-elle une version ou une copie ? Une VF et une VOSTFR sont-elles deux versions ou deux fichiers d'une même version ?).
2. **Détection « fichier réellement supprimé »** : la différence entre « inaccessible temporairement » et « supprimé » dépend d'un seuil / temporisation encore **À DÉFINIR**.
3. **Priorité de lecture** : quand plusieurs fichiers valides existent pour une même œuvre / épisode, lequel est proposé par défaut ? **À DÉFINIR**.
4. **Conflits de métadonnées** : comportement quand un fournisseur prioritaire renvoie une valeur vide ou aberrante — **À DÉFINIR / PROPOSÉ**.
5. **Statut de diffusion d'un épisode** vs disponibilité locale : l'articulation « Non encore diffusé » avec un épisode possédé avant diffusion n'est pas spécifiée — **À DÉFINIR**.
6. **Périmètre des « images mises en cache »** dans la sauvegarde : toutes les images ou seulement les vignettes générées localement ? **À DÉFINIR**.
7. **Version de .NET** : « .NET 10 » dans `PROJECT_VISION.md` vs « .NET 10 LTS » dans le socle technique — à confirmer / harmoniser (hors périmètre de cette revue).

## 7. Décisions proposées (PROPOSÉ)

1. **PROPOSÉ** : conserver systématiquement l'historique des valeurs corrigées (métadonnées) avec provenance et date.
2. **PROPOSÉ** : appliquer une priorité configurable par type de contenu (anime → Animeka ; film / série → TMDB), avec neutralité si le fournisseur prioritaire ne fournit rien.
3. **PROPOSÉ** : marquer une source « indisponible » sans changer l'état du catalogue ; ne jamais marquer un fichier « supprimé » avant confirmation ou seuil explicite.
4. **PROPOSÉ** : permettre plusieurs fichiers par œuvre / épisode, avec notion de version / copie séparée des métadonnées de catalogue.
5. **PROPOSÉ** : exclure par principe les fichiers vidéo de la sauvegarde de l'application.
6. **PROPOSÉ** : isoler l'historique et la progression par utilisateur ; la bibliothèque reste commune.

## 8. Conflits entre documents

Aucun conflit bloquant détecté entre `PROJECT_VISION.md` et `FUNCTIONAL_SCOPE.md`. Points à surveiller :

1. **Version .NET** : `PROJECT_VISION.md` mentionne « .NET 10 LTS » (socle technique §9) ; d'autres documents d'architecture peuvent mentionner « .NET 10 ». À harmoniser (hors périmètre de cette revue — ne pas modifier les autres documents).
2. **Statut / version des documents** : `FUNCTIONAL_SCOPE.md` conserve « Statut : Périmètre fonctionnel initial » et « Version : 1.0 ». Une mise à jour de version / statut est à décider par le propriétaire (**À DÉFINIR**).

## 9. Questions pour le propriétaire du projet

1. La distinction « versions » vs « copies » doit-elle reposer sur la qualité / édition uniquement, ou aussi sur la langue (VF / VOSTFR) ?
2. Quels critères confirment qu'un fichier est « réellement supprimé » (seuil, vérification manuelle, empreinte) ?
3. La priorité de lecture par défaut (qualité max ? langue préférée ? dernier ajouté ?) doit-elle être configurable par utilisateur ou globale ?
4. Les images en cache doivent-elles toutes être sauvegardées, ou faut-il privilégier la régénération au détriment de la sauvegarde ?
5. Confirmez-vous que les fichiers vidéo n'entrent jamais dans la sauvegarde de l'application ?
6. Souhaitez-vous que les encadrés marqués « PROPOSÉ » soient transformés en VALIDÉ après votre validation ?
7. Le statut / version des documents fondateurs doit-il être mis à jour (ex. « Version 1.1 — consolidée ») ?

---

**Fin de la revue.** Aucun code applicatif n'a été généré ou modifié ; seuls les deux documents fondateurs et ce fichier de revue ont été touchés.

