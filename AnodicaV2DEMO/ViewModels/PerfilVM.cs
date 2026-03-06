using Anodica.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PerfilVM
{
    public Perfil Perfil { get; set; }
    public IEnumerable<SelectListItem> LineasList { get; set; }
    public IEnumerable<SelectListItem> UbicacionesList { get; set; }
}