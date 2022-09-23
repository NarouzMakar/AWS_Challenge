

using Amazon.S3;
using Amazon.S3.Model;

public class UploaderHelper
{
    public static void PrepareDataForFileUpload(string jsonString)
    {
        try
        {
            string bucketName = Environment.GetEnvironmentVariable("Bucket_Name");
            string fileFullName = $"{DateTime.Now.ToString("MM-dd-yyyy")}/{Guid.NewGuid()}.json";
            UploadFile(bucketName, fileFullName, "application/json", jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in PrepareDataForFileUpload");
            Console.WriteLine("Exception : {0}", ex);
        }
    }
    static void UploadFile(string s3BucketName, string fileName, string contentType, string contentBody)
    {
        try
        {
            Console.WriteLine("UploadFile " + fileName);
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("", "");
            var client = new AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.USEast1);
            var putRequest = new PutObjectRequest
            {
                BucketName = s3BucketName,
                Key = fileName,
                ContentType = contentType,
                ContentBody = contentBody
            };
            client.PutObjectAsync(putRequest).Wait();
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
            {
                Console.WriteLine("Invalid credentials : {0}", amazonS3Exception.Message);
                Console.WriteLine("Exception : {0}", amazonS3Exception);
            }
            else
            {
                Console.WriteLine("Error Message : {0}", amazonS3Exception.Message);
                Console.WriteLine("Exception : {0}", amazonS3Exception);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Message : {0}", ex.Message);
            Console.WriteLine("Exception : {0}", ex);
        }
    }
}
