using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLanches.Models
{
    public class PedidoDetalhe
    {
        public int Id { get; set; }
        public int Quantidade { get;set; }
        public decimal SubTotal { get; set; }
        public string? ProdutoNome { get; set; }
        public string? ProdutoImagem { get; set; }
        public string CaminhoImagem => AppConfig.BaseUrl + ProdutoImagem;
        public decimal ProdutoPreco { get; set; }
    }
}
