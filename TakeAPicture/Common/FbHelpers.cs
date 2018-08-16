using System;
using System.Configuration;
using System.IO;
using Facebook;

namespace TakeAPicture.Common
{
    public class FbHelpers
    {
        public static String GetLoginUrl()
        {
            var client = new FacebookClient();
            var fbLoginUri = client.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["fb_key"],
                redirect_uri = "https://www.facebook.com/connect/login_success.html",  
                response_type = "code",
                display = "popup",
                scope = "email,publish_stream"
            });

            return fbLoginUri.ToString();      
        }

        public static String GetAccessToken(FacebookOAuthResult oauthResult)
        {
            var client = new FacebookClient();
            dynamic result = client.Get("/oauth/access_token",
                                        new
                                        {
                                            client_id = ConfigurationManager.AppSettings["fb_key"],
                                            client_secret = ConfigurationManager.AppSettings["fb_secret"],
                                            redirect_uri = "https://www.facebook.com/connect/login_success.html",  
                                            code = oauthResult.Code,
                                        });

            return result.access_token;
        }

        public static dynamic GetUser(String token)
        {
            var client = new FacebookClient();
            dynamic user = client.Get("/me", new { fields = "first_name,last_name,email,picture,gender", access_token = token });
            return user;
        }

        public static void Post(String token, String status)
        {
            if (status != "")
            {
            
            var client = new FacebookClient(token);
            dynamic result = client.Post("/me/feed", new { message = status });
            var x = 0;
            }
        }

        public static void PostWithPhoto(String token, String status, String photoPath)
        {
            var client = new FacebookClient(token);
            using (var stream = File.OpenRead(photoPath))
            {
                dynamic result = client.Post("me/photos",
                                                new
                                                {
                                                    message = status,
                                                    file = new FacebookMediaStream
                                                    {
                                                        ContentType = "image/jpg",
                                                        FileName = Path.GetFileName(photoPath)
                                                    }.SetValue(stream)
                                                });
            }
        }
    }
}