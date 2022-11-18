namespace Financeiro.Aula.Domain.ValueObjects
{
    public record TermoImpressao
    {
        public string Clausula { get; set; }

        public string Texto { get; set; }

        public bool Negrito { get; set; }

        public int Ordem { get; set; }

        public TermoImpressao(string clausula, string texto, bool negrito, int ordem)
        {
            Clausula = clausula;
            Texto = texto;
            Negrito = negrito;
            Ordem = ordem;
        }
    }
}