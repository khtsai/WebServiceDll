using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CallWebService webService = new CallWebService();

            string xmlRequestContent = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tip=\"http://www.dsc.com.tw/tiptop/TIPTOPServiceGateWay\"><soapenv:Header/><soapenv:Body><tip:GetProductInfoRequest><tip:request>        &lt;Request>&lt;Access>&lt;Authentication user=\"tiptop\" password=\"tiptop\" />&lt;Connection application=\"PLM\" source=\"192.168.1.2\" />&lt;Organization name=\"YI\" />&lt;Locale language=\"zh_tw\" />&lt;/Access>&lt;RequestContent>&lt;Parameter>&lt;Record>&lt;Field name=\"sfb01\" value=\"xxx-00002\" />&lt;/Record>&lt;/Parameter> &lt;/RequestContent>&lt;/Request>       </tip:request>      </tip:GetProductInfoRequest>   </soapenv:Body></soapenv:Envelope>";
            //string xmlRequestContent = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tip=\"http://www.dsc.com.tw/tiptop/TIPTOPServiceGateWay\"><soapenv:Header/><soapenv:Body><tip:GetProductInfoRequest><tip:request>        <Request><Access><Authentication user=\"tiptop\" password=\"tiptop\" /><Connection application=\"PLM\" source=\"192.168.1.2\" /><Organization name=\"YI\" /><Locale language=\"zh_tw\" /></Access><RequestContent><Parameter><Record><Field name=\"sfb01\" value=\"xxx-00002\" /></Record></Parameter> </RequestContent></Request>       </tip:request>      </tip:GetProductInfoRequest>   </soapenv:Body></soapenv:Envelope>";

            string returnValue = webService.SendRequest("http://211.22.185.46:5280/web/ws/r/aws_ttsrv2_toptest", "", xmlRequestContent, escaped: false);

            Console.WriteLine(returnValue);
            Console.ReadLine();

        }
    }
}
