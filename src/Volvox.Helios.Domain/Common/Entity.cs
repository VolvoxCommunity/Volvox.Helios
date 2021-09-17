using System.ComponentModel.DataAnnotations;

namespace Volvox.Helios.Domain.Common
{
    public class Entity
    {
        [Key] public int Id { get; set; }
    }
}