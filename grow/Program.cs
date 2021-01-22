using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Serilog;

namespace growtimelapse
{
    public class Program
    {

        private readonly static string cameraIP = Environment.GetEnvironmentVariable("CAMERA_IP");
        private static string cameraUrl;
        public static async Task<int> Main(string[] args)
        {
            ILogger logger = null;
            try 
            {
                logger = LoggerFactory.CreateLogger();
            }
            catch(Exception exc) 
            {
                Console.WriteLine($"Error creating logger: {exc}");
                return 1;
            }

            logger.Information($"Timelapse service is starting. Camera host is {cameraIP}.");

            cameraUrl = "http://" + cameraIP + "//ISAPI/Streaming/Channels/101/picture?snapShotImageType=JPEG";
            ImageClient currentClient = new ImageClient();
            Store store = new Store("timelapse1");
            TimeSpan interval = new TimeSpan(1, 0, 0);
            while (true)
            {
                try
                {
                    logger.Information("Capturing a frame...");

                    HttpResponseMessage response = await currentClient.client.GetAsync(cameraUrl);
                    response.EnsureSuccessStatusCode();
                    Stream responseImage = await response.Content.ReadAsStreamAsync();

                    logger.Information("Sending the frame to cloud...");

                    BlobClient blobClient = store.containerClient.GetBlobClient(DateTime.Now.ToString("yyyyMMddHHmmss") + ".JPEG");
                    await blobClient.UploadAsync(responseImage, true);

                }
                catch (Exception exc) 
                {
                    logger.Error(exc, "Error capturing a frame");
                }

                System.Threading.Thread.Sleep(interval);
            }
        }
    }
}

