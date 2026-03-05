using Anodica.AccesoDatos.Repositorio.IRepositorio;
using Anodica.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anodica.AccesoDatos.Repositorio
{
    public class PerfilRepositorio : Repositorio<Perfil, int>, IPerfilRepositorio
    {
        private readonly ApplicationDbContext _db;
        
        public PerfilRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Perfil perfil)
        {
            var objDesdeDb = _db.Perfiles.FirstOrDefault(p => p.PerfilID == perfil.PerfilID);
            if (objDesdeDb != null)
            {
                objDesdeDb.PerfilCodigoAlcemar = perfil.PerfilCodigoAlcemar;
                objDesdeDb.LineaRef = perfil.LineaRef;
                objDesdeDb.UbicacionRef = perfil.UbicacionRef;
                objDesdeDb.Descripcion = perfil.Descripcion;
                objDesdeDb.PesoXmetro = perfil.PesoXmetro;
                objDesdeDb.LongTiraMts = perfil.LongTiraMts;
                objDesdeDb.PesoXtira = perfil.PesoXtira;
                objDesdeDb.CantTirasPaquete = perfil.CantTirasPaquete;
                objDesdeDb.ManejaStockPropio = perfil.ManejaStockPropio;

                if (perfil.ImagenPerfil != null)
                {
                    objDesdeDb.ImagenPerfil = perfil.ImagenPerfil;
                }
            }
        }
    }
}
