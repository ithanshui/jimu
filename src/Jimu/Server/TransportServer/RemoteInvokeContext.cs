﻿using System;
using System.Linq;

namespace Jimu.Server
{
    /// <summary>
    ///     context of service invoker
    /// </summary>
    public class RemoteCallerContext
    {
        public RemoteCallerContext(JimuTransportMsg transportMessage, IServiceEntryContainer serviceEntryContainer,
            IResponse response, ILogger logger)
        {
            Response = response;
            TransportMessage = transportMessage;
            try
            {
                RemoteInvokeMessage = transportMessage.GetContent<JimuRemoteCallData>();
            }
            catch (Exception ex)
            {
                logger.Error("failed to convert transportmsg.content to  JimuRemoteCallerData.", ex);
                return;
            }

            ServiceEntry = serviceEntryContainer.GetServiceEntry()
                .FirstOrDefault(x => x.Descriptor.Id == RemoteInvokeMessage.ServiceId);
            if (ServiceEntry == null)
            {
                logger.Warn($"not found service: {RemoteInvokeMessage.ServiceId}");
            }
        }

        public JimuServiceEntry ServiceEntry { get; }
        public IResponse Response { get; }

        public JimuTransportMsg TransportMessage { get; }

        public JimuRemoteCallData RemoteInvokeMessage { get; }
    }
}