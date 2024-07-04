using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using EFTraining.Entities;

namespace EFTraining.Services;

public class FileService : IFileService
{
    private readonly IConfiguration _configuration;

    public FileService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<S3Response> DeleteFile(string keyName)
    {
        AWSCredentials credentials = new BasicAWSCredentials(
            GetConfigSetting("AccessKey"),
            GetConfigSetting("SecretKey"));

        AmazonS3Config config = new AmazonS3Config()
        {
            ServiceURL = GetConfigSetting("ServiceURL"),
            UseHttp = true,
            ForcePathStyle = true
        };

        var response = new S3Response();

        var request = new DeleteObjectRequest()
        {
            Key = keyName,
            BucketName = GetConfigSetting("BucketName")
        };

        using var client = new AmazonS3Client(credentials, config);
        DeleteObjectResponse s3response = await client.DeleteObjectAsync(request);

        try
        {
            response.StatusCode = 200;
            response.Message = $"{keyName} has been deleted successfully";
        }
        catch (AmazonS3Exception ex)
        {
            response.StatusCode = (int)ex.StatusCode;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<S3Response> DownloadFile(string keyName, string filepath)
    {
        AWSCredentials credentials = new BasicAWSCredentials(
            GetConfigSetting("AccessKey"),
            GetConfigSetting("SecretKey"));

        AmazonS3Config config = new AmazonS3Config()
        {
            ServiceURL = GetConfigSetting("ServiceURL"),
            UseHttp = true,
            ForcePathStyle = true
        };

        var response = new S3Response();

        var request = new GetObjectRequest()
        {
            Key = keyName,
            BucketName = GetConfigSetting("BucketName")
        };

        using var client = new AmazonS3Client(credentials, config);
        using GetObjectResponse s3response = await client.GetObjectAsync(request);

        try
        {
            // Save object to local file
            await s3response.WriteResponseStreamToFileAsync($"{filepath}\\{keyName}", true, CancellationToken.None);
            response.StatusCode = 200;
            response.Message = $"{keyName} has been downloaded successfully";
        }
        catch (AmazonS3Exception ex)
        {
            response.StatusCode = (int)ex.StatusCode;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<S3Response> UploadFile(Entities.S3Object s3obj)
    {
        AWSCredentials credentials = new BasicAWSCredentials(
            GetConfigSetting("AccessKey"),
            GetConfigSetting("SecretKey"));

        AmazonS3Config config = new AmazonS3Config()
        {
            ServiceURL = GetConfigSetting("ServiceURL"),
            UseHttp = true,
            ForcePathStyle = true
        };

        var response = new S3Response();

        try 
        {
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = s3obj.File,
                Key = s3obj.Name,
                BucketName = GetConfigSetting("BucketName"),
                CannedACL = S3CannedACL.NoACL
            };

            using var client = new AmazonS3Client(credentials, config);

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);

            response.StatusCode = 200;
            response.Message = $"{s3obj.Name} has been uploaded successfully";

        }
        catch(AmazonS3Exception ex)
        {
            response.StatusCode = (int)ex.StatusCode;
            response.Message = ex.Message;
        }
        catch(Exception ex) 
        {
            response.StatusCode = 500;
            response.Message = ex.Message;
        }

        return response;

    }

    string GetConfigSetting(string key)
    {
        return _configuration.GetSection("S3Storage").GetValue<string>(key) ?? "";
    }
}

