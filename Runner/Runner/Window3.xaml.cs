/*
 * Created by SharpDevelop.
 * User: KPK
 * Date: 03/23/2020
 * Time: 03:27
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

namespace Runner
{
	/// <summary>
	/// Interaction logic for Window3.xaml
	/// </summary>
	public partial class Window3 : Window
	{
		public Window3()
		{
			InitializeComponent();
		}
		
		void button1_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
		 public string getName
		 {
		 	get {
		 		return name.Text;
		 	}
		 }
	}
}