﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApplication1.Models;

public class Kiralama
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int OgrenciId { get; set; }
    
    [ValidateNever]
    public int KitapId { get; set; }
    [ForeignKey("KitapId")]
    
    [ValidateNever]
    public Kitap Kitap { get; set; }
}