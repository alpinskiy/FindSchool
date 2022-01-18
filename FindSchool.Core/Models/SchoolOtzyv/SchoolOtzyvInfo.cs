using FindSchool.Core.Extensions;

namespace FindSchool.Core.Models.SchoolOtzyv;

public class SchoolOtzyvInfo
{
    public SchoolOtzyvInfo(SchoolOtzyvItem item, SchoolOtzyvDetails? details)
    {
        Item = item;
        Details = details;
    }

    public SchoolOtzyvItem Item { get; set; }
    public SchoolOtzyvDetails? Details { get; set; }

    public School ToSchool()
    {
        return new School
        {
            SchoolOtzyvId = Item.Id,
            Name = Item.Name,
            Description = string.Empty,
            Address = Details?.Address,
            Number = Item.Name.TryGetSingleNumber(out var number)
                ? number
                : null,
            Rating = Item.Rating,
            CommentCount = Item.CommentCount
        };
    }
}