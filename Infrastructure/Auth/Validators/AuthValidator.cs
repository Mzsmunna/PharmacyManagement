using Application.Dtos;
using Application.Payloads;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Auth.Validators
{
    public static class AuthValidator
    {
        public static AppError ValidateSignUp(SignUpPayload payload)
        {
            var title = "Auth.SignUp";
            IDictionary<string, string[]> errors = new Dictionary<string, string[]>();
            List<string> messages = [];
            if (payload is null) return AppError.Bad(title, "Bad Request");         
            if (string.IsNullOrEmpty(payload.Name)) messages.Add("Name is required");
            if (payload.Name.Length < 3) messages.Add("Name lenth is too short");
            if (payload.Name.Length > 100) messages.Add("Name lenth is too big");
            if (messages.Count > 0) errors.Add($"{title}.Name", messages.ToArray()); 
            messages = [];
            return errors.Any() ? AppError.Validation($"{title}.Validation", 
                "One or more input in invalid", errors) : AppError.Ok;
        }

        public static AppError ValidateSignIn(SignInPayload payload)
        {
            return AppError.Ok;
        }
    }
}
