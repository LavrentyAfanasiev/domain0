﻿using Monik.Client;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Sdl.Topshelf.Nancy
{
    public static class CommandHelper
    {
        private const string Netsh = "netsh";

        public static bool AddFirewallRule(string rule, params int[] ports)
            => RunElevated(Netsh,
                $"advfirewall firewall add rule name=\"{rule}\" dir=in protocol=TCP localport=\"{string.Join(",", ports)}\" action=allow");

        public static bool RemoveFirewallRule(string rule)
            => RunElevated(Netsh,
                $"netsh advfirewall firewall delete rule name=\"{rule}\" dir=in");

        public static bool RemoveUrlReservation(Uri uri)
            => RunElevated(Netsh, 
                $"http delete urlacl url={uri.Scheme}://{uri.Host}:{uri.Port}/".Replace("localhost", "+"));

        public static bool AddUrlReservation(Uri uri, string user)
            => RunElevated(Netsh,
                $"http add urlacl url={uri.Scheme}://{uri.Host}:{uri.Port}/ user=\"{user}\"".Replace("localhost", "+"));

        public static bool AddSslCertificate(Uri uri, X509Certificate2 cert)
            => RunElevated(Netsh,
                $"http add sslcert ipport=0.0.0.0:{uri.Port} certhash={Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(cert.Thumbprint))} appid={{{Guid.NewGuid()}}}");

        public static bool RemoveSslCertificate(Uri uri)
            => RunElevated(Netsh,
                $"http delete sslcert ipport=0.0.0.0:{uri.Port}");

        public static bool RunElevated(string file, string args)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Verb = "runas",
                    Arguments = args,
                    FileName = file,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            process.WaitForExit();
            var output = process.StandardOutput.ReadToEnd();
            if (process.ExitCode == 0)
                return true;

            M.ApplicationError($"{args}-{output}");
            return false;
        }
    }
}
