﻿// -----------------------------------------------------------------------
// <copyright file="MessageEnvelope.cs" company="YamNet">
//   Copyright (c) YamNet 2013 and Contributors
// </copyright>
// -----------------------------------------------------------------------

namespace YamNet.Client
{
    using Newtonsoft.Json;

    /// <summary>
    /// Message API response that contains a list of messages (thread starters),
    /// a list of references, meta object (perspective data), and, optionally,
    /// a collection of messages for each thread.
    /// </summary>
    public sealed class MessageEnvelope
    {
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>The messages.</value>
        [JsonProperty("messages")]
        public Message[] Messages { get; set; }

//        /// <summary>
//        /// Gets or sets the references.
//        /// </summary>
//        /// <value>The references.</value>
//        [JsonProperty("references")]
//        [JsonConverter(typeof(ReferenceJsonConverter))]
//        public IReference[] References { get; set; }

//        /// <summary>
//        /// Gets or sets the meta.
//        /// </summary>
//        /// <value>The meta.</value>
//        [JsonProperty("meta")]
//        public Meta Meta { get; set; }

//        /// <summary>
//        /// Gets or sets the thread messages.
//        /// </summary>
//        /// <value>The thread messages.</value>
//        [JsonProperty("threaded_extended")]
//        public ThreadMessagesDictionary ThreadMessages { get; set; }
    }
}
