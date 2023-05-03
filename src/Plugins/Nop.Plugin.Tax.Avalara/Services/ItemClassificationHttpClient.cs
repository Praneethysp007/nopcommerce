﻿using System.Text;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Tax.Avalara.ItemClassificationAPI;

namespace Nop.Plugin.Tax.Avalara.Services
{
    /// <summary>
    /// Represents HTTP client to request Avalara Classification services
    /// </summary>
    public class ItemClassificationHttpClient
    {
        #region Fields

        protected readonly AvalaraTaxSettings _avalaraTaxSettings;
        protected readonly HttpClient _httpClient;

        #endregion

        #region Ctor

        public ItemClassificationHttpClient(
            AvalaraTaxSettings avalaraTaxSettings,
            HttpClient httpClient)
        {
            //configure client
            httpClient.BaseAddress = new Uri($"{AvalaraTaxDefaults.ClassificationUrl}/");
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, AvalaraTaxDefaults.UserAgent);
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, MimeTypes.ApplicationJson);

            //avalaraTaxSettings.CompanyId, avalaraTaxSettings.LicenseKey);

            _avalaraTaxSettings = avalaraTaxSettings;
            _httpClient = httpClient;
            
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Set security using CompanyId / License Key
        /// </summary>
        /// <returns>The asynchronous task whose result contains access token</returns>
        protected string PrepareSecurity()
        {
            if (_avalaraTaxSettings.CompanyId is null)
                throw new NopException("Company not selected");

            if (_avalaraTaxSettings.LicenseKey is null)
                throw new NopException("LicenseKey is not set");

            var s = $"{_avalaraTaxSettings.CompanyId}:{_avalaraTaxSettings.LicenseKey}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Request services
        /// </summary>
        /// <typeparam name="TRequest">Request type</typeparam>
        /// <typeparam name="TResponse">Response type</typeparam>
        /// <param name="request">Request</param>
        /// <returns>The asynchronous task whose result contains response details</returns>
        public async Task<HSClassificationModel> RequestAsync<TRequest>(TRequest request) where TRequest : Request
        {
            try
            {
                //prepare request parameters
                var requestString = JsonConvert.SerializeObject(request);
                var requestContent = new StringContent(requestString, Encoding.Default, MimeTypes.ApplicationJson);

                //execute request and get response
                var requestMessage = new HttpRequestMessage(new HttpMethod(request.Method), request.Path) { Content = requestContent };

                //add authorization
                var securityToken = PrepareSecurity();

                //call PrepareSecurity
                requestMessage.Headers.Add(HeaderNames.Authorization, $"Basic {securityToken}");

                var httpResponse = await _httpClient.SendAsync(requestMessage);

                //return result
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response>(responseString);
                if (!string.IsNullOrEmpty(result.Error))
                    throw new NopException($"Item HS classification error - {result.Error}");

                return result;
            }
            catch (AggregateException exception)
            {
                //rethrow actual exception
                throw exception.InnerException;
            }
        }

        #endregion
    }
}
