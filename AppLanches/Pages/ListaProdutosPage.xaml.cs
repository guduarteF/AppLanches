using AppLanches.Models;
using AppLanches.Services;
using AppLanches.Validations;

namespace AppLanches.Pages;

public partial class ListaProdutosPage : ContentPage
{
	private readonly ApiService _apiService;
	private readonly IValidator _validator;
	private int _categoriaId;
	private bool _loginPageDisplayed = false;

	public ListaProdutosPage(ApiService apiService,string categoriaNome,
								IValidator validator, int categoriaId)
	{
		InitializeComponent();
		_apiService = apiService;
		_validator = validator;
		_categoriaId = categoriaId;
		Title = categoriaNome ?? "Produtos";
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await GetListaProdutos(_categoriaId);
	}

	private async Task<IEnumerable<Produto>> GetListaProdutos(int categoriaId)
	{
		try
		{
			var (produtos, errorMessage) = await _apiService.GetProdutos("categoria", categoriaId.ToString());

			if(errorMessage == "Unauthorized" && !_loginPageDisplayed)
			{
				await DisplayLoginPage();
				return Enumerable.Empty<Produto>();
			}

			
			if (produtos is null)
			{
				await DisplayAlert("Erro", errorMessage ?? "N�o foi poss�vel obter as categorias.", "OK");
				return Enumerable.Empty<Produto>();
			}

			CvProdutos.ItemsSource = produtos;
			return produtos;
		}
		catch ( Exception ex)
		{
			await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
			return Enumerable.Empty<Produto>();

		}
	}

	private async Task DisplayLoginPage()
	{
		_loginPageDisplayed = true;
		await Navigation.PushAsync(new LoginPage(_apiService, _validator));
	}
}