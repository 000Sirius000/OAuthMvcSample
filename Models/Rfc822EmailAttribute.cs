using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OAuthMvcSample.Models;

public class Rfc822EmailAttribute : ValidationAttribute
{
    private const string Pattern =
        @"^(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+" +
        @"(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|" +
        @"""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|" +
        @"\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@" +
        @"(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+" +
        @"[a-zA-Z]{2,}|\[(?:(?:25[0-5]|2[0-4][0-9]|" +
        @"[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|" +
        @"[01]?[0-9][0-9]?|[a-zA-Z0-9-]*[a-zA-Z0-9]:" +
        @"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|" +
        @"\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)])$";

    public override bool IsValid(object? value)
    {
        if (value is not string s || string.IsNullOrWhiteSpace(s))
            return false;
        return Regex.IsMatch(s, Pattern, RegexOptions.Compiled);
    }

    public override string FormatErrorMessage(string name)
        => "Email має відповідати формату RFC 822 (допустимий синтаксис адреси).";
}
