using FindSchool.Core.Extensions;

namespace FindSchool.Core.Models.APeterburg;

public class APeterburgInfo
{
    public APeterburgInfo(APeterburgItem shallowInfo, APeterburgDetails? details)
    {
        ShallowInfo = shallowInfo;
        Details = details;
    }

    public APeterburgItem ShallowInfo { get; }
    public APeterburgDetails? Details { get; }

    public School ToSchool()
    {
        if (ShallowInfo == null || Details == null)
        {
            throw new InvalidOperationException("School has no details");
        }

        return new School
        {
            APeterburgId = ShallowInfo.Id,
            Name = Details.ShortName ?? Details.FullName,
            Description = string.Join('\n', Details.FullName, Details.Url),
            Address = ShallowInfo.Address,
            Number = (Details.ShortName ?? Details.FullName).TryGetSingleNumber(out var number)
                ? number
                : null,
            Rating = ShallowInfo.Rating,
            CommentCount = ShallowInfo.CommentCount
        };
    }
}