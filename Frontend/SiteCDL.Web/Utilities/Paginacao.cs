using Microsoft.EntityFrameworkCore;
using SiteCDL.Models;
using SiteCDL.Repository;
using System.Linq.Expressions;

namespace SiteCDL.Utilities
{
    public class Paginacao
    {

        private int _quantidadePaginas;
        private int _pageNumber;
        private int _quantidadeDeLinhas;
        private readonly int _pageSize;        
        private PaginacaoRepository _paginacaoRepository;
        private Expression<Func<Post, bool>> _expression;

        public Paginacao(PaginacaoRepository paginacaoRepository, int pageNumber, int pageSize, Expression<Func<Post, bool>> expression)
        {
            _pageNumber = pageNumber;
            _paginacaoRepository = paginacaoRepository;
            _pageSize = pageSize;
            _expression = expression;
        }

        public async Task<RetornaDadosPaginacao> ValidaQuantidadePagina()
        {
            await QuantidadeDePaginas();

            if (!int.TryParse(_pageNumber.ToString(), out _) || _pageNumber > _quantidadePaginas || _pageNumber < 1)
            {
                _pageNumber = _quantidadePaginas;
            }
            return new RetornaDadosPaginacao()
            {
                pageNumber = _pageNumber,
                pageSize = _pageSize
            };
        }

        private async Task QuantidadeDePaginas()
        {
            _quantidadeDeLinhas = await _paginacaoRepository.QuantidadeDeLinhas(_expression);
            _quantidadePaginas = (int)Math.Round(Math.Ceiling((double)_quantidadeDeLinhas / _pageSize));
        }

        public List<Post> SetValuesPaginacao(List<Post> model)
        {

            model[0].TotalPages = _quantidadePaginas;
            model[0].PageNumber = _pageNumber;
            model[0].QuantidadeLinhas = _quantidadeDeLinhas;
            model[0].PageSize = _pageSize;

            if (_pageNumber == 1)
            {
                model[0].Previous = 0;
            }
            else if (_pageNumber > 1)
            {
                model[0].Previous = _pageNumber - 1;
            }


            if (_pageNumber < model[0].TotalPages)
            {
                model[0].Next = _pageNumber + 1;
            }

            return model;
        }        

        public class RetornaDadosPaginacao
        {
            public int pageNumber { get; set; }
            public int pageSize { get; set; } 
        }

    }
}
