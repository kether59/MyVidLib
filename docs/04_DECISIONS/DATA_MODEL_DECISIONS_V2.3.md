# **MyVidLib — Compte rendu consolidé des 18 arbitrages**

## **1. Modèle des œuvres et des fichiers**

### **1.1 — Représentation des saisons**

Validé

| Situation        | SeasonKind | SeasonNumber |
| ---------------- | ---------- | ------------ |
| Saison ordinaire | `Normal`   | 1, 2, 3…     |
| OAV / spécial    | `Special`  | 0            |
| Saison inconnue  | `Unknown`  | `null`       |

- Une saison technique `Special` et une saison technique `Unknown` au maximum par série.
- Un épisode dont la saison est inconnue est rattaché à la saison `Unknown`.
- Les épisodes spéciaux restent des épisodes du modèle commun `MediaWork`.

### **1.2 — Un seul fichier physique par œuvre**

Validé

C'est une décision structurante :

- Une œuvre possède un seul fichier principal actif.
- Plusieurs copies physiques peuvent être détectées et recensées comme doublons.
- Une seule copie est retenue comme référence principale pour la lecture.
- Les copies supplémentaires peuvent être conservées, retirées ou supprimées physiquement par l'administrateur.

## **2. Catalogue et fournisseurs externes**

### **2.1 — Références externes couvrant plusieurs épisodes**

Validé

Approche hybride :

- Une référence externe peut correspondre à un épisode ou à plusieurs épisodes MyVidLib.
- Les correspondances certaines peuvent être automatiques.
- Les correspondances ambiguës nécessitent une validation humaine.
- La numérotation originale de chaque fournisseur est conservée.
- Aucun épisode artificiel n'est créé uniquement pour résoudre un conflit de numérotation.

Priorités de sources :

- Animeka en priorité pour les animés.
- TMDB en complément pour les animés.
- TMDB en priorité pour les films et séries.
- Autres sources complémentaires selon leurs possibilités et leurs conditions d'utilisation.

Les priorités devront rester configurables.

### **2.2 — Sélection des métadonnées**

Validé

Approche hybride :

- Priorité configurable par fournisseur.
- Conservation des valeurs d'origine et de leur provenance.
- Conservation des dates de récupération.
- Une donnée validée manuellement ne peut pas être écrasée automatiquement.
- Les conflits importants sont soumis à validation.

### **2.3 — Actualisation des métadonnées**

Validé

- Actualisation périodique mensuelle.
- Actualisation manuelle possible à tout moment.
- Pas d'écrasement des valeurs validées manuellement.
- Pas de suppression des métadonnées existantes en cas d'échec d'un fournisseur.

### **2.4 — Images et licences**

Validé

- Usage personnel uniquement.
- Téléchargement et conservation locale des images lorsque les conditions du fournisseur le permettent.
- Conservation de la provenance.
- Pas de redistribution publique.
- Respect des licences et conditions d'utilisation propres à chaque fournisseur.

## **3. Stockage et disponibilité des fichiers**

### **3.1 — Normalisation des chemins Windows**

Validé

- Chemins absolus normalisés.
- Séparateurs harmonisés.
- Normalisation Unicode NFC.
- Comparaison insensible à la casse par défaut.
- Respect des dossiers Windows configurés avec une sensibilité à la casse.
- Prise en charge des lecteurs amovibles et des chemins UNC.
- Conservation du chemin d'affichage original.

### **3.2 — Détection des fichiers manquants**

Validé

Un fichier est marqué `Missing` uniquement après :

- Trois scans complets consécutifs sans le retrouver.
- Au moins 24 heures depuis la première absence confirmée.

Un scan partiel ou une source inaccessible ne compte pas.

Un fichier `Missing` reste dans la base :

- Catalogue conservé.
- Métadonnées conservées.
- Historique conservé.
- Informations techniques conservées.
- Possibilité de retrouver et réassocier le fichier ultérieurement.

### **3.3 — Suppression volontaire par l'administrateur**

Validé

Trois opérations doivent être distinguées :

1. Retirer une référence de la bibliothèque sans toucher au fichier physique.
2. Supprimer réellement le fichier physique du disque, après confirmation explicite.
3. Retirer une œuvre du catalogue, selon des règles de protection de l'historique.

Après suppression physique :

- L'œuvre reste au catalogue.
- La référence au fichier reste conservée.
- Son statut devient `Missing`.
- Les métadonnées et historiques sont préservés.

Il faudra enregistrer le motif et la date de suppression, afin de distinguer une suppression volontaire d'une absence détectée par scan.

### **3.4 — Disponibilité des sources**

Validé

- Plusieurs tentatives espacées.
- Délai configurable avant de déclarer une source `Inaccessible`.
- Distinction entre disque débranché, partage réseau indisponible et erreur d'accès.
- Une source inaccessible ne doit pas provoquer de suppression du catalogue.

## **4. Visionnage et historique**

### **4.1 — Sessions interrompues**

Validé

- Une session `InProgress` sans mise à jour pendant cinq minutes devient `Interrupted`.
- Vérification au démarrage et périodiquement.
- Délai configurable.
- Dernière position de lecture conservée.
- Aucune validation automatique du statut `Watched` à la suite d'une interruption.

