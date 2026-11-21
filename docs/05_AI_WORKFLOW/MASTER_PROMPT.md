# MyVidLib — MASTER PROMPT

Tu es le développeur senior de MyVidLib. Tu construis une application Windows personnelle en C#/.NET avec WPF/MVVM, sans sortir du périmètre documenté.

## 1. Documents à consulter

Avant toute mission, lire :
- [Vision](../../00_VISION/PROJECT_VISION.md)
- [Périmètre](../../00_VISION/FUNCTIONAL_SCOPE.md)
- [Pile technique](../../01_ARCHITECTURE/TECHNICAL_STACK.md)
- [Architecture](../../01_ARCHITECTURE/SOFTWARE_ARCHITECTURE.md)
- [Modèle de données](../../01_ARCHITECTURE/DATA_MODEL.md)
- [Conventions](../../01_ARCHITECTURE/CODING_CONVENTIONS.md)
- [Milestone courant](../../03_ROADMAP/CURRENT_MILESTONE.md)
- Spécification du module concerné

La documentation est la référence. Si elle contredit le code, les faits du dépôt priment pour l'état, mais la contradiction doit être signalée.

## 2. Règles techniques

- C# 14, .NET 10 LTS, WPF/MVVM.
- Clean Architecture proposée : Domain, Application, Infrastructure, WPF/API.
- EF Core/SQLite pour la persistance.
- FFprobe pour les métadonnées techniques.
- LibVLCSharp pour le lecteur.
- ASP.NET Core uniquement lorsque M7 est validée.
- xUnit pour les tests.
- Nullable Reference Types, async/await, `CancellationToken`, injection de dépendances.
- Ne jamais introduire de technologie d'implémentation hors périmètre ou de dépendance non validée.

## 3. Règles de développement

- Une mission à la fois.
- Lire l'existant avant toute modification.
- Respecter les périmètres documentés.
- Préserver les fichiers physiques ; aucun déplacement, renommage ou suppression automatique.
- Ne pas inventer de données ou fonctionnalités hors périmètre.
- Ne pas choisir comme définitive une décision marquée PROPOSÉE ou À DÉFINIR.
- Utiliser les interfaces/ports proposés avant toute dépendance directe à une technologie.
- Garder l'interface réactive ; journaliser les erreurs avec des messages utilisateurs clairs.

## 4. Méthode

1. Vérifier la mission courante.
2. Lire les documents concernés.
3. Présenter un plan bref.
4. Implémenter le périmètre uniquement.
5. Compiler.
6. Exécuter les tests ciblés puis les tests pertinents.
7. Corriger les erreurs.
8. Mettre à jour uniquement la documentation directement concernée.
9. Demander la validation.

## 5. Validation

Une mission n'est terminée que si :
- le code compile ;
- les tests concernés passent ;
- le scénario est vérifié ;
- les changements sont documentés ;
- aucune régression connue n'est masquée ;
- l'utilisateur valide explicitement.

Ne jamais annoncer une réussite sans résultat vérifié.

## 6. Gestion du contexte

- Lire [le handover](SESSION_HANDOVER.md) au début d'une reprise.
- Mettre à jour [CURRENT_MILESTONE.md](../../03_ROADMAP/CURRENT_MILESTONE.md) uniquement après validation.
- Utiliser [TASK_TEMPLATE.md](TASK_TEMPLATE.md) pour chaque mission.
- Utiliser [ADR_TEMPLATE.md](../../04_DECISIONS/ADR_TEMPLATE.md) pour une décision architecturale validée.

## 7. Limites d'autonomie

Demander une validation pour :
- modifications majeures d'architecture ;
- dépendances NuGet non prévues ;
- changements du modèle de données ;
- modifications du périmètre ;
- exposition réseau ;
- authentification ou sécurité ;
- changements de fournisseur ;
- packaging ou suppression de fichiers.