using System;
using System.Threading;

namespace HorseThreadRace
{    
    class Program
    {
        static readonly object l = new object();
        static bool running = true;
        static int winner = 0;
        static int posFin = 100;

        static void race(object obj)
        {
            Horse h = (Horse)obj;
            Random n = new Random();
            
            while (running)
            {
                lock (l)
                {
                    if (running)
                    {
                        Console.SetCursorPosition(h.Posicion, h.Row);
                        Console.Write(" ");
                        h.Posicion += n.Next(2, 10);
                        Console.SetCursorPosition(h.Posicion, h.Row);
                        if (h.Posicion < posFin)
                        {
                            Console.Write(h.Dorsal);
                            Thread.Sleep(n.Next(200, 400));
                        }
                        else
                        {
                            running = false;
                            Console.Write(h.Dorsal + "**** WINNER");
                            winner = h.Dorsal;
                            Monitor.Pulse(l);
                        }
                    }                    
                }
            }
        }

        static void marks(int n, int bet)
        {            
            for(int i = 0; i < n; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("{0}|{1, "+(posFin-2)+"}|", i, " ");                
            }            

            Console.SetCursorPosition(0, n + 1);
            Console.Write("BET: " + bet);

            for(int j = 3; j > 0; j--)
            {
                Console.SetCursorPosition(0, n + 2);
                Console.Write(j);
                Thread.Sleep(1000);
            }
            Console.SetCursorPosition(0, n + 2);
            Console.Write(" ");
        }

        static int bet(int n)
        {
            bool error = true;
            int bet = 0;
            while (error)
            {
                error = false;
                Console.Write("HORSES  0 - "+(n-1)+"\nBET:  ");
                try
                {
                    bet = Convert.ToInt32(Console.ReadLine());
                    if(bet < 0 || bet > n-1)
                    {
                        throw new ArgumentException();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error. Enter a number.");
                    error = true;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Error. Value out of range.");
                    error = true;
                }
            }
            return bet;
        }

        static void Main(string[] args)
        {
            string resp = "";
            int n = 5;
            do
            {                                
                bool error = true;            
                int ap = bet(n);
                Console.Clear();
                marks(n, ap);

                Horse[] horses = new Horse[n];
                for (int i = 0; i < n; i++)
                {
                    horses[i] = new Horse(i, i);
                    Console.SetCursorPosition(0, i);
                    Console.Write(horses[i].Dorsal);
                }

                Thread[] hippodrome = new Thread[n];
                for (int i = 0; i < n; i++)
                {
                    hippodrome[i] = new Thread(race);
                    hippodrome[i].Start(horses[i]);
                }

                lock (l)
                {
                    while (running)
                    {
                        Monitor.Wait(l);
                    }
                }

                Console.SetCursorPosition(0, n+5);
                Console.WriteLine("THE WINNER IS: " + winner);
                if (ap == winner)
                {
                    Console.WriteLine("CONGRATULATIONS!!! You win the bet.");
                }
                else
                {
                    Console.WriteLine("SORRY... You lose the bet.");
                }
                Console.WriteLine("Do you want to play again? Y/N");
                error = true;
                while (error)
                {
                    error = false;
                    resp = Console.ReadLine().Trim().ToUpper();
                    if (resp.Equals("Y"))
                    {
                        Console.Clear();
                        running = true;
                    }else if (resp.Equals("N"))
                    {
                        Console.WriteLine("--- GAME OVER ---");
                    }
                    else
                    {
                        error = true;
                        Console.WriteLine("Error. Reply Y/N");
                    }
                }
            } while (resp.Equals("Y"));
        }               
    }
}
