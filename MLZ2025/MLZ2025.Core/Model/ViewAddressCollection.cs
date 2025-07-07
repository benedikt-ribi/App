using MLZ2025.Shared.Model;
using System.Collections.ObjectModel;

public class ViewAddressCollection : ObservableCollection<ViewAddress>
{
    // Hier kannst du eigene Methoden, Events oder Hilfsfunktionen ergänzen.
    // Beispiel: Ein Event, wenn ein Eintrag hinzugefügt wird.
    public event Action<ViewAddress>? AddressAdded;

    protected override void InsertItem(int index, ViewAddress item)
    {
        base.InsertItem(index, item);
        AddressAdded?.Invoke(item);
    }

    // Beispiel: Einfache Suche nach Name
    public ViewAddress? FindByName(string firstName, string lastName) =>
        this.FirstOrDefault(a => a.FirstName == firstName && a.LastName == lastName);
}
