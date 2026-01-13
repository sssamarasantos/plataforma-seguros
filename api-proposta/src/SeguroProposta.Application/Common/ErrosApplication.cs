using SeguroProposta.Domain.Common;

namespace SeguroProposta.Application.Common;

public static class ErrosApplication
{
    public static Erro InserirPropostaFalhou { get; } = new(TipoErro.ErroOperacional, "Erro ao inserir a proposta.");
    public static Erro AtualizarPropostaFalhou(int id) => new(TipoErro.ErroOperacional, $"Erro ao atualizar o status da proposta {id}");
    public static Erro PropostaNaoEncontrada(int id) => new(TipoErro.NaoEncontrado, $"Proposta com ID {id} não encontrada.");
}