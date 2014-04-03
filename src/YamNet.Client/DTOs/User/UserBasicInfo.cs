﻿// -----------------------------------------------------------------------
// <copyright file="UserBasicInfo.cs" company="YamNet">
//   Copyright (c) 2013 YamNet contributors
// </copyright>
// -----------------------------------------------------------------------

namespace YamNet.Client
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// The user.
    /// </summary>
    public class UserBasicInfo //: IAutoCompleteValueEx
    {
        #region System
        /// <summary>
        /// Gets or sets the account activation date + time.
        /// </summary>
        [JsonProperty("activated_at")]
        public DateTimeOffset? ActivatedAt { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the account state / status.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; } // TODO: Map to an enumeration

        /// <summary>
        /// Gets or sets the account type.
        /// </summary>
        /// <remarks>
        /// Used in User REST API with include_followed_users parameter.
        /// </remarks>
        [JsonProperty("type")]
        public string Type { get; set; } // TODO: Map to an enumeration
        #endregion

        #region Personal Details
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the mugshot url.
        /// </summary>
        [JsonProperty("mugshot_url")]
        public Uri MugshotUrl { get; set; }

        /// <summary>
        /// Gets or sets the mugshot url template.
        /// </summary>
        [JsonProperty("mugshot_url_template")]
        public string MugshotUrlTemplate { get; set; }

        /// <summary>
        /// Gets or sets the web url.
        /// </summary>
        [JsonProperty("web_url")]
        public Uri WebUrl { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }
        #endregion

        #region Work, School, and Network
        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        [JsonProperty("job_title")]
        public string JobTitle { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the user statistics.
        /// </summary>
        [JsonProperty("stats")]
        public UserStat Stats { get; set; }
    }
}