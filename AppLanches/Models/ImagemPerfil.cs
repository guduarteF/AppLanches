using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLanches.Models
{
    public class ImagemPerfil
    {
        public string? UrlImagem { get; set; }
        public string? CaminhoImagem => AppConfig.BaseUrl + UrlImagem;
    }
}
