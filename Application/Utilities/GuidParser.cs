namespace Application.Utilities;

public static class GuidParser
{
    public static Guid? SafeParse(string? input)
        => Guid.TryParse(input, out var guid) ? guid : null;
}