﻿// -----------------------------------------------------------------------
// <copyright file="JsonServiceClient.cs" company="YamNet">
//   Copyright (c) 2013 YamNet contributors
// </copyright>
// -----------------------------------------------------------------------

namespace YamNet.Client
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using YamNet.Client.Exceptions;

    /// <summary>
    /// The JSON web service client.
    /// </summary>
    public partial class JsonServiceClient
    {
        /// <summary>
        /// The http client.
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// The http client handler.
        /// </summary>
        private HttpClientHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonServiceClient"/> class.
        /// </summary>
        /// <param name="token">The access token.</param>
        /// <param name="proxy">The proxy.</param>
        /// <param name="credentials">The credentials.</param>
        public JsonServiceClient(string token, IWebProxy proxy, ICredentials credentials)
        {
            this.handler =
                new HttpClientHandler
                    {
                        UseProxy = true,
                        Proxy = proxy,
                        UseDefaultCredentials = false,
                        Credentials = credentials,
                        PreAuthenticate = true
                    };

            this.Init(token);
        }

        /// <summary>
        /// Post via async.
        /// </summary>
        /// <param name="uri">The full uri.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <returns>The <see cref="Task{BaseEnvelope{object}}"/>.</returns>
        internal async Task<IBaseEnvelope<object>> PostAsync(string uri, object parameters = null)
        {
            return await this.PostAsync<object>(uri, parameters);
        }

        /// <summary>
        /// Post via HTTP async.
        /// </summary>
        /// <param name="uri">The full uri.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <typeparam name="T">The class type.</typeparam>
        /// <returns>The <see cref="Task{BaseEnvelope{T}}"/>.</returns>
        internal async Task<IBaseEnvelope<T>> PostAsync<T>(string uri, object parameters = null)
            where T : class
        {
            return await this.ExecuteAsync<T>(HttpMethod.Post, uri, parameters);
        }

        /// <summary>
        /// Get via HTTP async.
        /// </summary>
        /// <param name="uri">The full uri.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <typeparam name="T">The class type.</typeparam>
        /// <returns>The <see cref="Task{BaseEnvelope{T}}"/>.</returns>
        internal async Task<IBaseEnvelope<T>> GetAsync<T>(string uri, object parameters = null)
            where T : class
        {
            return await this.ExecuteAsync<T>(HttpMethod.Get, uri, parameters);
        }

        /// <summary>
        /// Delete via HTTP async.
        /// </summary>
        /// <param name="uri">The full uri.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <returns>The <see cref="Task{BaseEnvelope{object}}"/>.</returns>
        internal async Task<IBaseEnvelope<object>> DeleteAsync(string uri, object parameters = null)
        {
            return await this.DeleteAsync<object>(uri, parameters);
        }

        /// <summary>
        /// Delete via HTTP async.
        /// </summary>
        /// <param name="uri">The full uri.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <typeparam name="T">The class type.</typeparam>
        /// <returns>The <see cref="Task{BaseEnvelope{T}}"/>.</returns>
        internal async Task<IBaseEnvelope<T>> DeleteAsync<T>(string uri, object parameters = null)
            where T : class
        {
            return await this.ExecuteAsync<T>(HttpMethod.Delete, uri, parameters);
        }

        /// <summary>
        /// Execute via HTTP async.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="uri">The full uri.</param>
        /// <param name="parameters">The query parameters.</param>
        /// <typeparam name="T">The class type.</typeparam>
        /// <returns>The <see cref="Task{BaseEnvelope{T}}"/>.</returns>
        protected async Task<IBaseEnvelope<T>> ExecuteAsync<T>(
            HttpMethod method,
            string uri,
            object parameters)
            where T : class
        {
            const int Retry = 5;

            var tryAgain = true;
            var counter = 0;

            var response = default(HttpResponseMessage);
            var result = default(IBaseEnvelope<T>);

            try
            {
                // Create the HTTP Request object based on HttpClient PCL
                var request = new HttpRequestObject(this.Serializer);
                var defaultDelay = TimeSpan.FromSeconds(10);

                while (tryAgain && counter++ < Retry)
                {
                    var delay = defaultDelay;

                    // Send the HttpRequest asynchronously
                    response =
                        await request.ExecuteRequestAsync<T>(
                            this.client,
                            method,
                            this.Endpoint,
                            uri,
                            parameters);

                    // Check if response was a success
                    // i.e. HTTP 200 OK
                    // TODO: Need refactor, this flag may not belong here
                    tryAgain = !response.IsSuccessStatusCode;

                    if (tryAgain)
                    {
                        switch ((int)response.StatusCode)
                        {
                            case 401:
                                // 401 Unauthorised Exception.
                                // Immediately retry on first and second request.
                                // If auth is required, sometimes two request are
                                // required, first one to determine which scheme
                                // are being used.
                                // Reference:
                                // http://stackoverflow.com/a/17025435/1615437
                                delay = counter <= 1
                                    ? TimeSpan.FromMilliseconds(1)
                                    : defaultDelay;
                                break;

                            case 429:
                                // 429 Too Many Requests.
                                // Log the exception if the API rate limit
                                // has been exceeded.
                                // Reference:
                                // https://developer.yammer.com/restapi/#rest-ratelimits
                                // TODO: Create specified delay based on docs
                                Debug.WriteLine("Rate Limit Exceeded");

                                // Wait 10 seconds before trying again
                                delay = defaultDelay;
                                break;
                        }

                        // Retry request with a specified delay
                        await TaskEx.Delay(delay);
                    }
                }

                if (!tryAgain)
                {
                    var responseHandler =
                        new HttpResponseHandler(
                            this.Deserializer,
                            this.ResponseErrorHandler);

                    result = await responseHandler.HandleResponseAsync<T>(response);
                    response.Dispose();
                }
            }
            catch (AggregateException ae)
            {
                // Catch Yammer aggregate exception
                ae.Flatten().Handle(e =>
                    {
                        // Return all exception other than Rate Limit Exceeded type
                        if (e.GetType() != typeof(RateLimitExceededException))
                        {
                            result =
                                new BaseEnvelope<T>
                                    {
                                        Exception = e.InnerException ?? e
                                    };
                        }

                        return true;
                    });
            }
            catch (HttpRequestException httpEx)
            {
                // Catch other HTTP exception
                if (httpEx.InnerException is WebException)
                {
                    var webEx = httpEx.InnerException as WebException;
                    var status = webEx.Status;

                    if (status.ToString() == "NameResolutionFailure")
                    {
                        var offlineException = new OfflineException();

                        result = new BaseEnvelope<T> { Exception = offlineException };
                    }
                }
            }
            catch (Exception ex)
            {
                result = new BaseEnvelope<T> { Exception = ex };
            }

            return result;
        }

        /// <summary>
        /// Initialise the service client.
        /// </summary>
        /// <param name="token">The access token.</param>
        private void Init(string token)
        {
            this.client =
                this.handler == null
                    ? new HttpClient()
                    : new HttpClient(this.handler);

            // Add the bearer token and set timeout to 60 seconds
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            this.client.Timeout = TimeSpan.FromSeconds(60);
        }

        #region Disposable
        /// <summary>
        /// Finalizes an instance of the <see cref="JsonServiceClient"/> class. 
        /// </summary>
        ~JsonServiceClient()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Dispose the current object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Release managed and, optionally, unmanaged resources.
        /// </summary>
        /// <param name="disposing">Set <c>true</c> to release both managed and unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (!disposing)
            {
                if (this.client != null)
                {
                    this.client.Dispose();
                    this.client = null;
                }

                if (this.handler != null)
                {
                    this.handler.Dispose();
                    this.handler = null;
                }

                GC.SuppressFinalize(this);
            }

            // No unmanaged resources to release
            this.disposed = true;
        }
        #endregion
    }
}
