using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace growtimelapse
{
    public class Program
    {

        private static readonly string cameraUrl = @"http://192.168.1.74//ISAPI/Streaming/Channels/101/picture?snapShotImageType=JPEG";
        public static async Task Main(string[] args)
        {
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
