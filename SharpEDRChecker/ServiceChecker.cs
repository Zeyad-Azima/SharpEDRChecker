﻿using System;
using System.Management;

namespace SharpEDRChecker
{
    internal class ServiceChecker
    {
        internal static void CheckServices()
        {
            Console.WriteLine("[!] Checking Services...");
            var serviceList = new ManagementObjectSearcher("Select * From Win32_Service").Get();
            bool foundSuspiciousService = false;
            foreach (var service in serviceList)
            {
                var serviceName = service["Name"];
                var serviceDisplayName = service["DisplayName"];
                var serviceDescription = service["Description"];
                var serviceCaption = service["Caption"];
                var servicePathName = service["PathName"];
                var serviceState = service["State"];
                var servicePID = service["ProcessId"];

                var allattribs = $"{serviceName} - " +
                    $"{serviceDisplayName} - " +
                    $"{serviceDescription} - " +
                    $"{serviceCaption} - " +
                    $"{servicePathName}";

                foreach (var edrstring in EDRData.edrlist)
                {
                    if (allattribs.ToLower().Contains(edrstring.ToLower()))
                    {
                        Console.WriteLine($"[-] Suspicious service found:" +
                            $"\n \t Name: {serviceName}" + 
                            $"\n \t DisplayName: {serviceDisplayName}" +
                            $"\n \t Description: {serviceDescription}" +
                            $"\n \t Caption: {serviceCaption}" +
                            $"\n \t Binary: {servicePathName}" +
                            $"\n \t Status: {serviceState}" +
                            $"\n \t Process ID: {servicePID}");
                        Console.WriteLine($"[!] Matched on: {edrstring}\n");
                        foundSuspiciousService = true;
                    }
                }
            }
            if (!foundSuspiciousService)
            {
                Console.WriteLine("[+] No suspicious services found\n");
            }
        }
    }
}