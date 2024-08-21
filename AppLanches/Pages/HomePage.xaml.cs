using AppLanches.Models;
using AppLanches.Services;
using AppLanches.Validations;

namespace AppLanches.Pages;

public partial class HomePage : ContentPage
{
	private readonly ApiService _apiService;
	private readonly IValidator _validator;
	private bool _loginPageDisplayed = false;

	//public HomePage(ApiService apiService, IValidator validator)
	//{
	//	InitializeComponent();
	//	_apiService = apiService;
	//	_validator = validator;
	//}

	//protected override async void OnAppearing()
	//{
	//	base.OnAppearing();
	//	await GetListaCategorias();
	//	await GetMaisVendidos();
	//	await GetPopulares();
	//}

	//private async Task<IEnumerable<Categoria>> GetListaCategorias()
	//{
	//	try
	//	{
	//		var (categorias, errorMessage) = await _apiService.GetCategorias();

	//		if (errorMessage == "Unauthorized" && !_loginPageDisplayed)
	//		{
	//			await DisplayLoginPage();
	//			return Enumerable.Empty<Categoria>();
	//		}

	//		if(categorias == null)
	//		{
	//			await DisplayAlert("Erro", errorMessage ?? "Não foi possível obter as categorias.", "OK");
			
	//			return Enumerable.Empty<Categoria>();
	//		}

            // CvCategorias.ItemsSource = categorias;
	//		return categorias;
	//	}
	//	catch (Exception ex)
	//	{
	//		await DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
	//		return Enumerable.Empty<Categoria>();
	//	}
	//}

	private async Task DisplayLoginPage()
	{
		_loginPageDisplayed = true;
		await Navigation.PushAsync(new LoginPage(_apiService, _validator));
	}
}