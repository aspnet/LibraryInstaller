﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Web.LibraryManager.Contracts;

namespace Microsoft.Web.LibraryManager.Tools.Contracts
{
    internal class ConsoleLogger : ILogger, IInputReader
    {
        private object _syncObject = new object();

        private ConsoleLogger()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public static ConsoleLogger Instance { get; } = new ConsoleLogger();

        public string GetUserInput(string fieldName)
        {
            lock(_syncObject)
            {
                Console.Out.WriteLine($"{fieldName}: ");
                return Console.ReadLine();
            }
        }

        public void Log(string message, LogLevel level)
        {
            lock (_syncObject)
            {
                if (level == LogLevel.Error)
                {
                    if (Console.BackgroundColor != ConsoleColor.Red)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    
                    Console.Error.WriteLine(message);

                    Console.ResetColor();
                }
                else
                {
                    Console.Out.WriteLine(message);
                }
            }
        }
    }
}
