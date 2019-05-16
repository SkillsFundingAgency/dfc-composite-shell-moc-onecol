using System.ComponentModel.DataAnnotations;

namespace DFC.Composite.Shell.Moc.OneCol.Data
{
    public class Category : Models.BaseEditViewModel
    {
        [Display(Name = "Category", Prompt = "Category", Description = "Enter the Category for this Trade")]
        [Required(ErrorMessage = FieldMandatoryhValidationError)]
        public string Name { get; set; }

        [Display(Name = "Trades", Prompt = "Trades", Description = "Enter the number of trades for this Category")]
        [Required(ErrorMessage = FieldMandatoryhValidationError)]
        public int TradeCount { get; set; }
    }
}
