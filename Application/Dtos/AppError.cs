using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public sealed record AppError(ErrorType Type,
        string Title,
        string Message,
        string Status = "",
        int StatusCode = 102,
        IDictionary<string, string[]>? Details = null
    )
    {
        public static AppError Ok = new(ErrorType.Ok, string.Empty, string.Empty);
        public static AppError Missing(string title = "", string message = "Data for Requested query / payload seems missing", IDictionary<string, string[]>? details = null) => new(ErrorType.Missing, title, message, "204-missing", 204, details);
        public static AppError Bad(string title = "", string message = "Requested body payload seems empty or corrupted", IDictionary<string, string[]>? details = null) => new(ErrorType.Bad, title, message, "400-bad-request", 400, details);
        public static AppError Unauthorized(string title = "", string message = "We didn't recognize the user", IDictionary<string, string[]>? details = null) => new(ErrorType.Unauthorized, title, message, "401-unauthorized", 401, details);
        public static AppError Forbidden(string title = "", string message = "User may not have valid permission", IDictionary<string, string[]>? details = null) => new(ErrorType.Forbidden, title, message, "403-forbidden", 403, details);
        public static AppError NotFound(string title = "", string message = "Data Not found", IDictionary<string, string[]>? details = null) => new(ErrorType.NotFound, title, message, "404-not-found", 404, details);
        public static AppError Conflict(string title = "", string message = "Data Maybe already exists", IDictionary<string, string[]>? details = null) => new(ErrorType.Conflict, title, message, "409-conflict", 409, details);
        public static AppError Validation(string title = "", string message = "Payload seems invalid", IDictionary<string, string[]>? details = null) => new(ErrorType.Validation, title, message, "422-unprocessable-content", 422, details);
        public static AppError Server(string title = "", string message = "Something went wrong", IDictionary<string, string[]>? details = null) => new(ErrorType.Server, title, message + "; " + Environment.StackTrace, "500-internal-server-error", 500, details);
        public static AppError Network(string title = "", string message = "Network issue while fetching data", IDictionary<string, string[]>? details = null) => new(ErrorType.Network, title, message, "502-bad-gateway", 502, details);
    }
}
