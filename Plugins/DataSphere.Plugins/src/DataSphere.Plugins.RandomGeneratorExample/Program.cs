﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataSphere.Plugins.RandomGeneratorExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Common.DataSphereClient client = new Common.DataSphereClient("127.0.0.1", 9001);
            double rightAscensionSpeed = 2;
            double declinationSpeed = 0.5f;
            double rightAscension = 0;
            double declination = -1;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task.Run(async () =>
            {
                while (true)
                {
                    var elapsed = stopwatch.Elapsed;
                    stopwatch.Restart();

                    rightAscension += elapsed.TotalSeconds*rightAscensionSpeed;
                    declination += elapsed.TotalSeconds*declinationSpeed;

                    while (rightAscension > 1)
                    {
                        rightAscension -= 1;
                    }
                    while (declination > 1)
                    {
                        declination -= 2;
                    }

                    var dataPoint = new DataPointViewModel()
                    {
                        color = new DataPointColor(),
                        rightAscension = (float) rightAscension,
                        declination = (float) declination
                    };
                    var message = JsonConvert.SerializeObject(dataPoint);
                    Console.WriteLine("Sending message {0}", message);
                    await client.SendMessageToServerTaskAsync(message);
                }
            }).Wait();
        }
    }
}
