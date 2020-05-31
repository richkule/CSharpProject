/*
 * Created by SharpDevelop.
 * User: KPK
 * Date: 12/07/2019
 * Time: 02:37
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

namespace Game_Pazzle
{
	/// <summary>
	/// Interaction logic for Recrord.xaml
	/// </summary>
	public partial class Recrord : Window
	{
		public Recrord()
		{
			InitializeComponent();
			read();
		}
		public partial class pobed
		{
			public string name;
			public int shag;
		}
		int srav(pobed x, pobed y)
		{
			if (x.shag<y.shag) return -1;
			if (x.shag==y.shag) return 0;
			if (x.shag>y.shag) return 1;
			else return 0;
		}
		string path = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
		void read()
		{
			
			string path1 = path+@"\tab\3x3.txt";
			string path2 = path+@"\tab\4x4.txt";
			string path3 = path+@"\tab\5x5.txt";
			string[] phs = new string[] {path1,path2,path3};
			StackPanel[] sps = new StackPanel[] {s3,s4,s5};
			for(int i = 0;i<3;i++) 
			{
				using (System.IO.StreamReader sw = new System.IO.StreamReader(phs[i]))
	       		{
					List<pobed> list = new List<Recrord.pobed>();
					string s;
					while ((s = sw.ReadLine()) != null)
					{
						if (s=="") break;
						pobed pob = new pobed();
						string[] mas = s.Split('`');
						pob.name = mas[0];
						pob.shag = Convert.ToInt32(mas[1]);
						list.Add(pob);
					}
					list.Sort(srav);
					foreach (pobed element in list)
					{
						Label lb = new Label();
						lb.Content = element.name + " за " + Convert.ToString(element.shag) + " секунд";
						sps[i].Children.Add(lb);
					}
	       		}
			}
		}
		
	}
}