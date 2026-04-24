namespace GoodHamburger.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Registro não encontrado.") : base(message) { }
    }
}
