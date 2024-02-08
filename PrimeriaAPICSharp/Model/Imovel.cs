namespace PrimeriaAPICSharp.Model
{
    public class Imovel
    {
       

        public Guid Id { get; set; }
        public Endereco Endereco { get; set; }
        public string Proprietario { get; set; }

        public Imovel(Endereco endereco, string proprietario)
        {
            Id = Guid.NewGuid();
            Endereco = endereco;
            Proprietario = proprietario;
        }
    }
}
