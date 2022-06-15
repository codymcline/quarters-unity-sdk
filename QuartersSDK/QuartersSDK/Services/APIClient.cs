﻿using NetTopologySuite.Utilities;
using Newtonsoft.Json;
using QuartersSDK.Data;
using QuartersSDK.Data.Enums;
using QuartersSDK.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace QuartersSDK.Services
{
    public class APIClient : IAPIClient
    {
        private ResponseData DoPost(HttpContent payload, string subPath)
        {
            var rdo = new ResponseData();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(subPath);
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(payload.Headers.ContentType.MediaType));

                    // make request
                    var request = httpClient.PostAsync(subPath, payload);
                    request.Wait();
                    var response = request.Result.Content.ReadAsStringAsync();

                    // check for error
                    if (!request.Result.IsSuccessStatusCode)
                    {
                        rdo.SetError(new Error(response.Result), request.Result.StatusCode);
                        return rdo; 
                    }
                    rdo.SetData(response.Result, request.Result.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Error error = new Error(ex.Message, ex.InnerException.ToString());
                throw ex;
            }
            return rdo;
        }

        public ResponseData RequestPost(string url, RequestData request)
        {
            try
            {
                var data = new MultipartFormDataContent();
                data.Add(new StringContent(request.GrantType), "grant_type");
                data.Add(new StringContent(request.ClientId), "client_id");
                data.Add(new StringContent(request.ClientSecret), "client_secret");
                data.Add(new StringContent(request.RefreshToken), "refresh_token");
                if (!String.IsNullOrEmpty(request.CodeVerifier))
                    data.Add(new StringContent(request.CodeVerifier), "code_verifier");
                if (!String.IsNullOrEmpty(request.Code))
                    data.Add(new StringContent(request.Code), "code");

                return DoPost(data, url);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex.Message);
                throw ex;
            }
        }

        public HttpWebResponse RequestGet(string url, string requestToken)
        {
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                httpRequest.Accept = "application/json";
                httpRequest.Headers["Authorization"] = $"Bearer ${requestToken}";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                return httpResponse;
            }
            catch (Exception ex)
            {
                Error error = new Error(ex.Message);
                throw ex;
            }
        }
    }
}