### **4.2 — Statut de session et fin de média**

Validé

Conservation des deux propriétés :

- `Status` : état de la session.
- `MediaEndReached` : indique si la lecture a atteint la fin du fichier.

Valeurs de `Status` :

- `InProgress`
- `EndedNormally`
- `Interrupted`
- `Error`

Ces informations ne sont pas interchangeables.

### **4.3 — Priorité de lecture**

Validé

- Priorité globale configurable des sources.
- Choix du fichier `Present` situé sur la source prioritaire.
- Repli sur une autre source disponible.
- Possibilité de choisir manuellement un autre emplacement.

### **4.4 — Passage automatique à Watched**

Validé

- Seuil de 90 % de la durée du fichier.
- Seuil configurable.
- Statut indépendant par utilisateur.
- Possibilité de modifier manuellement le statut.

### **4.5 — Conservation de l'historique**

Validé

- Conservation illimitée par défaut.
- Pas de purge automatique.
- Purge manuelle possible par l'administrateur.
- Suppression possible des données d'un utilisateur sans supprimer le catalogue commun ni les données des autres utilisateurs.

## **5. Sauvegardes et sécurité**

### **5.1 — Sauvegarde complète**

Validé

La sauvegarde doit comprendre :

- Base SQLite.
- Configuration.
- Utilisateurs.
- Progressions et historiques.
- Métadonnées locales.
- Associations et identifiants des fournisseurs.
- Images téléchargées et cache utile.

Les fichiers vidéo ne sont pas inclus par défaut.

### **5.2 — Protection des secrets**

Validé

- Protection des clés API par DPAPI Windows.
- Aucun secret en clair dans Git ou dans les fichiers de configuration.
- Prévoir une procédure de migration et de restauration des secrets.

Attention : DPAPI étant spécifique à Windows et au contexte de sécurité associé, le futur serveur Linux devra utiliser un mécanisme de protection des secrets adapté.

## **6. Architecture cible**

### **6.1 — Hébergement initial**

Validé

- Application Windows WPF.
- API ASP.NET Core intégrée au processus WPF.
- Architecture permettant de séparer ultérieurement le serveur et l'interface.

### **6.2 — Cible finale Raspberry Pi 4**

Validé

[Raspberry PI4 4GB Board](https://images.openai.com/static-rsc-4/BQPtsk39zytxTUveDxgoh_v6gi90UrujD-fvPcvC_FqE_UMg8r07gbjBBO_SvWvNQBXoCS7q-vZPqSKD8x9tNBOoQRRepbXrVzE-brposfdJVGn4Ptox8Aa4gxwcpG-nyBBHSl_69pizwF3e5WKRYwMNN3TF65veIv3w55mgiDI?purpose=inline)

Le Raspberry Pi 4 devient le serveur central :

- Linux ARM64.
- .NET et ASP.NET Core.
- SQLite.
- Catalogue et stockage des données centralisés.
- API accessible sur le réseau local.
- Interface web responsive pour PC, téléphones et tablettes.
- Accès aux fichiers vidéo présents sur les disques USB ou les stockages réseau.

WPF restera l'interface Windows de la première version. Le serveur Raspberry Pi ne dépendra pas de WPF.

## **7. Points techniques à réconcilier avant V2.3**

Les 18 arbitrages sont terminés, mais leur intégration dans le modèle nécessite encore quelques vérifications. Je préfère les signaler maintenant plutôt que de produire un modèle qui contiendrait des contradictions.

| Point | Sujet                 | Travail nécessaire                                                                                                                       |
| ----- | --------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| R1    | `MediaFileAssignment` | Déterminer s'il reste utile ou si `MediaWorkId` peut être directement porté par `MediaFile`.                                             |
| R2    | `MediaVersion`        | Réexaminer sa nécessité puisque les versions alternatives sont exclues.                                                                  |
| R3    | Doublons physiques    | Définir comment représenter plusieurs copies d'une même œuvre, notamment sur plusieurs disques.                                          |
| R4    | Suppression           | Distinguer suppression physique, retrait d'une référence et retrait d'une œuvre du catalogue.                                            |
| R5    | Statuts               | Clarifier les états d'un fichier, d'une référence et d'une source pour ne pas confondre `Missing`, `Inaccessible` et retrait volontaire. |
| R6    | Raspberry Pi          | Définir une stratégie de stockage des secrets compatible Windows et Linux, ainsi que les chemins de fichiers multiplateformes.           |
| R7    | Lecture               | Prévoir une lecture vidéo via navigateur sur le serveur Linux, distincte du lecteur LibVLCSharp WPF.                                     |


### **Décision complémentaire — Gestion des doublons physiques**

Validé

- Une œuvre possède un seul fichier principal actif.
- MyVidLib peut détecter plusieurs copies physiques correspondant à cette œuvre.
- Les copies supplémentaires sont identifiées comme doublons potentiels.
- L'administrateur peut choisir le fichier principal.
- Il peut retirer une copie de la bibliothèque ou la supprimer physiquement après confirmation.
- La priorité globale des sources s'applique au choix du fichier principal disponible, et non à plusieurs associations actives.

Cela permet de conserver un modèle simple sans perdre la capacité de gérer plusieurs disques.

