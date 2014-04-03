﻿// -----------------------------------------------------------------------
// <copyright file="IMessageClient.cs" company="YamNet">
//   Copyright (c) 2014 YamNet contributors
// </copyright>
// -----------------------------------------------------------------------

namespace YamNet.Client
{
    using System.Linq;

    /// <summary>
    /// The MessageClient interface.
    /// </summary>
    public interface IMessageClient
    {
        /// <summary>
        /// Get all public messages in the User's network. 
        /// Corresponds to "All" conversations in the Yammer web interface.
        /// </summary>
        /// <param name="limit">The returned message limit.</param>
        /// <param name="trim">The returned message trim / limit options.</param>
        /// <param name="thread">The thread options.</param>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        IQueryable<Message> GetAll(int limit, MessageQueryTrim trim, MessageQueryThread thread);

        /// <summary>
        /// Get the User's feed based on the selection made between "Following" and "Top" conversation.
        /// </summary>
        /// <param name="limit">The returned message limit.</param>
        /// <param name="trim">The returned message trim / limit options.</param>
        /// <param name="thread">The thread options.</param>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        IQueryable<Message> GetFeed(int limit, MessageQueryTrim trim, MessageQueryThread thread);

        /// <summary>
        /// Get the algorithmic feed that corresponds to "Top" conversation.
        /// </summary>
        /// <param name="limit">The returned message limit.</param>
        /// <param name="trim">The returned message trim / limit options.</param>
        /// <param name="thread">The thread options.</param>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        IQueryable<Message> GetTop(int limit, MessageQueryTrim trim, MessageQueryThread thread);

        /// <summary>
        /// Get the "Following" feed which is conversations involving people, groups and topics that the user is following.
        /// </summary>
        /// <param name="limit">The returned message limit.</param>
        /// <param name="trim">The returned message trim / limit options.</param>
        /// <param name="thread">The thread options.</param>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        IQueryable<Message> GetFollowing(int limit, MessageQueryTrim trim, MessageQueryThread thread);

        /// <summary>
        /// Get all messages sent by the User. 
        /// </summary>
        /// <param name="limit">The returned message limit.</param>
        /// <param name="trim">The returned message trim / limit options.</param>
        /// <param name="thread">The thread options.</param>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        IQueryable<Message> GetSent(int limit, MessageQueryTrim trim, MessageQueryThread thread);

        /// <summary>
        /// Get all private messages received by the User.
        /// </summary>
        /// <param name="limit">The returned message limit.</param>
        /// <param name="trim">The returned message trim / limit options.</param>
        /// <param name="thread">The thread options.</param>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        IQueryable<Message> GetPrivate(int limit, MessageQueryTrim trim, MessageQueryThread thread);

        /// <summary>
        /// Get all messages received by the User.
        /// </summary>
        /// <param name="limit">The returned message limit.</param>
        /// <param name="trim">The returned message trim / limit options.</param>
        /// <param name="thread">The thread options.</param>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        IQueryable<Message> GetReceived(int limit, MessageQueryTrim trim, MessageQueryThread thread);

        /// <summary>
        /// Marks the specified message as liked by the current user.
        /// </summary>
        /// <param name="id">The message id to like.</param>
        void LikeById(long id);

        /// <summary>
        /// Removes the like mark from the specified message.
        /// </summary>
        /// <param name="id">The message id to unlike.</param>
        void UnlikeById(long id);
        
        /// <summary>
        /// Remove a message by its id. The message must be owned by the current user.
        /// </summary>
        /// <param name="id">The message id.</param>
        void DeleteById(long id);
    }
}
