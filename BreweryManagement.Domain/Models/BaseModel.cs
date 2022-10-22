using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Domain.Models
{
    public class BaseModel
    {
        [Required, Key, MaxLength(128)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required, MaxLength(200), Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; } = "DefaultName";
    }
}
