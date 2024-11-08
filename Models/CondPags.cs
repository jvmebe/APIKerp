namespace APIKerp.Models
{
    public class CondPags
    {
        public string CondicaoPagamento { get; set; }
        public decimal TaxaJuros { get; set; }
        public int NumeroParcelas { get; set; }
        public string Tipo {  get; set; }
        public string Dia {  get; set; }
        public string Ativo { get; set; }
        public string PorParcela { get; set; }
    }
}
