using HtmlAgilityPack;
using Super_Lotto.DataSources.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Super_Lotto.DataSources
{
	public class DataProvider
	{
		public const string Address = "http://chart.lottery.gov.cn//dltBasicZongHeTongJi.do?typ=3&issueTop={0}&param=0";
		public IList<HistoryData> GetHistoryData(int IssueCount)
		{
			IList<HistoryData> historys = new List<HistoryData>();
			string url = string.Format(Address, IssueCount.ToString());
			var htmlWeb = new HtmlWeb();
			HtmlDocument htmlDocument = htmlWeb.Load(url, "Get");
			foreach (var attr in FilterFitElement(htmlDocument.DocumentNode.ChildNodes).Reverse())
			{
				var fitAtrrChild = FilterFitElement(attr.ChildNodes).ToList();
				if (fitAtrrChild.Count < 3)
				{
					continue;
				}
				HistoryData history = new HistoryData();
				history.Date = fitAtrrChild[0].InnerText;
				history.Issue = fitAtrrChild[1].InnerText;
				IList<int> numbers = new List<int>();
				string numbersText = fitAtrrChild[2].InnerText;
				foreach (var num in numbersText.Split('-'))
				{
					foreach (var n in num.Split(' '))
					{
						int value;
						bool isSuccess = int.TryParse(n, out value);
						if (isSuccess)
						{
							numbers.Add(value);
						}
					}
				}
				if (numbers.Any())
				{
					history.Numbers = numbers.ToArray();
					historys.Add(history);
				}
			}

			return historys;
		}

		private IEnumerable<HtmlNode> FilterFitElement(HtmlNodeCollection collection)
		{
			return collection.Where(ele => ele.NodeType == HtmlNodeType.Element);
		}
	}
}
