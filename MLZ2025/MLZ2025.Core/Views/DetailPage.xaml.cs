using MLZ2025.Core.ViewModel;

namespace MLZ2025;

public partial class DetailPage : ContentPage
{
  public DetailPage(DetailViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }
}

