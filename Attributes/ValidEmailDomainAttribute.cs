using System;
using System.ComponentModel.DataAnnotations;

public class ValidEmailDomainAttribute : ValidationAttribute
{
    // Array to hold the allowed email domains
    private readonly string[] _allowedDomains;

    // Constructor to initialize the allowed domains
    public ValidEmailDomainAttribute(params string[] allowedDomains)
    {
        _allowedDomains = allowedDomains;
    }

    // Override the IsValid method to perform the domain validation
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Cast the value to a string (expected to be an email address)
        var email = value as string;

        // Check if the email is null or empty
        if (string.IsNullOrEmpty(email))
        {
            return new ValidationResult("Email address is required.");
        }

        // Extract the domain part of the email address
        var emailDomain = email.Substring(email.IndexOf('@') + 1);

        // Check if the extracted domain matches any of the allowed domains
        foreach (var domain in _allowedDomains)
        {
            if (emailDomain.Equals(domain, StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Success; // Return success if a match is found
            }
        }

        // Return an error message if none of the allowed domains match
        return new ValidationResult($"Email address must be one of the following domains: {string.Join(", ", _allowedDomains)}.");
    }
}
