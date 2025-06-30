using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Core.Services;
using Core;

namespace Core;

public partial class MainPageViewModel : ViewModelBase
{
    private readonly ILocalStorage<Person> _localStorage;
    private readonly IPersonService _personService;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private int _age;
    private string _plz = string.Empty;
    private Person? _selectedItem;

    public MainPageViewModel()
    {
        //throw new InvalidOperationException("This constructor is for detecting binding in XAML and should never be called.");
    }

    public MainPageViewModel(ILocalStorage<Person> localStorage, IPersonService personService)
    {
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        Items = new ObservableCollection<Person>();
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (SetField(ref _firstName, value))
            {
                OnPropertyChanged(nameof(FullName));
            }
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (SetField(ref _lastName, value))
            {
                OnPropertyChanged(nameof(FullName));
            }
        }
    }

    public object FullName => $"{LastName} {FirstName}";

    public int Age
    {
        get => _age;
        set => SetField(ref _age, value);
    }

    public string PLZ
    {
        get => _plz;
        set
        {
            if (SetField(ref _plz, value))
            {
                OnPropertyChanged(nameof(PLZ));
            }
        }
    }

    public bool IsReady => SelectedItem != null;

    public ObservableCollection<Person> Items { get; private set; } = new();

    public Person? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetField(ref _selectedItem, value))
            {
                if (value != null)
                {
                    // Nur setzen, wenn sich der Wert unterscheidet, um unnötige PropertyChanged-Events zu vermeiden
                    if (FirstName != value.FirstName)
                        FirstName = value.FirstName;
                    if (LastName != value.LastName)
                        LastName = value.LastName;
                    if (Age != value.Age)
                        Age = value.Age;
                    if (PLZ != value.PLZ)
                        PLZ = value.PLZ;
                }
                else
                {
                    if (FirstName != string.Empty)
                        FirstName = string.Empty;
                    if (LastName != string.Empty)
                        LastName = string.Empty;
                    if (Age != 0)
                        Age = 0;
                    if (PLZ != string.Empty)
                        PLZ = string.Empty;
                }
            }
        }
    }

    public void Increment()
    {
        Age += 1;
    }

    [RelayCommand]
    public async Task EnsureModelLoaded()
    {
        if (Items.Count == 0)
        {
            try
            {
                var people = await _personService.Load();

                if (people.Count == 0)
                {
                    people.Add(new Person());
                }

                foreach (var person in people)
                {
                    Items.Add(person);
                }

                SelectedItem = Items.First();

                // PLZ-PropertyChanged nach SelectedItem setzen, damit Reihenfolge stimmt
                OnPropertyChanged(nameof(IsReady));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public async Task Save()
    {
        var model = SelectedItem;

        if (model == null)
        {
            throw new InvalidOperationException("Cannot save a non-existent model");
        }

        model.FirstName = FirstName;
        model.LastName = LastName;
        model.Age = Age;
        model.PLZ = PLZ;

        await _localStorage.Save(model);
        await _personService.Save(model);
    }

    public void Add()
    {
        var settingsModel = new Person
        {
            FirstName = FirstName,
            LastName = LastName,
            Age = Age,
            PLZ = PLZ
        };

        Items.Add(settingsModel);

        SelectedItem = settingsModel;
    }
}