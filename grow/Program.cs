using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace growtimelapse
{
    public class Program
    {

        private readonly static string cameraIP = Environment.GetEnvironmentVariable("CAMERA_IP");
        private static string cameraUrl;
        public static async Task Main(string[] args)
        {
            cameraUrl = "http://" + cameraIP + "//ISAPI/Streaming/Channels/101/picture?snapShotImageType=JPEG";
            ImageClient currentClient = new ImageClient();
            Store store = new Store("timelapse1");
            TimeSpan interval = new TimeSpan(0, 1, 0);
            while (true)
            {
                try
                {
                    HttpResponseMessage response = await currentClient.client.GetAsync(cameraUrl);
                    response.EnsureSuccessStatusCode();
                    Stream responseImage = await response.Content.ReadAsStreamAsync();

                    BlobClient blobClient = store.containerClient.GetBlobClient(DateTime.Now.ToString("yyyyMMddHHmmss") + ".JPEG");
                    await blobClient.UploadAsync(responseImage, true);

                }
                catch (Exception) {
                }

                System.Threading.Thread.Sleep(interval);

            }

        }


    }
}
