using Launcher.Properties;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace KartRider
{
	public static class InitFont
	{
		[DllImport("gdi32.dll")]
		public static extern int AddFontResource(string lpFileName);

		[DllImport("gdi32.dll")]
		public static extern int RemoveFontResource(string lpFileName);

		public static void InstallFont()
		{
			string fontPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Microsoft\Windows\Fonts\gulim.ttc";
			byte[] fontData = Resources.gulim_ttc;
			if (!File.Exists(fontPath))
			{
				try
				{
					using (FileStream fileStream = new FileStream(fontPath, FileMode.Create))
					{
						fileStream.Write(fontData, 0, fontData.Length);
					}
					Console.WriteLine("字体数据已成功保存为字体文件。");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"保存字体文件时出现异常: {ex.Message}");
				}
			}
			try
			{
				using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", true))
				{
					if (key != null)
					{
						key.SetValue("Gulim & GulimChe & Dotum & DotumChe (TrueType)", fontPath);
						Console.WriteLine($"已在注册表中添加字体。");
					}
					else
					{
						Console.WriteLine("无法打开或创建字体相关的注册表键。");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"在注册表中添加字体信息时出现异常: {ex.Message}");
			}
			LoadFont(fontPath);
		}

		public static void LoadFont(string fontFilePath)
		{
			// 添加字体
			int result = AddFontResource(fontFilePath);
			if (result > 0)
			{
				Console.WriteLine("字体加载成功！");
			}
			else
			{
				Console.WriteLine("字体加载失败！");
			}
		}

		public static void UnloadFont(string fontFilePath)
		{
			// 移除字体
			int result = RemoveFontResource(fontFilePath);
			if (result > 0)
			{
				Console.WriteLine("字体卸载成功！");
			}
			else
			{
				Console.WriteLine("字体卸载失败！");
			}
		}
	}
}
