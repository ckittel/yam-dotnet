﻿// -----------------------------------------------------------------------
// <copyright file="GroupClient.cs" company="YamNet">
//   Copyright (c) 2013 YamNet contributors
// </copyright>
// -----------------------------------------------------------------------

namespace YamNet.Client
{
    using System.Threading.Tasks;

    /// <summary>
    /// Yammer group membership client.
    /// </summary>
    /// <remarks>
    /// REST API documentation: https://developer.yammer.com/restapi/#rest-groups
    /// </remarks>
    public class GroupClient : ClientBase, IGroupClient
    {
        /// <summary>
        /// The group membership client base uri.
        /// </summary>
        private const string BaseUri = "/group_memberships";

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupClient"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public GroupClient(Client client)
            : base(client)
        {
        }

        /// <summary>
        /// Join the group specified by the numeric Id.
        /// </summary>
        /// <param name="id">The group id.</param>
        public async Task JoinById(long id)
        {
            var url = this.GetFinalUrl(string.Format("{0}.json?group_id={1}", BaseUri, id));

            await this.Client.PostAsync(url);
        }

        /// <summary>
        /// Leave the group specified by the numeric Id.
        /// </summary>
        /// <param name="id">The group id.</param>
        public async Task LeaveById(long id)
        {
            var url = this.GetFinalUrl(string.Format("{0}.json?group_id={1}", BaseUri, id));

            await this.Client.DeleteAsync(url);
        }
    }
}