using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anodica.Modelos
{
    [Table("PerfilEquivalencia")]
    public class PerfilEquivalencia
    {
        [Key]
        public int PerfilEquivalenciaId { get; set; }

        [Required]
        public int PerfilRef { get; set; }

        [Required]
        public int PerfilEquivalenteRef { get; set; }


        [ForeignKey("PerfilRef")]
        public Perfil PerfilPrincipal { get; set; }

        [ForeignKey("PerfilEquivalenteRef")]
        public Perfil PerfilAlternativo { get; set; }
    }
}