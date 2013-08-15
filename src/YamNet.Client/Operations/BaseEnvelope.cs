﻿// -----------------------------------------------------------------------
// <copyright file="BaseEnvelope.cs" company="YamNet">
//   Copyright (c) YamNet 2013 and Contributors
// </copyright>
// -----------------------------------------------------------------------

namespace YamNet.Client
{
    using System;

    /// <summary>
    /// The base envelope.
    /// </summary>
    /// <typeparam name="T">The class type.</typeparam>
    public class BaseEnvelope<T> : IBaseEnvelope<T>
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public T Content { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets a value indicating whether is faulted.
        /// </summary>
        public bool IsFaulted
        {
            get { return Exception != null; }
        }

        /// <summary>
        /// The result.
        /// </summary>
        /// <returns>The <see cref="T"/>.</returns>
        /// <exception cref="Exception">The exception.</exception>
        public T Result()
        {
            if (this.IsFaulted)
            {
                throw Exception;
            }

            return this.Content;
        }
    }
}