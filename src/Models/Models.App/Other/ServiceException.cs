// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Other.Models
{
    /// <summary>
    /// Graph service exception.
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Creates a new service exception.
        /// </summary>
        /// <param name="error">The error that triggered the exception.</param>
        /// <param name="innerException">The possible innerException.</param>
        public ServiceException(ServerResponse error, Exception innerException = null)
            : this(error, responseHeaders: null, statusCode: default, innerException: innerException)
        {
        }

        /// <summary>
        /// Creates a new service exception.
        /// </summary>
        /// <param name="error">The error that triggered the exception.</param>
        /// <param name="innerException">The possible innerException.</param>
        /// <param name="responseHeaders">The HTTP response headers from the response.</param>
        /// <param name="statusCode">The HTTP status code from the response.</param>
        public ServiceException(ServerResponse error, System.Net.Http.Headers.HttpResponseHeaders responseHeaders, System.Net.HttpStatusCode statusCode, Exception innerException = null)
            : base(error?.ToString(), innerException)
        {
            this.Error = error;
            this.ResponseHeaders = responseHeaders;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Creates a new service exception.
        /// </summary>
        /// <param name="error">The error that triggered the exception.</param>
        /// <param name="innerException">The possible innerException.</param>
        /// <param name="responseHeaders">The HTTP response headers from the response.</param>
        /// <param name="statusCode">The HTTP status code from the response.</param>
        /// <param name="rawResponseBody">The raw JSON response body.</param>
        public ServiceException(
            ServerResponse error,
            System.Net.Http.Headers.HttpResponseHeaders responseHeaders,
            System.Net.HttpStatusCode statusCode,
            string rawResponseBody,
            Exception innerException = null)
            : this(error, responseHeaders, statusCode, innerException)
        {
            this.RawResponseBody = rawResponseBody;
        }

        /// <summary>
        /// The error from the service exception.
        /// </summary>
        public ServerResponse Error { get; }

        // ResponseHeaders and StatusCode exposed as pass-through.

        /// <summary>
        /// The HTTP response headers from the response.
        /// </summary>
        public System.Net.Http.Headers.HttpResponseHeaders ResponseHeaders { get; }

        /// <summary>
        /// The HTTP status code from the response.
        /// </summary>
        public System.Net.HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Provide the raw JSON response body.
        /// </summary>
        public string RawResponseBody { get; }

        /// <summary>
        /// Checks if a given error code has been returned in the response at any level in the error stack.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns>True if the error code is in the stack.</returns>
        public bool IsMatch(string errorCode)
        {
            if (string.IsNullOrEmpty(errorCode))
            {
                throw new ArgumentException("errorCode cannot be null or empty", "errorCode");
            }

            var currentError = this.Error;

            if (string.Equals(currentError.Code.ToString(), errorCode, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $@"Status Code: {this.StatusCode}{Environment.NewLine}{base.ToString()}";
        }
    }
}
