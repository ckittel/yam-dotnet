﻿// -----------------------------------------------------------------------
// <copyright file="ISerializer.cs" company="YamNet">
//   Copyright (c) YamNet 2013 and Contributors
// </copyright>
// -----------------------------------------------------------------------

namespace YamNet.Client
{
    /// <summary>
    /// The Serializer interface.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Gets the content type.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// The serialize to byte array.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        byte[] SerializeToByteArray(object obj);

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string Serialize(object obj);
    }
}