using Mapster;
using Anodica.Modelos;
using AnodicaV2DEMO.ViewModels; // O donde guardes tus VMs

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Insumo, InsumoVM>.NewConfig();

        TypeAdapterConfig<InsumoVM, Insumo>.NewConfig()
    .Ignore(dest => dest.CantidadStock);

        TypeAdapterConfig<PerfilVM, Perfil>.NewConfig()
    .Ignore(dest => dest.ImagenPerfil)
    .Ignore(dest => dest.PesoXtira)   //valor derivado de calculo
    .Ignore(dest => dest.PerfilTratamientos); 

        TypeAdapterConfig<InsumoMovimientoVM, InsumoMovimiento>.NewConfig()
    .Ignore(dest => dest.FechaCreacion)
    .Ignore(dest => dest.UserAccountRef)
    .Ignore(dest => dest.Insumo); 
    }
}