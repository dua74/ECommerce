﻿namespace Ecom.API.Helper
{
    public class APIExceptions : ResponseAPI
    {
        public APIExceptions(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; } 
    }
}
