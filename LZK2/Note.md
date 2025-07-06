# LZK2 Note

|                  Name | Points  | Notes                                                                             |
| --------------------: | :-----: | --------------------------------------------------------------------------------- |
|          PLZ einbauen |   5/5   |                                                                                   |
| Neuer Dienst einbauen |   4/5   | Warum hast du `ILocalStorage` umgebaut? Schadet nicht wurde aber nicht gefordert. |
|       Dienst ausbauen |   3/3   | Beim `SQLiteStorage` werden nun beim Programmstart alle Daten gelöscht)           |
|      Ansicht anpassen |   2/2   |                                                                                   |
| Grid-ansicht anpassen |   2/2   |                                                                                   |
|                 Total | 16 / 17 |                                                                                   |
|                Abzüge |    1    | Daten werden gelöscht beim Start                                                  |
|                 Total | 15 / 17 |                                                                                   |
|                 Total |   5.3   | ✊                                                                                 |

### Neuer Eigenschaft einbauen

- `PLZ` Eigenschaft einbauen.
  - Siehe `Age`.
  - ✅ Modell anpassen.
  - ✅ ViewModell anpassen.
  - ✅ Im View anzeigen.
    - ✅ Die Liste braucht eine neue Spalte.
    - ✅ Man sollte die PLZ eingeben können.
  - ✅ Beim Speichern berücksichtigen.

### Neuer Dienst einbauen

- ✅ `IPersonService` erstellen und einbauen
  - Grundsätzlich soll die Logik von `MainPageViewModel.Save()` ausgelagert werden.
  - ✅ `IPersonService.Save(Person person)` Methode definieren.
  - ✅ Logik von `MainPageViewModel.Save()` nach `PersonService.Save()` verschieben.
  - ✅ `IPersonService` soll `ILocalService` einsetzen, um zu speichern.
  - ⛔️ `IPersonService` muss im IOC registriert sein.

### Dienst ausbauen

- ✅ `IPersonService` um eine `Load()` Methode ergänzen.
  - ✅ Ein Teil der Logik von `MainPageViewModel.EnsureModelLoaded()` soll ausgelagert werden.
  - ✅ Ein Teil ist vom `MainPageViewModel` abhängig und soll *nicht* ausgelagert werden.

### Ansicht anpassen

- Ansicht mit Styles anpassen
  - ✅ Buttons sollen mit orangem (`#FF8000`) Hintergrund dargestellt werden.
  - ✅ Buttons sollen mit der gleichen Breite dargestellt werden.
  - Properties `BackgroundColor` und `WidthRequest` sind evtl. interessant.
  - `Styles.xaml` evtl. auch.

### Grid-ansicht anpassen

- ✅ Umrandung vom Grid besser sichtbar zu machen
  - Evtl. mit einem `Frame` mit einem `BorderColor`?
  - Oder vllt. mit einer `BackgroundColor`?
  - Oder beides?
- ✅ Spalte `Age` soll im Grid mit einer anderen Farbe (`TextColor`) dargestellt werden.
