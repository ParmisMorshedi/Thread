using System.Threading;
using System.Collections.Concurrent;
using System.Timers;
using System.Diagnostics;


namespace ConsoleApp1
{
    internal class Program
    {
        //Initializes the variable to detremine the outcome of the race.
        static int win = 1;

        //To facilitate cancelation of threads.
        static CancellationTokenSource cts = new CancellationTokenSource();
        
        static void Main(string[] args)
        {
            //To create instances of the Car class.
            Car Car1 = new Car("car1");
            Car Car2 = new Car("car2");

            Thread thread1 = new Thread(() => CarEvent(Car1,cts.Token));
            Thread thread2 = new Thread(() => CarEvent(Car2, cts.Token));
            
            Console.WriteLine("The competition has started!");

            //Both cars starts at a time
            thread1.Start();
            thread2.Start();

            while (true)
            {
                Console.ReadLine();

                Console.WriteLine($"{Car1.Name} with {Car1.Speed} km/h has diriven {Car1.Distance} km");

                Console.WriteLine($"{Car2.Name} with {Car2.Speed} km/h has diriven {Car2.Distance} km");
                

                Thread.Sleep(100);

                if (Car1.Distance > 10 && Car2.Distance > 10)
                {
                    Console.WriteLine("Both cars have finished the race.");

                    //signal cancelation to threads
                    cts.Cancel();

                    //To wait for threads to finish befor exiting
                    thread1.Join();
                    thread2.Join();
                }

            }

        }
            
            //Metod to handel carevents
            static void CarEvent(Car car, CancellationToken cancellationToken)
            {       
                    //Recording the intitial second for time-based events
                    int second = DateTime.Now.Second;

                    Console.WriteLine($" Second {second}  views that {car.Name} has started");
                    
                    Console.WriteLine($"{car.Name} starts");
                    
                    //Cars drive less than 10km.
                    while (car.Distance < 10 && !cancellationToken.IsCancellationRequested)
                    {   
                          //The events must not occur immediately after the start of the race.
                           Thread.Sleep(1000);

                        // Checking for time-based events every 30 seconds.
                        if ((DateTime.Now.Second-second) % 30 == 0 )
                        {
                             Random random = new Random();

                            double randomEvent = random.NextDouble();

                            if (randomEvent <= 0.02) //probability 1/50
                            {
                                Console.WriteLine($"{car.Name} needs to refuel. The car stops 30 seconds");
                                Thread.Sleep(30000);
                            }
                            else if (randomEvent <= 0.04) //probability 2/50
                            {
                                Console.WriteLine($"{car.Name} needs tire change. The car stops 20 seconds");
                                Thread.Sleep(20000);

                            }
                            else if (randomEvent <= 0.1) //probability 5/50
                            {
                                Console.WriteLine($"{car.Name} needs to wash windshield. The car stops 10 seconds");
                                Thread.Sleep(10000);

                            }
                            else if (randomEvent <= 0.2) //probability 10/50
                            {
                                Console.WriteLine($" The speed of the {car.Name} is reduced by 1km/h ");

                                car.Speed --; //The speed of the car is reduced by 1km/h
                           
                            }

                        }
                        //Updating the distance traveled by the car
                        car.Distance += car.Speed * 1.0/ 3600; 

                    }
                    // Displaying the outcome of the race based on the flag
                    if ( win==1  ) 
                    {
                        Console.WriteLine($"{car.Name} has arrived and wins");
                        win = 0;
                    }
                    else  
                    {
                        Console.WriteLine($"{car.Name} has arrived too");
                        Console.WriteLine("The race is over");

                        ////To exit when threads and program are finished 
                        Environment.Exit(0);



                    }
               
            }


    }
    
}
    