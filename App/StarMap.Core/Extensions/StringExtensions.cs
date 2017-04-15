namespace StarMap.Core.Extensions
{
  public static class StringExtensions
  {

    /// <summary>
    /// Indicates whether the specified string is null or an System.String.Empty string.
    /// </summary>
    /// <param name="s">The string to test.</param>
    /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string s)
    {
      return string.IsNullOrEmpty(s);
    }

    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="s">The string to test.</param>
    /// <returns>true if the value parameter is null or System.String.Empty, or if value consists exclusively of white-space characters.</returns>
    public static bool IsNullOrWhiteSpace(this string s)
    {
      return string.IsNullOrWhiteSpace(s);
    }
  }
}
