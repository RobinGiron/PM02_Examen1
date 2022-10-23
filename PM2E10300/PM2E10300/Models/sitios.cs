using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E10300.Models
{
    public class sitios
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public byte[] img { get; set; }
        public float Longitud { get; set; }
        public float Latitud { get; set; }
        [MaxLength(100)]
        public String desc { get; set; }
    }
}
