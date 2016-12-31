using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataSphere.Plugins.RandomGeneratorExample;
using Newtonsoft.Json;
using Tweetinvi;
using Tweetinvi.Models;

namespace DataSphere.Plugins.Twitter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filter = String.Empty;
            while (String.IsNullOrWhiteSpace(filter))
            {
                Console.WriteLine("What do you want to display? ");
                filter = Console.ReadLine();
            }

            Common.DataSphereClient client = new Common.DataSphereClient("127.0.0.1", 9001);
            Auth.SetUserCredentials(args[0], args[1], args[2], args[3]);

            var stream = Stream.CreateFilteredStream();
            stream.AddTrack(filter);
            stream.MatchingTweetReceived += (sender, message) =>
            {
                //Console.WriteLine("A tweet containing " + filter + " has been found; the tweet is '" + message.Tweet + "'");
                var tweet = JsonConvert.DeserializeObject<TweetViewModel>(message.Json);
                if (tweet?.place?.boundingBox != null)
                {
                    foreach (var box in tweet.place.boundingBox.coordinates)
                    {
                        foreach (var coordinate in box)
                        {
                            SubmitMessage(client, coordinate);
                            break;
                        }
                    }
                }
                //foreach (var location in message.MatchingLocations)
                //{
                //    SubmitMessage(client, location.Coordinate1);
                //    SubmitMessage(client, location.Coordinate2);
                //}
            };
            stream.StartStreamMatchingAllConditions();

            while (true)
            {
                Task.Delay(1000);
                Console.WriteLine("Waiting...");
            }
        }

        private static void SubmitMessage(Common.DataSphereClient client, double[] coordinate)
        {
            if (coordinate != null)
            {
                Console.WriteLine("GRAPHING COORDINATE: " + coordinate);
                var dataPoint = new DataPointViewModel()
                {
                    color = new DataPointColor(),
                    declination = (float) coordinate[0]/360.0f,
                    rightAscension = (float) (coordinate[1])/90.0f
                };
                var message = JsonConvert.SerializeObject(dataPoint);
                client.SendMessageToServerTaskAsync(message);
            }
        }
    }
}
