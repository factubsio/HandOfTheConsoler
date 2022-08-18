using BetterConsole.Contract;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HandOfTheConsoler
{
    internal class IPC
    {
        private readonly NamedPipeServerStream pipe;
        private readonly MemoryPool<byte> pool;
        private readonly BinaryReader reader;

        private static IPC? _Instance;
        public static IPC Instance => _Instance ??= new();
        public IPC()
        {
            pipe = new(PipeContract.PipeName, PipeDirection.In, 2, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            pool = MemoryPool<byte>.Shared;
            reader = new(pipe);

        }

        public async Task Connect()
        {
            await pipe.WaitForConnectionAsync();
        }

        public void ConsumeAll(Action<LogMessage> callback)
        {
            Task.Run(() =>
            {
                while (pipe.IsConnected)
                {
                    try
                    {
                        var raw = reader.ReadString();
                        callback(JsonSerializer.Deserialize<LogMessage>(raw));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });

        }

    }
}
