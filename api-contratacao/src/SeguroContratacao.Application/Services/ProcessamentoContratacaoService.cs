using SeguroContratacao.Application.Common;
using SeguroContratacao.Application.Interfaces;
using SeguroContratacao.Domain.Common;
using SeguroContratacao.Domain.Interfaces;
using SeguroContratacao.Domain.Models;

namespace SeguroContratacao.Application.Services
{
    public class ProcessamentoContratacaoService : IProcessamentoContratacaoService
    {
        private readonly IContratacaoRepository _contratacaoRepository;
        private readonly INotificacaoService _notificacaoService;

        public ProcessamentoContratacaoService(
            IContratacaoRepository contratacaoRepository,
            INotificacaoService notificacaoService)
        {
            _contratacaoRepository = contratacaoRepository;
            _notificacaoService = notificacaoService;
        }

        public async Task<ResultadoOperacao> ProcessarContratacaoAsync(Contratacao contratacao)
        {
            var sucesso = await _contratacaoRepository.InserirAsync(contratacao);
            if (!sucesso)
            {
                return ErrosApplication.ResultadoContratacaoFalhou;
            }

            await _notificacaoService.NotificarContratacaoEfetivadaAsync(
                contratacao.Id,
                contratacao.EmailContratante,
                contratacao.NumeroApolice);

            return ResultadoOperacao.Sucesso();
        }
    }
}