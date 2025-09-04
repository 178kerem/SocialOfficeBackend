namespace SocialOffice.Domain.Utilities.Results
{
    public interface IResult
    {
        bool IsSuccess { get; set; }
        List<KeyValuePair<string, string>> Messages { get; set; }
    }
}

