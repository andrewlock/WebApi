﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Formatter;
using Microsoft.OData;

namespace System.Web.OData.Batch
{
    /// <summary>
    /// Provides extension methods for the <see cref="HttpContent"/> class.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ODataHttpContentExtensions
    {
        /// <summary>
        /// Gets the <see cref="ODataMessageReader"/> for the <see cref="HttpContent"/> stream.
        /// </summary>
        /// <param name="requestContainer">The dependency injection container for the request.</param>
        /// <param name="content">The <see cref="HttpContent"/>.</param>
        /// <param name="settings">The <see cref="ODataMessageReaderSettings"/>.</param>
        /// <returns>A task object that produces an <see cref="ODataMessageReader"/> when completed.</returns>
        public static Task<ODataMessageReader> GetODataMessageReaderAsync(this HttpContent content,
            IServiceProvider requestContainer,
            ODataMessageReaderSettings settings)
        {
            return GetODataMessageReaderAsync(content, requestContainer, settings, CancellationToken.None);
        }

        /// <summary>
        /// Gets the <see cref="ODataMessageReader"/> for the <see cref="HttpContent"/> stream.
        /// </summary>
        /// <param name="requestContainer">The dependency injection container for the request.</param>
        /// <param name="content">The <see cref="HttpContent"/>.</param>
        /// <param name="settings">The <see cref="ODataMessageReaderSettings"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object that produces an <see cref="ODataMessageReader"/> when completed.</returns>
        public static async Task<ODataMessageReader> GetODataMessageReaderAsync(this HttpContent content,
            IServiceProvider requestContainer, ODataMessageReaderSettings settings, CancellationToken cancellationToken)
        {
            if (content == null)
            {
                throw Error.ArgumentNull("content");
            }

            cancellationToken.ThrowIfCancellationRequested();
            Stream contentStream = await content.ReadAsStreamAsync();

            IODataRequestMessage oDataRequestMessage = new ODataMessageWrapper(contentStream, content.Headers)
            {
                Container = requestContainer
            };
            ODataMessageReader oDataMessageReader = new ODataMessageReader(oDataRequestMessage, settings);
            return oDataMessageReader;
        }
    }
}