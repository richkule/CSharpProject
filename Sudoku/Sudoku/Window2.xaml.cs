/*
 * Created by SharpDevelop.
 * User: KPK
 * Date: 02/27/2020
 * Time: 18:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
namespace Sudoku
{
	/// <summary>
	/// Interaction logic for Window2.xaml
	/// </summary>
	public partial class Window2 : Window
	{
		public Window2()
		{
			InitializeComponent();
			Read_Best();
		}
		string base_path = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
		public partial class name_time
		{
			public string name;
			public int time;
		}
		int SortFun(name_time x, name_time y)
		{
			if (x.time<y.time) return -1;
			if (x.time==y.time) return 0;
			if (x.time>y.time) return 1;
			else return 0;
		}
		void Read_Best()
		{
			string path1 = base_path+"/Best/Легкая.txt";
			string path2 = base_path+"/Best/Средняя.txt";
			string path3 = base_path+"/Best/Тяжелая.txt";
			string[] phs = new string[] {path1,path2,path3};
			StackPanel[] sps = new StackPanel[] {eazy_panel,med_panel,hard_panel};
			for(int i = 0;i<3;i++) 
			{
				using (System.IO.StreamReader sw = new System.IO.StreamReader(phs[i]))
	       		{
					List<name_time> list = new List<name_time>();
					string line;
					while ((line = sw.ReadLine()) != null)
					{
						if (line == "") break;
						name_time new_best = new name_time();
						string[] split = line.Split(';');
						new_best.name = split[0];
						new_best.time = Convert.ToInt32(split[1]);
						list.Add(new_best);
					}
					list.Sort(SortFun);
					foreach (name_time element in list)
					{
						Label new_best = new Label();
						new_best.Content = element.name + " за " + Convert.ToString(element.time) + " секунд";
						new_best.HorizontalContentAlignment = HorizontalAlignment.Center;
						sps[i].Children.Add(new_best);
					}
	       		}
			}
		}
	}
}