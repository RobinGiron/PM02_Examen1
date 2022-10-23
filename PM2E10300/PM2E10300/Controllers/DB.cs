using System;
using System.Collections.Generic;
using System.Text;
using PM2E10300.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PM2E10300.Controllers
{
    public class DB
    {
        readonly SQLiteAsyncConnection db;
        public DB() { }
        public DB(String path)
        {
            db = new SQLiteAsyncConnection(path);
            db.CreateTableAsync<sitios>();
        }
        public Task<List<sitios>> listadire()
        {
            return db.Table<sitios>().ToListAsync();
        }
        public Task<sitios> getmylocation(int uId)
        {
            return db.Table<sitios>()
                .Where(i => i.id == uId)
                .FirstOrDefaultAsync();
        }
        public Task<sitios> getlong(float Longitud)
        {
            return db.Table<sitios>().Where(i => i.Longitud == Longitud).FirstOrDefaultAsync();
        }
        public Task<sitios> getlatt(float Latitud)
        {
            return db.Table<sitios>().Where(i => i.Latitud == Latitud).FirstOrDefaultAsync();
        }
        public Task<sitios> getdesc(String Descripcion)
        {
            return db.Table<sitios>().Where(i => i.desc == Descripcion).FirstOrDefaultAsync();
        }
        public Task<Int32> savelocation(sitios ubicacion)
        {
            if (ubicacion.id != 0)
            {
                return db.UpdateAsync(ubicacion);
            }
            else
            {
                return db.InsertAsync(ubicacion);
            }
        }
        public Task<Int32> EliminarUbicacion(sitios ubicacion)
        {
            return db.DeleteAsync(ubicacion);
        }
        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
