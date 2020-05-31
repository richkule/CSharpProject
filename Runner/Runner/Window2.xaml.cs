/*
 * Created by SharpDevelop.
 * User: KPK
 * Date: 03/23/2020
 * Time: 02:55
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
namespace Runner
{
	/// <summary>
	/// Interaction logic for Window2.xaml
	/// </summary>
	public partial class Window2 : Window
	{
		string path = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory) + "result.txt";
		void ReadResult()
		{
			string line;
			Label label;
			using (StreamReader sr = new StreamReader(path))
	   		{
				while ((line = sr.ReadLine()) != null)
				{
					string[] mas = line.Split(';');
					label = new Label();
					label.HorizontalContentAlignment = HorizontalAlignment.Center;
					label.VerticalContentAlignment = VerticalAlignment.Center;
					label.Content = mas[0];
					diff.Children.Add(label);
					label = new Label();
					label.HorizontalContentAlignment = HorizontalAlignment.Center;
					label.VerticalContentAlignment = VerticalAlignment.Center;
					label.Content = mas[1];
					name.Children.Add(label);
					label = new Label();
					label.HorizontalContentAlignment = HorizontalAlignment.Center;
					label.VerticalContentAlignment = VerticalAlignment.Center;
					label.Content = mas[2];
					score.Children.Add(label);
				}
	   		}
		}
		public Window2()
		{
			InitializeComponent();
			ReadResult();
		}
	}

}