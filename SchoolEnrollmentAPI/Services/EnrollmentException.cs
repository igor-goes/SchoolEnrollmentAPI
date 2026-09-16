using System;
using System.Net;

namespace SchoolEnrollmentAPI.Services
{
    /// <summary>
    /// Transporta uma falha esperada de matrícula e o status HTTP adequado para ela.
    /// </summary>
    public class EnrollmentException : Exception
    {
        public EnrollmentException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}
