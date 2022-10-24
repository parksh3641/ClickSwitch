using System;
using System.Text;
public static class Extensions
{
    public static string ReplaceAt(this string input, int index, char newChar)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input");
        }
        StringBuilder builder = new StringBuilder(input);
        builder[index] = newChar;
        return builder.ToString();
    }
}