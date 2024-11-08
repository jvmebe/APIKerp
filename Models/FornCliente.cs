namespace APIKerp.Models
{
    public class FornCliente: Pai
    {
        
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoEstadual { get; set; }
        public string CpfCnpj { get; set; }
        public string Tipo { get; set; }
        public string CidadeNome { get; set; }
        public string EstadoNome { get; set; }
        public string EstadoSigla { get; set; }
        public string PaisNome { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string ConsumidorRevenda { get; set; } 
        public string Observacao { get; set; }
        public string RegimeSemSt { get; set; }        
        public string ProdutorRural { get; set; }      
        public DateTime DataUltimaCompra { get; set; }
        public string Ativo { get; set; }              
        public string FisicaJuridica { get; set; }     
        public string ListaNome { get; set; }
        public DateTime ListaDataModificacao { get; set; }
        public decimal MargemLucro { get; set; }
        public decimal DescMax { get; set; }
        public decimal PerComissao { get; set; }
        public string CondicaoPagamentoNome { get; set; }
        public decimal TaxaJuros { get; set; }
        public int NumeroParcelas { get; set; }
        public DateTime CondicaoPagamentoDataCadastro { get; set; }
        public DateTime CondicaoPagamentoDataAlteracao { get; set; }
    }
}
