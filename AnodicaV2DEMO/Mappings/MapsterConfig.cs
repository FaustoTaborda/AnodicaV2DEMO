using Mapster;
using Anodica.Modelos;
using AnodicaV2DEMO.ViewModels; 

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Insumo, InsumoVM>.NewConfig();

        TypeAdapterConfig<InsumoVM, Insumo>.NewConfig()
    .Ignore(dest => dest.CantidadStock);

        TypeAdapterConfig<PerfilTratamientoFilaVM, PerfilTratamiento>.NewConfig()
            .Map(dest => dest.TratamientoRef, src => src.TratamientoRef)
            .Map(dest => dest.UbicacionRef, src => src.UbicacionRef)
            .Map(dest => dest.CantMinimaTirasStock, src => src.CantMinimaTirasStock)
            .Ignore(dest => dest.CantidadStock);

        TypeAdapterConfig<PerfilVM, Perfil>.NewConfig()
    .Ignore(dest => dest.ImagenPerfil)
    .Map(dest => dest.PesoXtira, src => src.PesoXmetro * src.LongTiraMts)
    .Ignore(dest => dest.PerfilTratamientos)

    .AfterMapping((src, dest) =>
     {
         var tratamientosEnDb = dest.PerfilTratamientos.ToList();
         var tratamientosTildados = src.Tratamientos != null ? src.Tratamientos.Where(t => t.EstaSeleccionado).ToList() : new List<PerfilTratamientoFilaVM>();
         var idsTratamientosTildados = tratamientosTildados.Select(t => t.TratamientoRef).ToList();
         var tratamientosDesmarcado = tratamientosEnDb.Where(pt => !idsTratamientosTildados.Contains(pt.TratamientoRef)).ToList();

         foreach (var itemAEliminar in tratamientosDesmarcado)
         {
             dest.PerfilTratamientos.Remove(itemAEliminar);
         }

         foreach (var itemPantalla in tratamientosTildados)
         {
             var relacionExistente = tratamientosEnDb.FirstOrDefault(pt => pt.TratamientoRef == itemPantalla.TratamientoRef);

             if (relacionExistente != null)
             {
                 relacionExistente.UbicacionRef = itemPantalla.UbicacionRef;
                 relacionExistente.CantMinimaTirasStock = itemPantalla.CantMinimaTirasStock;
             }
             else
             {
                 var nuevoTratamiento = itemPantalla.Adapt<PerfilTratamiento>();
                 nuevoTratamiento.CantidadStock = 0;
                 dest.PerfilTratamientos.Add(nuevoTratamiento);
             }
         }
     });

        TypeAdapterConfig<InsumoMovimientoVM, InsumoMovimiento>.NewConfig()
    .Ignore(dest => dest.FechaCreacion)
    .Ignore(dest => dest.UserAccountRef)
    .Ignore(dest => dest.Insumo); 
    }
}