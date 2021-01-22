﻿using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace WebService
{
    public class CallWebService
    {
        public string SendRequest(string inputUrl, string actionUrl, string xmlDoc, string returnStyle = "XML", string username = "", string password = "", string host = "" , string option = "DEFAULT")
        {
            var _url = inputUrl;
            var _action = actionUrl;

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(xmlDoc);
            HttpWebRequest webRequest = CreateWebRequest(_url, _action, username, password, option, host);

            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(soapResult);
                string soapResponse = "";
                if (returnStyle == "XML")
                {
                    soapResponse = xmldoc.InnerXml;
                }
                else
                {
                    soapResponse = xmldoc.InnerText;
                }

                return soapResponse;
            }
            /*using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                    Console.WriteLine(soapResult);
                }
                return soapResult;
            }*/
        }

        private HttpWebRequest CreateWebRequest(string url, string action, string username, string password, string option, string host)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            
            string encoded = "";
            if (username != "" && password != "")
            {
                if (option != "UTF8")
                {
                    encoded = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(username + ":" + password));
                }
                else
                {
                    encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(username + ":" + password));
                }
                
                webRequest.Headers["Authorization"] = string.Format("Basic {0}", encoded);
            }

            if (host != "")
            {
                webRequest.Headers["Host"] = host;
            }

            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private XmlDocument CreateSoapEnvelope(string xmlDoc)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(xmlDoc);
            return soapEnvelopeDocument;
        }

        private void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}
