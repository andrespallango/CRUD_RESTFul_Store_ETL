using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDD
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VentaId { get; set; }
        [Required]
        public DateTime FechaVenta { get; set; } = DateTime.Now;
        public double MontoTotal { get; set; } 
        public int CantidadProducto {  get; set; }
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }


    }
}

