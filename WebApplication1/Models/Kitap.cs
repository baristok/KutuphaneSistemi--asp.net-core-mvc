using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApplication1.Models;

public class Kitap
{
    [Key] // primary key
    public int Id { get; set; }
    
    [Required]// not null
    [MaxLength(25)]// 25 max lenght
    public string KitapAdi { get; set; }
    
    //not null olmasın
    public string Tanim { get; set; }
    
    [Required]
    public string Yazar { get; set; }
    
    [Required]
    [Range(10,5000)]
    public double Fiyat { get; set; }
    [ValidateNever]
    public int KitapTuruId { get; set; }
    [ForeignKey("KitapTuruId")]
    [ValidateNever]
    public KitapTuru KitapTuru { get; set; }
    [ValidateNever]
    public string ResimURL { get; set; }
    
}