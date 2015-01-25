using MyCode.Converter;
using MyCode.Utils;
using System.Globalization;
using System.IO;
using System.ServiceModel.Web;

namespace MyCode.Download
{
	public class ContentDownload
	{
		public static Stream Download(string fileName, Stream fileStream)
		{
			if(WebOperationContext.Current != null)
			{
				var outgoingResponse = WebOperationContext.Current.OutgoingResponse;

				SetCurrentBrowserStyle();

				outgoingResponse.ContentType = "application/octet-stream";

				var encodefileName = GetFileName(fileName);
				var contentDispositionStringFormater = GetContentDispositionStringFormater();

				outgoingResponse.Headers.Add("Content-Disposition", string.Format(CultureInfo.InvariantCulture, contentDispositionStringFormater, encodefileName));
			}
			return fileStream;
		}

		enum BrowserStyles
		{
			MSIE = 0,
			Chrome = 1,
			Firefox = 2
		}

		private static BrowserStyles CurrentBrowserStyle { get; set; }

		private static void SetCurrentBrowserStyle()
		{
			var browsers = EnumUtils.GetValues<BrowserStyles>();
			var incomingRequest = WebOperationContext.Current.IncomingRequest;
			foreach (var browser in browsers)
			{
				if (incomingRequest.UserAgent.IndexOf(browser.ToString()) >= 0)
				{
					CurrentBrowserStyle = browser;
					break;
				}
			}
		}

		private static string GetFileName(string fileName)
		{
			string returnFileName = null;
			switch (CurrentBrowserStyle)
			{
				case BrowserStyles.MSIE:
				case BrowserStyles.Chrome:
				case BrowserStyles.Firefox:
					{
						returnFileName = ASCIIConverter.ToHexString(fileName);
					}
					break;
				default:
					{
						returnFileName = fileName;
					}
					break;
			}

			return returnFileName;
		}

		private static string GetContentDispositionStringFormater()
		{
			string stringFormater = null;
			switch (CurrentBrowserStyle)
			{
				case BrowserStyles.Firefox:
					{
						stringFormater = "attachment;filename*=utf-8'" + CultureInfo.CurrentCulture.ToString() + "'\"{0}\"";
					}
					break;
				case BrowserStyles.MSIE:
				case BrowserStyles.Chrome:
				default:
					{
						stringFormater = "attachment; filename=\"{0}\"";
					}
					break;
			}
			return stringFormater;
		}
	}
}
