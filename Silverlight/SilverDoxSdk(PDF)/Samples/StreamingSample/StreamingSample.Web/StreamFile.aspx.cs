using System;
using System.IO;
using System.Diagnostics;

namespace StreamingServer
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // we are sending binary data, not HTML/CSS, so clear the page headers
            Response.Clear();
            Response.ContentType = "Application/xod";

            // note that user input should be filtered for security reasons
            string filePath = Server.MapPath(".") + "\\ClientBin\\newsletter.xod";
            //string filePath = Server.MapPath(".") + "\\Windows_8.pdf";

            // Opens a stream from a  .xod file on server.
            // Using PDFNet, an original file (e.g. a pdf file) could be converted on the fly and
            // streamed directly to the client. See the PDFNet sample "SilverDoxStreaming".
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                // send data 30 KB at a time
                Byte[] t = new Byte[30 * 1024];

                int bytesRead = 0;
                bytesRead = fs.Read(t, 0, t.Length);
                Response.BufferOutput = false;

                int totalBytesSent = 0;

                Debug.WriteLine("Commence streaming...");
                while (bytesRead > 0)
                {
                    // write bytes to the response stream
                    Response.BinaryWrite(t);

                    // write to output how many bytes have been sent
                    totalBytesSent += bytesRead;
                    Debug.WriteLine("Server sent total " + totalBytesSent + " bytes.");

                    // read next bytes
                    bytesRead = fs.Read(t, 0, t.Length);
                }
            }

            Debug.WriteLine("Done.");

            // ensure all bytes have been sent and stop execution
            Response.End();
        }
    }
}