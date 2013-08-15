﻿// -----------------------------------------------------------------------
// <copyright file="JsonDotNetSerializer.cs" company="YamNet">
//   Copyright (c) YamNet 2013 and Contributors
// </copyright>
// -----------------------------------------------------------------------

namespace YamNet.Client
{
    using System.Collections.Generic;
    using System.Text;

    using Newtonsoft.Json;

    /// <summary>
    /// The JSON.Net serializer.
    /// </summary>
    public class JsonDotNetSerializer : ISerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDotNetSerializer" /> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="converters">The converters.</param>
        public JsonDotNetSerializer(Encoding encoding = null, params JsonConverter[] converters)
        {
            this.TextEncoding = encoding ?? Encoding.UTF8;
            this.Converters = converters;
            this.Settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Converters = this.Converters
            };
        }

        /// <summary>
        /// Gets or sets the text encoding.
        /// </summary>
        /// <value>The text encoding.</value>
        public Encoding TextEncoding { get; protected set; }

        /// <summary>
        /// Gets or sets the converters.
        /// </summary>
        /// <value>The converters.</value>
        public IList<JsonConverter> Converters { get; protected set; }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType
        {
            get { return "application/json"; }
        }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        protected JsonSerializerSettings Settings { get; set; }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The result as byte[].</returns>
        public byte[] SerializeToByteArray(object obj)
        {
            return this.TextEncoding.GetBytes(this.Serialize(obj));
        }

        /// <summary>
        /// Serializes the specified <see cref="object"/>
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to serialize</param>
        /// <returns>The result as string.</returns>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, this.Settings);
        }
    }
}