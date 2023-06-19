using System;
using System.IO;
using System.Numerics;

namespace MassReplacer
{
    internal class Program
    {
        const string ver = "3";
        const string splash = "Mass Replacer V" + ver + " --> Made By [tyuXX]";
        const string logfile = "mrvd" + ver + ".log";
        const string debuglogfile = "mrvdebug" + ver + ".log";
        static BigInteger In100( BigInteger n, BigInteger all )
        {
            if (n < 1) { return 0; }
            if (n > all) { return 100; }
            return (n * 100) / all;
        }
        static ConsoleColor StabilityColoring( BigInteger s )
        {
            if (s > 95) { return ConsoleColor.Green; }
            if (s > 80) { return ConsoleColor.DarkGreen; }
            if (s > 65) { return ConsoleColor.Yellow; }
            if (s > 50) { return ConsoleColor.Red; }
            if (s > 40) { return ConsoleColor.DarkRed; }
            if (s > 25) { return ConsoleColor.Gray; }
            return ConsoleColor.White;
        }
        static void Main( string[] args )
        {
            Console.Title = splash + " State:Startup";
            File.Create( logfile );
            while (true)
            {
                Console.WriteLine( Console.Title );
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Clear();
                BigInteger Files = 0;
                BigInteger PFiles = 0;
                BigInteger Dirs = 0;
                BigInteger PDirs = 0;
                BigInteger Err = 0;
                BigInteger Stability = 0;
                Console.Title = splash + " State:Input";
                Console.Write( "To Get Replaced:" );
                string tor = Console.ReadLine();
                Console.Write( "To Place:" );
                string rep = Console.ReadLine();
                Console.Write( "Directory:" );
                string path = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Title = splash + " State:Processing...";
                if (Directory.Exists( path ))
                {
                    foreach (string tmp in Directory.GetFiles( path, "*", SearchOption.AllDirectories ))
                    {
                        try
                        {
                            File.WriteAllText( tmp, File.ReadAllText( tmp ).Replace( tor, rep ) );
                            FileInfo fileInfo = new FileInfo( tmp );
                            if (fileInfo.Name.Contains( tor )) { File.Move( tmp, fileInfo.Directory.FullName + @"\" + fileInfo.Name.Replace( tor, rep ) ); Files++; }
                        }
                        catch (Exception e) { File.AppendAllText( logfile, e.Message ); File.AppendAllText( debuglogfile, e.StackTrace ); Err++; }
                        PFiles++;
                    }
                    foreach (string tmp in Directory.GetDirectories( path, "*", SearchOption.AllDirectories ))
                    {
                        try
                        {
                            DirectoryInfo directoryInfo = new DirectoryInfo( tmp );
                            if (directoryInfo.Name.Contains( tor )) { Directory.Move( tmp, directoryInfo.Parent.FullName + @"\" + directoryInfo.Name.Replace( tor, rep ) ); Dirs++; }
                        }
                        catch (Exception e) { File.AppendAllText( logfile, e.Message ); File.AppendAllText( debuglogfile, e.StackTrace ); Err++; }
                        PDirs++;
                    }
                    Stability = (100 - In100( Err, PDirs + PFiles ));
                }
                Console.Clear();
                Console.ForegroundColor = StabilityColoring( Stability );
                Console.WriteLine( Files + "<-- Files Changed" );
                Console.WriteLine( PFiles + "<-- Files Processed" );
                Console.WriteLine( Dirs + "<-- Directories Changed" );
                Console.WriteLine( PDirs + "<-- Directories Processed" );
                Console.WriteLine( Err + "<-- Errors Occured" );
                Console.WriteLine( "Stability: " + Stability + "%" );
                Console.WriteLine( "Done! Press any key to continue." );
                Console.Title = splash + " State:Idle";
                Console.ReadKey();
            }
        }
    }
}
