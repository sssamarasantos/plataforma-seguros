namespace SeguroContratacao.Domain.Interfaces
{
    public interface INotificacaoService
    {
        Task NotificarContratacaoEfetivadaAsync(int idContratacao, string emailContratante, string numeroApolice);
    }
}