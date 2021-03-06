﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.EntityModel
{
    using System;

    public sealed class ConsoleLogListener : ILoggerListener
    {
        public LogLevel LogLevelThreshold { get; set; }

        public void WriteLine(ILogItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var level = item.LogLevel;
            var message = item.Message;
            var phase = item.Phase;
            var file = item.File;
            var line = item.Line;
            if (level < LogLevelThreshold) return;
            var formatter = level + ": " + message;
            if (!string.IsNullOrEmpty(phase))
            {
                formatter += " in phase " + phase;
            }
            if (!string.IsNullOrEmpty(file))
            {
                formatter += " in file " + file;
                if (!string.IsNullOrEmpty(line)) formatter += " line " + line;
            }

            var foregroundColor = Console.ForegroundColor;
            try
            {
                ChangeConsoleColor(level);
                Console.WriteLine(formatter);
            }
            finally
            {
                Console.ForegroundColor = foregroundColor;
            }
        }

        public void Dispose()
        {
        }

        public void Flush()
        {
        }

        private void ChangeConsoleColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    throw new NotSupportedException(level.ToString());
            }
        }
    }
}
