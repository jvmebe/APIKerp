namespace APIKerp.Models
{
    public class Cliente:Pai
    {
        public string RazaoSocial { get; set; }
        public string Endereco { get; set; }
        public int    Numero {  get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string FisicaJuridica { get; set; }
        public string Ativo { get; set; }
        public string ConsumidorRevenda { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscMunicipalSuframa { get; set; }
        public string CpfCnpj { get; set; }
        public string RegimeTributarioDaEmpresa { get; set; }

        public DateTime DataFundacao { get; set; }
        public string RegimeSemSt { get; set; }
        public string ProdutorRural { get; set; }
        public string VerEmFornecedores { get; set; }
        public string PessoasAutorizadas { get; set; }
        public string ObsDiv { get; set; }  
        public DateTime UltimaCompra { get; set; }

        //Placeholder
        public int codRamoAtividades { get; set; }
        public int codRegioes { get; set; }
        public int codVendedores { get; set; }
        public int codFuncionarios { get; set; }
        public int codCondicaoPagamentos { get; set; }
        public int codCidades { get; set; }
        public int codTransportadoras { get; set; }
        public int codContadores { get; set; }
        public int codFornecedores { get; set; }
        public int codListaPrecos { get; set; }
        public int codContatos { get; set; }

        //Agregação
        /*
        public RamoAtividades ramoAtividades;
        public Regioes regioes;
        public Vendedores vendedores;
        public Funcionarios funcionarios;
        public CondicaoPagamentos condicaoPagamentos;
        public Cidades cidades;
        public Transportadoras transportadoras;
        public Contadores contadores;
        public Fornecedores fornecedores;
        public ListaPrecos listaPrecos;
        public Contatos contatos;
        */
    }
}
