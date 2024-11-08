namespace APIKerp.Models
{
    public class Estados:Pai
    {
        public string   Estado {  get; set; }
        public string   Sigla { get; set; }
        public decimal  PercIcms { get; set; }
        public decimal  IcmsInt { get; set; }
        public decimal  PerRedSt {  get; set; }
        public int      CodigoWeb { get; set; }


        // Info para GET
        public int      IdPais { get; set; }
        public string?   NomePais {  get; set; }

        //public Paises?   Pais { get; set; }

    }
}
