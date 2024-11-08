namespace APIKerp.Models
{
    public class Fornecedor:Pai
    {
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CpfCnpj {  get; set; }
        public string FisicaJuridica { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Tipo {  get; set; }
        
        //Endereco
        public string Logradouro { get; set; }
        public string Numero {  get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep {  get; set; }
        public int IdCidade { get; set; }
        public string? Cidade { get; set; }
        public int IdRegiao { get; set; }
        public string? Regiao { get; set; }

        public string ConsumidorRevenda { get; set; }
        public string? Observacao { get; set; }
        public string Ativo { get; set; }
        public decimal LimiteCredito { get; set; }        
        public string Trade {  get; set; }
        
    }
}
