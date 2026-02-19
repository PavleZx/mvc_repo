using System.ComponentModel.DataAnnotations;


namespace mvc_app.Models;

public class Expense
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    [Range(0.0001, double.MaxValue, ErrorMessage = "Please enter a positive value")]
    public DateTime Date { get; set; } = DateTime.Now;
}