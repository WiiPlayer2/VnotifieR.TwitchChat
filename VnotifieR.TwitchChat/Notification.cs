using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VnotifieR.TwitchChat
{
    class Notification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
