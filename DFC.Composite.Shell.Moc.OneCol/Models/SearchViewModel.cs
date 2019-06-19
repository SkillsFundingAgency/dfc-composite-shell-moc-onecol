using System.ComponentModel.DataAnnotations;

namespace DFC.Composite.Shell.Moc.OneCol.Models
{
    public class SearchViewModel : BaseViewModel
    {
        [Display(Name = "Search Clue", Prompt = "Search Clue", Description = "Enter a Search Clue for a Trade")]
        public string Clue { get; set; }
    }
}
