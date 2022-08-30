namespace NotinoHomework.Models;

using System.ComponentModel.DataAnnotations;

public record ConvertRequest([Required]string FileName, [Required]string Target);