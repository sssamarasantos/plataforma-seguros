namespace SeguroProposta.Domain.Common;

public enum TipoErro
{
    Validacao,
    RegraDeNegocio,
    NaoEncontrado,
    ErroOperacional
}

public record Erro(TipoErro TipoErro, string Mensagem);

public class ResultadoOperacao
{
    public bool EhSucesso { get; }
    public bool EhFalha => !EhSucesso;
    public Erro? Erro { get; }

    protected ResultadoOperacao(bool sucesso, Erro? erro)
    {
        if (sucesso && erro is not null)
            throw new InvalidOperationException("Um resultado de sucesso não pode conter um erro.");

        if (!sucesso && erro is null)
            throw new InvalidOperationException("Resultado de falha deve conter erro.");

        EhSucesso = sucesso;
        Erro = erro;
    }

    public static ResultadoOperacao Sucesso() => new(true, null);
    public static ResultadoOperacao Falha(Erro erro) => new(false, erro ?? throw new ArgumentNullException(nameof(erro)));

    public static implicit operator ResultadoOperacao(Erro erro) => Falha(erro);

    public static ResultadoOperacao<T> Sucesso<T>(T valor) => new(valor, true, null);
    public static ResultadoOperacao<T> Falha<T>(Erro erro) => new(default!, false, erro);
}

public class ResultadoOperacao<T> : ResultadoOperacao
{
    public T Valor
    {
        get
        {
            if (EhFalha)
                throw new InvalidOperationException($"Não é possível acessar o valor de um resultado de falha para o tipo {typeof(T).Name}. Erro: {Erro?.Mensagem}");

            return _valor;
        }
    }

    private readonly T _valor;

    protected internal ResultadoOperacao(T valor, bool sucesso, Erro? erro)
        : base(sucesso, erro)
    {
        _valor = valor;
    }

    public static implicit operator ResultadoOperacao<T>(T valor) => Sucesso(valor);

    public static implicit operator ResultadoOperacao<T>(Erro erro) => Falha<T>(erro);
}


