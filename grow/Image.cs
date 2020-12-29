using System;
using System.Net;
using System.IO;
using System.Net.Http;

namespace growtimelapse
{
    public class ImageClient
    {
        public Stream loadedStream;

        public HttpClient client;
        public HttpClientHandler httpClientHandler;
        public Stream responseBody = null;
        
        private readonly string cameraLogin = Environment.GetEnvironmentVariable("CAMERA_LOGIN");
        private readonly string cameraPassword = Environment.GetEnvironmentVariable("CAMERA_PASSWORD");

        public ImageClient()
        {
            httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential(cameraLogin, cameraPassword),
            };

            client = new HttpClient(httpClientHandler);
            
        }

    }
}
