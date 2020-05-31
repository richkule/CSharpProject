/*
 * Created by SharpDevelop.
 * User: KPK
 * Date: 27.02.2020
 * Time: 14:18
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
using System.Windows.Threading;
namespace Sudoku
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Kletka : TextBox
	{
		public string zn;
	}
	public partial class Window1 : Window
	{
		public List<Kletka> kletka_array = new List<Kletka>();
		public string base_path = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
		public string difficulity = "Средняя";
		public int time;
		DispatcherTimer timer = new DispatcherTimer();
		void Start_Game(object sender, RoutedEventArgs e)
		{
			string path = base_path + "/Game/";
			if (sender == Eazy) difficulity = "Легкая";
			if (sender == Medium) difficulity = "Средняя";
			if (sender == Hard) difficulity = "Тяжелая";
			path = path+difficulity;
			StreamReader gr = new StreamReader(path+"/Game.txt");
			StreamReader sr = new StreamReader(path+"/Solution.txt");
			char gl;
			char sl;
			int n = 0;
			for(int i = 0;i<11;i++)
			{
				if ((i+1)%4==0) continue;
				for(int j = 0;j<=12;j++)
				{
					if(j>=11)
					{
						gr.Read();
						sr.Read();
						continue;
					}
					if((j+1)%4 == 0) continue;
					gl = (char)gr.Read();
					sl = (char)sr.Read();
					if (gl == ' ')
					{
						kletka_array[n].Text = "";
						kletka_array[n].IsEnabled = true;
					}
					else
					{
						kletka_array[n].Text = gl.ToString();
						kletka_array[n].IsEnabled = false;
					}
					kletka_array[n].zn = sl.ToString();
					Grid.SetColumn(kletka_array[n],j);
					Grid.SetRow(kletka_array[n],i);
					n++;
				}
			}
			sr.Close();
			gr.Close();
			time = 0;
			dif_label.Content="Сложность: " + difficulity;
			timer.Start();
		}
		void Save_Game(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.	SaveFileDialog();
		    saveFileDialog1.Filter = "Text documents (.txt)|*.txt";
		    saveFileDialog1.Title = "Создание сохранения";
		    saveFileDialog1.DefaultExt = ".txt";
		    saveFileDialog1.FileName = "Save";
		    saveFileDialog1.ShowDialog();
		    string path = saveFileDialog1.FileName;
		    if (saveFileDialog1.FileName == "Save") MessageBox.Show("Не выбран путь сохранения");
		    else
		    {
		    	 using (StreamWriter sw = new StreamWriter(path))
	            {
			    	foreach (Kletka elem in kletka_array) 
			    	{
			    		if (elem.Text == "")sw.Write(" ");
			    		else sw.Write(char.Parse(elem.Text));
		    	 		sw.Write(char.Parse(elem.zn));
		    	 		if (elem.IsEnabled) sw.Write('1');
		    	 		else sw.Write('0');
			    	}
			    	sw.WriteLine();
			    	sw.WriteLine(difficulity);
			    	sw.WriteLine(time.ToString());
			    	
	            }  	
		    }
		}
		void Load_Game(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.FileName = "Save"; 
			dlg.DefaultExt = ".txt"; 
			dlg.Title = "Загрузка сохранения";
			dlg.Filter = "Text documents (.txt)|*.txt";
		    dlg.ShowDialog();
		    string path = dlg.FileName;
		    if (dlg.FileName=="Save") MessageBox.Show("Не выбрано сохранение");
		    else
		    {
		    	using (StreamReader sr = new StreamReader(path))
	            {
		    		int n = 0;
		    		string text;
		    		for(int i = 0;i<11;i++)
		    		{
		    			if ((i+1)%4==0) continue;
		    			for(int j = 0;j<11;j++)
		    			{
		    				if ((j+1)%4==0) continue;
		    				text = ((char)sr.Read()).ToString();
		    				if (text == " ") kletka_array[n].Text = "";
		    				else kletka_array[n].Text=text;
		    				kletka_array[n].zn=((char)sr.Read()).ToString();
		    				if (sr.Read()=='1') kletka_array[n].IsEnabled = true;
		    				else kletka_array[n].IsEnabled = false;
		    				Grid.SetColumn(kletka_array[n],j);
		    				Grid.SetRow(kletka_array[n],i);
		    				n++;
		    			}
		    		}
		    		sr.ReadLine();
		    		difficulity = sr.ReadLine();
		    		dif_label.Content = "Сложность: "+difficulity;
		    		time = Convert.ToInt16(sr.ReadLine());
		    		timer.Start();
		    	}
		    }
		}
		void MakeBorder(int i, int j)
		{
			Border bord = new Border();
			bord.Background = Brushes.Black;
			Sudoku_Grid.Children.Add(bord);
			Grid.SetRow(bord,i);
			Grid.SetColumn(bord,j);
		}
		void MakeGrid()
		{
			for(int i = 0;i<11;i++)
			{
				Sudoku_Grid.RowDefinitions.Add(new RowDefinition());
				Sudoku_Grid.ColumnDefinitions.Add(new ColumnDefinition());
			}
			for(int i = 0;i<11;i++)
			{
				for(int j = 0;j<11;j++)
				{
					if((i+1)%4==0)
					{
						Sudoku_Grid.ColumnDefinitions[i].Width = new GridLength(2);	
						MakeBorder(i,j);
						continue;
					}
					if((j+1)%4==0)
					{
						Sudoku_Grid.RowDefinitions[j].Height = new GridLength(2);
						MakeBorder(i,j);
						continue;
					}
					Kletka kletka = new Kletka();
					kletka.MaxLength = 1;
					kletka.FontSize = 20;
					kletka.BorderBrush = Brushes.Black;
					kletka.HorizontalContentAlignment = HorizontalAlignment.Center;
					kletka.VerticalContentAlignment = VerticalAlignment.Center;
					kletka_array.Add(kletka);
					kletka.TextChanged+=Text_Change;
					Sudoku_Grid.Children.Add(kletka);
					Grid.SetColumn(kletka,j);
					Grid.SetRow(kletka,i);
				}
			}
		}
		void timerTick(object sender, EventArgs e)
        {
            time++;
            time_label.Content = "Время: " + Convert.ToString(time);
        }
		void Text_Change(object sender, RoutedEventArgs e)
		{
			bool k = true;
			foreach (Kletka elem in kletka_array) {
				if (elem.zn!=elem.Text){
					k = false;
					break;					
				}
			}
			if (k)
			{
				End_Game();
			}
		}
		void End_Game()
		{
			string name = Player_Name.Text;
			if (name == "") name = "Аноним";
			name = name + ";" + time.ToString();
			using (StreamWriter writer = new StreamWriter(base_path+"/Best/"+difficulity+".txt",true))
			{
				writer.WriteLine(name);
			}
			foreach (Kletka element in kletka_array) {
				element.IsEnabled = false;
			}
            timer.Stop();	
            MessageBox.Show("Игра Окончена");
		}
		void Show_Best(object sender, RoutedEventArgs e)
		{
			Window2 win_best = new Window2();
			win_best.Show();
		}
		public Window1()
		{	
			InitializeComponent();
			MakeGrid();
			timer.Tick += new EventHandler(timerTick);
			timer.Interval = new TimeSpan(0,0,1);
		}
	}
}