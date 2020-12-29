using System;
using Azure.Storage.Blobs;
using System.IO;

namespace growtimelapse
{
    public class Store
    {
        public BlobServiceClient blobServiceClient;
        public BlobContainerClient containerClient;
        string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        public Store(string ContainerName)
        {
            blobServiceClient = new BlobServiceClient(connectionString);
            containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        }

       
    }
}
