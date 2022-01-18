namespace FindSchool.Core.Models;

public class School
{
    public int? APeterburgId { get; set; }
    public int? SchoolOtzyvId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public int? Number { get; set; }
    public decimal Rating { get; set; }
    public int CommentCount { get; set; }
}