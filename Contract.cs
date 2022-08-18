using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetterConsole.Contract
{
    /// <summary>
    /// API for pipe IPC between BetterConsole and BetterConsole.Mod.
    /// </summary>
    public static class PipeContract
    {
        public const string PipeName = "BetterConsole.Pipe";
    }

    /// <summary>
    /// Struct with log message details. Used to serialize/deserialize JSON.
    /// </summary>
    public struct LogMessage
    {
        [JsonInclude]
        public bool Control;

        [JsonInclude]
        public string Timestamp;

        [JsonInclude]
        public string Severity;

        [JsonInclude]
        public string ChannelName;

        [JsonInclude]
        public List<string> Message;
    }
}

