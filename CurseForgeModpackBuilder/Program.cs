using CurseForgeModpackBuilder.Models;
using CurseForgeModpackBuilder.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace CurseForgeModpackBuilder
{
	class Program
	{
		static ManifestModel manifest;
		const string pathMods = @"./mods";
		static void Main(string[] args)
		{
			var manifestPath = args[0];
			var manifestRaw = File.ReadAllText(manifestPath);
			manifest = JsonConvert.DeserializeObject<ManifestModel>(manifestRaw);
			if (!Directory.Exists(pathMods))
				Directory.CreateDirectory(pathMods);
			DownloadMods();
		}

		private static void DownloadMods()
		{
			try
			{
				Console.WriteLine($"Total mods count: {manifest.files.Count}");
				var errorsUrls = new List<string>();

				foreach (var fileInfo in manifest.files)
				{
					var url = $"https://www.curseforge.com/api/v1/mods/{fileInfo.projectID}/files/{fileInfo.fileID}/download";

					if (!DownloadFile(url))
					{
						errorsUrls.Add(url);
					}
					
					
				}

				Console.WriteLine("Done.\n");
				if (errorsUrls.Count > 0)
				{
					Console.WriteLine("Cannot download these files: ");

					foreach (var errorUrl in errorsUrls)
					{
						Console.WriteLine(errorUrl);
					}
				}
			}
			catch (Exception e)
			{

			}
		}

		private static void AddHeaders(WebRequest wb)
		{
			wb.Headers.Add("accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
			wb.Headers.Add("accept-encoding: gzip, deflate, br");
			wb.Headers.Add("accept-language: en-US,en;q=0.9,ru;q=0.8");
			wb.Headers.Add("dnt: 1");
			wb.Headers.Add("sec-ch-ua: \"Not.A / Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Opera GX\";v=\"100\"");
			wb.Headers.Add("sec-ch-ua-mobile: ?0");
			wb.Headers.Add("sec-ch-ua-platform: \"Windows\"");
			wb.Headers.Add("sec-fetch-dest: document");
			wb.Headers.Add("sec-fetch-mode: navigate");
			wb.Headers.Add("sec-fetch-site: same-origin");
			wb.Headers.Add("sec-fetch-user: ?1");
			wb.Headers.Add("upgrade-insecure-requests: 1");
			wb.Headers.Add("user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 OPR/100.0.0.0");
			//need to add your cookies files, take it when you downloading file direct from curseforge
		}

		public static bool DownloadFile(String remoteFilename)
		{
			int bytesProcessed = 0;
			Stream remoteStream = null;
			Stream localStream = null;
			WebResponse responseFileName = null;
			WebResponse response = null;
			try
			{
				WebRequest requestFileName = WebRequest.Create(remoteFilename);
				AddHeaders(requestFileName);
				responseFileName = requestFileName.GetResponse();
				if (responseFileName != null)
				{
					WebRequest request = WebRequest.Create(responseFileName.ResponseUri);
					AddHeaders(request);
					response = request.GetResponse();
					if (response != null)
					{
						var urlPaths = responseFileName.ResponseUri.ToString().Split('/');
						string fileName = urlPaths[urlPaths.Length - 1];
						int length = Convert.ToInt32(responseFileName.Headers["Content-Length"]);

						Console.Write($"{fileName}: ");
						var progressBar = new ProgressBar();

						remoteStream = response.GetResponseStream();
						localStream = File.Create(pathMods + "/" + fileName);

						byte[] buffer = new byte[1024];
						int bytesRead;

						do
						{
							bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
							localStream.Write(buffer, 0, bytesRead);
							bytesProcessed += bytesRead;
							progressBar.Report((double)bytesProcessed / length);
						} while (bytesRead > 0);
						progressBar.Report(1);
						progressBar.Dispose();
						Console.Write($"Done.\n");

					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
			finally
			{
				if (responseFileName != null) responseFileName.Close();
				if (remoteStream != null) remoteStream.Close();
				if (localStream != null) localStream.Close();
			}
			return true;
		}
	}
}
