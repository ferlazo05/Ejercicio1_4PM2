using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Ejercicio1_4.Models
{
    public class Photos
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string nombre { get; set; }
        [MaxLength(50)]
        public string descripcion { get; set; }
        [MaxLength(50)]
        public Byte[] foto { get; set; }
    }
}
