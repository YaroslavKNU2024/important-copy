using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirusAppFinal.Models;

public class CountriesEdit {
    public int Id { get; set; }
    //[Display(Name = "Назва")]
    //[StringLength(255, ErrorMessage = "Занадто коротке або занадто довге.")]
    //[Required(ErrorMessage = "Поле не повинно бути пустим.")]
    public string? CountryName { get; set; }

    public List<int> VariantsIds { get; set; } = new List<int>();
}
