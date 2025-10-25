using System;

namespace GrupoCeleste.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string IdTransaccion { get; set; }
        public string EmailUsuario { get; set; }
    }

    public class Subscripcion
    {
        public int Id { get; set; }
        public string EmailUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; }
        public string IdSubscripcion { get; set; }
    }
}
