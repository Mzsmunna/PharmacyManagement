using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    public class AppException : Exception
    {
        public AppError Error { get; }
        public AppException(AppError error) : base(error.Message)
        {
            Error = error;
        }
    }
}
