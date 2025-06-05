using System;
using System.Net;
using System.Net.Sockets;

namespace CSM.Util
{
    class CSMWebClient : WebClient
    {
        private WebRequest _request = null;

        protected override WebRequest GetWebRequest(Uri address)
        {
            this._request = base.GetWebRequest(address);

            if (this._request is HttpWebRequest webRequest)
            {
                webRequest.ServicePoint.BindIPEndPointDelegate = (servicePoint, remoteEndPoint, retryCount) =>
                {
                    if (remoteEndPoint.AddressFamily == AddressFamily.InterNetworkV6 && Socket.OSSupportsIPv6)
                    {
                        return new IPEndPoint(IPAddress.IPv6Any, 0);
                    }

                    return new IPEndPoint(IPAddress.Any, 0);
                };

                webRequest.AllowAutoRedirect = false;
            }

            return this._request;
        }

        public HttpStatusCode StatusCode()
        {
            HttpStatusCode result;

            if (this._request == null)
            {
                throw (new InvalidOperationException("Unable to retrieve the status code, maybe you haven't made a request yet."));
            }

            if (base.GetWebResponse(this._request) is HttpWebResponse response)
            {
                result = response.StatusCode;
            }
            else
            {
                throw (new InvalidOperationException("Unable to retrieve the status code, maybe you haven't made a request yet."));
            }

            return result;
        }
    }
}
