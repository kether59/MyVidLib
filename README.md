# MyVidLib

**My Video Library** — Gestionnaire personnel de bibliothèque multimédia pour Windows.

MyVidLib centralise et organise une bibliothèque personnelle de films, séries et animés stockés sur différents supports locaux ou réseau, **sans jamais déplacer, renommer ou supprimer automatiquement** les fichiers originaux.

## État du projet

| Jalon | Description | Statut |
|-------|-------------|--------|
| **M0** | Initialisation technique | En cours de validation |
| M1–M8 | Fonctionnalités métier | Non commencés |

**M0** fournit uniquement le socle technique : solution multi-projets, architecture en couches, DI, Serilog, MVVM minimal, tests xUnit et compilation.

## Prérequis

- Windows 10 ou Windows 11
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (LTS)
- Visual Studio Code (recommandé) ou Visual Studio 2022/2026
- Git

> L’interface WPF nécessite Windows. La compilation des projets non-WPF est possible sous Linux, mais l’application et les tests WPF ne s’exécutent que sous Windows.

## Architecture

```text
MyVidLib/
├── MyVidLib.sln
├── Directory.Build.props          # Propriétés communes (NRT, ImplicitUsings, C# 14)
├── Directory.Packages.props       # Versions NuGet centralisées
├── src/
│   ├── MyVidLib.Domain/           # Entités, objets valeur, règles métier (aucune dépendance technique)
│   ├── MyVidLib.Application/      # Cas d’utilisation, DTO, ports
│   ├── MyVidLib.Infrastructure/   # Persistance, fichiers, services externes, logging
│   └── MyVidLib.Wpf/              # Interface WPF + composition root (DI, Serilog)
└── tests/
    ├── MyVidLib.Domain.Tests/
    ├── MyVidLib.Application.Tests/
    └── MyVidLib.Infrastructure.Tests/
```

### Règles de dépendance

- **Domain** → aucune dépendance projet
- **Application** → Domain
- **Infrastructure** → Application, Domain
- **Wpf** → Application, Infrastructure (composition root uniquement)
- Les ViewModels n’accèdent jamais directement à EF Core, SQLite ou au système de fichiers.

## Installation de l’environnement

```bash
# Cloner le dépôt
git clone <url-du-dépôt> MyVidLib
cd MyVidLib

# Restaurer les packages
dotnet restore
```

## Compilation

```bash
dotnet build
```

Configuration Debug par défaut. Pour Release :

```bash
dotnet build -c Release
```

## Exécution des tests

```bash
dotnet test
```

Ou par projet :

```bash
dotnet test tests/MyVidLib.Domain.Tests
dotnet test tests/MyVidLib.Application.Tests
dotnet test tests/MyVidLib.Infrastructure.Tests
```

## Lancement de l’application

Sous **Windows** uniquement :

```bash
dotnet run --project src/MyVidLib.Wpf
```

Ou ouvrir `MyVidLib.sln` dans Visual Studio / VS Code et lancer le projet `MyVidLib.Wpf`.

Au démarrage, l’application :

1. Configure Serilog (console + fichiers journaliers dans `%LOCALAPPDATA%\MyVidLib\Logs`)
2. Démarre le Host .NET avec injection de dépendances
3. Affiche une fenêtre minimale prouvant le fonctionnement du MVVM

## Packages NuGet (M0)

| Package | Usage |
|---------|--------|
| CommunityToolkit.Mvvm | MVVM (ObservableObject, RelayCommand, source generators) |
| Microsoft.Extensions.DependencyInjection | Conteneur DI |
| Microsoft.Extensions.Hosting | Host générique |
| Microsoft.Extensions.Logging | Abstractions de journalisation |
| Serilog + Sinks (Console, File) | Journalisation structurée |
| xUnit + Microsoft.NET.Test.Sdk | Tests unitaires |

Aucune dépendance EF Core, FFprobe, LibVLC ou fournisseur externe n’est introduite en M0.

## Documents de référence

Les documents fondateurs se trouvent dans le dossier parent / documentation du projet :

- `DATA_MODEL.md` (V2.3.1) — modèle de données conceptuel validé
- `CODING_CONVENTIONS.md` — conventions de code
- `SOFTWARE_ARCHITECTURE.md` — architecture logicielle
- `TECHNICAL_STACK.md` — pile technique
- `PROJECT_VISION.md` — vision produit

Le modèle de données est **validé** : il ne doit pas être modifié ni traduit en code EF Core sans autorisation explicite.

## Prochaines étapes

Après validation de M0, le jalon suivant (M1) introduira le modèle de persistance et le scanner de fichiers, conformément à la feuille de route.

---

*MyVidLib — M0 Technical Initialization*
