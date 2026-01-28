using System.ComponentModel.DataAnnotations;

namespace BlazorDeploy
{
    public enum TipoMovimentacao
    {
        Compra = 1,
        Venda = 2,
        Perda = 3,
        AjusteManual = 4
    }

    public enum Marcas
    {
        [Display(Name = "Brahma")] Brahma,
        [Display(Name = "Skol")] Skol,
        [Display(Name = "Antarctica")] Antarctica,
        [Display(Name = "Original")] Original,
        [Display(Name = "Xereta")] Xereta,
        [Display(Name = "Coca-Cola")] CocaCola,
        [Display(Name = "Fanta")] Fanta,
        [Display(Name = "Heineken")] Heineken,
        [Display(Name = "Budweiser")] Budweiser,
        [Display(Name = "Red Bull")] RedBull,
        [Display(Name = "Gatorade")] Gaterade,
        [Display(Name = "Petra")] Petra,
        [Display(Name = "Pepsi")] Pepsi,
        [Display(Name = "Sukita")] Sukita,
        [Display(Name = "Corona")] Corona,
        [Display(Name = "Tubaina")] Tubaina,
        [Display(Name = "Lacta")] Lacta,
        [Display(Name = "Eisenbahn")] Eisenbahn,
        [Display(Name = "Baianinha")] Baianinha,
        [Display(Name = "Chapinha")] Chapinha,
        [Display(Name = "Askov")] Askov,
        [Display(Name = "Stella")] Stella,
        [Display(Name = "Monster")] Monster,
        [Display(Name = "Amstel")] Amstel,
        [Display(Name = "Red label")] RedLabel,
        [Display(Name = "Del valle")] DelValle,
        [Display(Name = "Sprite")] Sprite,
        [Display(Name = "Imperio")] Imperio,
        [Display(Name = "Baly")] Baly,
        [Display(Name = "Crystal")] Crystal,
        [Display(Name = "Gorillaz")] Gorillaz
    }
}
