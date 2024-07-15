using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PCSetter
{
    internal class Program
    {
       public static string VariablesRoot = "\\Variables\\";
        static void Main(string[] args)
        {
            //pobierz wartosci
            string PCIndex = ReadValOrCreate("PCIndex");
            string KlasaNr = ReadValOrCreate("KlasaNr");
            string urzytkownik = ReadValOrCreate("urzytkownik");
            string haslo = ReadValOrCreate("haslo");


            string NowaNazwakomputera = "";
            if(PCIndex == "0")
            {
                NowaNazwakomputera = $"SALA-{KlasaNr}-KN";
            }else
            {
                NowaNazwakomputera = $"SALA-{KlasaNr}-{PCIndex}";
            }

            //nowi urzytkownicy

            string urzytkownikASI = "aadf"; //ReadValOrCreate("urzytkownikASI");
            string hasloASI = "aadf"; //ReadValOrCreate("hasloASI");

            NoweKonto(urzytkownikASI, hasloASI);

            DodajDoAdministratorow(urzytkownikASI);

            ZmieniNazwe(NowaNazwakomputera);
            Console.ReadLine();
        }

        public static string ReadVal(string val) {

            string workingDirectory = Environment.CurrentDirectory;
            string filePath = workingDirectory + VariablesRoot + val + ".txt";
            Console.WriteLine(filePath);
            if (File.Exists(filePath)) {
                string[] lines = File.ReadAllLines(filePath);


                return lines[0];
            }

            return "NULL";
        }
        public static void SETVal(string valName, string val)
        {

            string workingDirectory = Environment.CurrentDirectory;
            string filePath = workingDirectory + VariablesRoot + valName + ".txt";
            Console.WriteLine(filePath);
            StreamWriter sw = File.CreateText(filePath);
            sw.Flush();
            sw.Close();
        }

        public static string ReadValOrCreate(string val)
        {

            string workingDirectory = Environment.CurrentDirectory;
            string filePath = workingDirectory + VariablesRoot + val + ".txt";
            Console.WriteLine(filePath);
            Console.WriteLine($"1 : {File.Exists(filePath)}");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);


                return lines[0];
            }
            else { 
                StreamWriter wr = File.CreateText(filePath);
                wr.Write(0);
                wr.Flush();
                wr.Close();
                return "0";
            }

            return "NULL";
        }


        public static void ZmieniNazwe(string nazwa)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"wmic computersystem where \"caption='%computername%'\" rename \"{nazwa}\"";
            //startInfo.Verb = "runas";

            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;

            process.OutputDataReceived += (sender, argss) => Console.WriteLine(argss.Data);
            process.ErrorDataReceived += (sender, argss) => Console.WriteLine(argss.Data);

            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.StandardInput.WriteLine($"wmic computersystem where \"caption='%computername%'\" rename \"{nazwa}\"");

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            
            // until we are done
            process.WaitForExit();
            Console.WriteLine("EXITED");
        }

        public static void NoweKonto(string nazwa, string haslo)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "net.exe";
            startInfo.Arguments = $"user {nazwa} {haslo} /add /active:yes /expires:never /passwordchg:no ";
            startInfo.Verb = "runas";

            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.OutputDataReceived += (sender, argss) => Console.WriteLine(argss.Data);
            process.ErrorDataReceived += (sender, argss) => Console.WriteLine(argss.Data);

            process.StartInfo.UseShellExecute = false;
            process.Start();


            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // until we are done
            process.WaitForExit();
            Console.WriteLine("EXITED");
        }

        public static void DodajDoAdministratorow(string nazwa)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "net.exe";
            startInfo.Arguments = $"localgroup Administratorzy {nazwa} /add";
            startInfo.Verb = "runas";

            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.OutputDataReceived += (sender, argss) => Console.WriteLine(argss.Data);
            process.ErrorDataReceived += (sender, argss) => Console.WriteLine(argss.Data);

            process.StartInfo.UseShellExecute = false;
            process.Start();


            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // until we are done
            
            Console.WriteLine("EXITED");
        }
    }
}
