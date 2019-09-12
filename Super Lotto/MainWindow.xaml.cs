using Microsoft.Win32;
using Super_Lotto.DataSources;
using Super_Lotto.DataSources.Entities;
using Super_Lotto.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Super_Lotto
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		private Random randomField;
		public Random Random
		{
			get
			{
				if (randomField == null)
				{
					randomField = new Random();
				}
				return randomField;
			}
		}

		private DataProvider providerField;
		public DataProvider Provider
		{
			get
			{
				if (providerField == null)
				{
					providerField = new DataProvider();
				}
				return providerField;
			}
		}
		public MainWindowViewModel ViewModel
		{
			get
			{
				return this.DataContext as MainWindowViewModel;
			}
			set
			{
				this.DataContext = value;
			}
		}
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			int issueCount = int.Parse(button.Content.ToString());

			if (ViewModel == null)
			{
				ViewModel = new MainWindowViewModel();
			}
			ViewModel.Historys = Provider.GetHistoryData(issueCount);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Cursor = Cursors.Wait;
			if (ViewModel == null)
			{
				ViewModel = new MainWindowViewModel();
			}
			Dictionary<int, int> FrontAreaCountDict = new Dictionary<int, int>();
			Dictionary<int, int> BackAreaCountDict = new Dictionary<int, int>();
			IList<string> ForecastResult = new List<string>();
			if (ViewModel.CalculationCount > 0 && ViewModel.ForecastCount > 0)
			{
				IList<int> FrontArea = new List<int>();
				IList<int> BackArea = new List<int>();
				IList<HistoryData> historyDatas = Provider.GetHistoryData(ViewModel.CalculationCount);
				foreach (var historyData in historyDatas)
				{
					for (int i = 0; i < historyData.Numbers.Count(); i++)
					{
						if (i < 5)
						{


							if (FrontAreaCountDict.ContainsKey(historyData.Numbers[i]))
							{
								FrontAreaCountDict[historyData.Numbers[i]]++;
							}
							else
							{
								FrontAreaCountDict[historyData.Numbers[i]] = 1;
							}

						}
						else
						{

							if (BackAreaCountDict.ContainsKey(historyData.Numbers[i]))
							{
								BackAreaCountDict[historyData.Numbers[i]]++;
							}
							else
							{
								BackAreaCountDict[historyData.Numbers[i]] = 1;
							}

						}
					}
				}

				for (int i = 1; i <= 35; i++)
				{
					if (!FrontAreaCountDict.ContainsKey(i))
					{
						FrontAreaCountDict[i] = 1;
					}
				}
				for (int i = 1; i <= 12; i++)
				{
					if (!BackAreaCountDict.ContainsKey(i))
					{
						BackAreaCountDict[i] = 1;
					}
				}

				int FrontAreaCountValueCount = FrontAreaCountDict.Values.Sum();
				int BackAreaCountValueCount = BackAreaCountDict.Values.Sum();
				for (int i = 0; i < ViewModel.ForecastCount; i++)
				{
					FrontArea.Clear();
					BackArea.Clear();
					SelectNum(FrontAreaCountValueCount, FrontArea, FrontAreaCountDict, 5);
					SelectNum(BackAreaCountValueCount, BackArea, BackAreaCountDict, 2);
					ForecastResult.Add(string.Join(",", FrontArea.OrderBy(ele => ele)) + "-" + string.Join(",", BackArea.OrderBy(ele => ele)));
				}
				ViewModel.ForeacastNumbers = ForecastResult;
			}
			this.Cursor = Cursors.Arrow;
		}

		private void SelectNum(int valueSum, IList<int> area, Dictionary<int, int> numCountDict, int areaCount)
		{
			if (area.Count >= areaCount)
			{
				return;
			}
			Thread.Sleep(10);
			int location = Random.Next(valueSum);
			int sum = 0;
			foreach (var entry in numCountDict)
			{
				sum += entry.Value;
				if (sum >= location)
				{
					if (!area.Contains(entry.Key))
					{
						area.Add(entry.Key);
					}
					SelectNum(valueSum, area, numCountDict, areaCount);
					break;
				}
			}

		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ViewModel = new MainWindowViewModel();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			SaveFileDialog save = new SaveFileDialog
			{
				Filter = "(文本文件)|*.txt"
			};
			if (save.ShowDialog().Value)
			{
				using (FileStream fileStream = File.Create(save.FileName))
				{
					foreach (var result in ViewModel.ForeacastNumbers)
					{
						byte[] byteArray = Encoding.UTF8.GetBytes(result);
						fileStream.Write(byteArray, 0, byteArray.Count());
						byte[] newlineArray = Encoding.UTF8.GetBytes(Environment.NewLine);
						fileStream.Write(newlineArray, 0, newlineArray.Count());
					}
				}
			}
		}
	}
}
