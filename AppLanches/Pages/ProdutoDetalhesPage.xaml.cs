using AppLanches.Models;
using AppLanches.Services;
using AppLanches.Validations;

namespace AppLanches.Pages;

public partial class ProdutoDetalhesPage : ContentPage
{
	private readonly ApiService _apiService;
	private readonly IValidator _validator;
	private int _produtoId;
	private bool _loginPareDisplayed = false;

	public ProdutoDetalhesPage(int produtoId, string produtoNome,
								ApiService apiService, IValidator validator)
	{
		InitializeComponent();
		_apiService = apiService;
		_validator = validator;
		_produtoId = produtoId;
        Title = produtoNome ?? "Detalhe do Produto";
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetProdutoDetalhes(_produtoId);
    }

    private async Task<Produto?>GetProdutoDetalhes(int produtoId)
    {
        var (produtoDetalhe, errorMessage) = await _apiService.GetProdutoDetalhe(produtoId);

        if(errorMessage == "Unauthorized" && !_loginPareDisplayed)
        {
            await DisplayLoginPage();
            return null;
        }

        if(produtoDetalhe is null)
        {
            // Lidar com o erro, exibir mensagem ou logar 
            await DisplayAlert("Erro", errorMessage ?? "Não foi possível obter o produto.", "OK");
            return null;
        }
        

        if(produtoDetalhe != null)
        {
            //Atualizar as propriedades dos controles com os dados do produto
            ImagemProduto.Source = produtoDetalhe.CaminhoImagem;
            LblProdutoNome.Text = produtoDetalhe.Nome;
            LblProdutoPreco.Text = produtoDetalhe.Preco.ToString();
            LblProdutoDescricao.Text = produtoDetalhe.Detalhe;
            LblPrecoTotal.Text = produtoDetalhe.Preco.ToString();
        }
        else
        {
            await DisplayAlert("Erro", errorMessage ?? "Não foi possível obter detalhes do produto", "OK");
            return null;
        }
        return produtoDetalhe;
    }

    private void ImagemBtnFavorito_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnRemove_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(LblQuantidade.Text, out int quantidade) && 
            decimal.TryParse(LblProdutoPreco.Text, out decimal precoUnitario))
        {
            // Decrementa a quantidade, e não permite que seja menor que 1
            quantidade = Math.Max(1, quantidade - 1);
            LblQuantidade.Text = quantidade.ToString();

            // Calcula o preço total
            var precoTotal = quantidade * precoUnitario;
            LblPrecoTotal.Text = precoTotal.ToString();
        }
        else
        {
            // Tratar caso as conversões falhem
            DisplayAlert("Erro", "Valores inválidos", "OK");
        }
    }

    private void BtnAdiciona_Clicked(object sender, EventArgs e)
    {
        if(int.TryParse(LblQuantidade.Text, out int quantidade) &&
                decimal.TryParse(LblProdutoPreco.Text, out decimal precoUnitario))
        {
            //Incrementa a quantidade
            quantidade++;
            LblQuantidade.Text = quantidade.ToString();

            //Calcula o preço total
            var precoTotal = quantidade * precoUnitario;
            LblPrecoTotal.Text = precoTotal.ToString(); // formata como moeda
        }
        else
        {
            // tratar caso as conversões falhem
            DisplayAlert("Erro", "Valores inválidos", "OK");
        }
    }

    private async void BtnIncluirNoCarrinho_Clicked(object sender, EventArgs e)
    {
        try
        {
            var carrinhoCompra = new CarrinhoCompra()
            {
                Quantidade = Convert.ToInt32(LblQuantidade.Text),
                Preco = Convert.ToDecimal(LblProdutoPreco.Text),
                ValorTotal = Convert.ToDecimal(LblPrecoTotal.Text),
                ProdutoId = _produtoId,
                ClienteId = Preferences.Get("usuarioid", 0)
            };
            var response = await _apiService.AdicionaItemNoCarrinho(carrinhoCompra);
            if(response.Data)
            {
                await DisplayAlert("Sucesso", "Item adicionado ao carrinho", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Erro", $"Falha ao adicionar item: {response.ErrorMessage}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPareDisplayed = true;

        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }
}