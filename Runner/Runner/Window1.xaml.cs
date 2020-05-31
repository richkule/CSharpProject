/*
 * Created by SharpDevelop.
 * User: KPK
 * Date: 20.03.2020
 * Time: 16:45
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
namespace Runner
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
		public partial class Player : Image
	{
		public int pic = 0;
		public int speed;
		string path = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
		public bool jump = false;
		public bool fall = false;
		public DispatcherTimer timer = new DispatcherTimer();
		public DispatcherTimer jtimer = new DispatcherTimer();
		void MovePlayer(object sender, EventArgs e)
		{
			Thickness thick = this.Margin;
			if (this.jump)
			{
				if (thick.Bottom >= 140)
				{
					this.jump = false;
					this.fall = true;
				}
				else thick.Bottom +=speed;
			}
			if (this.fall == true)
			{
				if (thick.Bottom == 0) this.fall = false;
				else thick.Bottom -=speed;
			}
			this.Margin = thick;
		}
		
		void animate(object sender, EventArgs e)
		{
			System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
			string fullpath;
			pic+=1;
			if (pic == 9) pic = 1;
			if (!(this.jump || this.fall))
			{
				fullpath = path+"Player"+pic.ToString()+".png";
				bi.BeginInit();
				bi.UriSource = new Uri(fullpath);
				bi.EndInit();
				this.Source = bi;
			}
		}

		public Player(int speed)
		{
			this.speed = speed;
			timer.Tick += new EventHandler(animate);
			jtimer.Tick += new EventHandler(MovePlayer);
			timer.Interval = new  TimeSpan(0,0,0,0,125);
			jtimer.Interval = new  TimeSpan(0,0,0,0,1);
			timer.Start();
			jtimer.Start();
			this.HorizontalAlignment = HorizontalAlignment.Left;
			this.VerticalAlignment = VerticalAlignment.Bottom;
			this.Width = 68;
			this.Height = 289;
		}
	}
	public partial class Box : Image
	{
		string path = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
		public int speed;
		public DispatcherTimer timer = new DispatcherTimer();
		void MoveBox(object sender, EventArgs e)
		{
			Thickness thick = this.Margin;
			thick.Right+=speed;
			this.Margin = thick;
			if (thick.Right > 995) timer.Stop();
		}
		public Box(int speed)
		{
			timer.Tick += new EventHandler(MoveBox);
			timer.Interval = new  TimeSpan(0,0,0,0,1);
			timer.Start();
			this.speed = speed;
			this.HorizontalAlignment = HorizontalAlignment.Right;
			this.VerticalAlignment = VerticalAlignment.Bottom;
			this.Width = 50;
			this.Height = 50;
			System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(path+"Box.png");
			bi.EndInit();
			this.Source = bi;
		}
	}

	public partial class Window1 : Window
	{
		string path = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
		List<Box> listbox = new List<Box>();
		DispatcherTimer Spawntimer = new DispatcherTimer();
		DispatcherTimer checktimer = new DispatcherTimer();
		DispatcherTimer scoretimer = new DispatcherTimer();
		Random spawntime = new Random();
		double speed;
		bool start,pause;
		int time = 0;
		Player player;
		public Window1()
		{
			InitializeComponent();
			System.Windows.Media.Imaging.BitmapImage bi;
			checktimer.Interval =  new  TimeSpan(0,0,0,0,50);
			checktimer.Tick += new EventHandler(Check);
			Spawntimer.Interval = new  TimeSpan(0,0,0,0,1);
			Spawntimer.Tick += new EventHandler(SpawnBox);
			scoretimer.Interval = new  TimeSpan(0,0,1);
			scoretimer.Tick += new EventHandler(Score);
			bi = new System.Windows.Media.Imaging.BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(path+"Cloud.png");
			bi.EndInit();
			cloud.Source = bi;
			bi = new System.Windows.Media.Imaging.BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(path+"Ground.png");
			bi.EndInit();
			ground.Source = bi;
		}
		void Jump(object sender, KeyEventArgs  e)
		{
			if(e.Key.ToString() == "Space")
			{
				if (!player.fall) 
				{
					player.jump = true;
					System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage(); 
					bi.BeginInit();
					bi.UriSource = new Uri(path+"jump.png");
					bi.EndInit();
					player.Source = bi;
				}
			}
			if(e.Key.ToString() == "Return")
			{
				if (start)
				{
					if (pause) Unpause();
					else Pause();
				}
			}
		}
		void SpawnBox(object sender, EventArgs e)
		{
			Box box = new Box(Convert.ToInt32(speed));
			Main_Grid.Children.Add(box);
			Grid.SetRow(box,2);
			listbox.Add(box);
			if (speed < 18) speed+=0.5;
			Spawntimer.Interval = new TimeSpan(0,0,0,0,spawntime.Next(1000,2500));
		}

		void Check(object sender, EventArgs e)
		{
			Box dels = new Box(1);
			bool b = false;
			foreach (Box box in listbox) {
				if (!start) break;
				box.speed = Convert.ToInt32(speed);
				if (!box.timer.IsEnabled) {
					Main_Grid.Children.Remove(box);
					dels = box;
				}
				else if (player.Margin.Bottom <= 50 && box.Margin.Right >878 )
				{
					b = true;
					break;
				}
			}
			listbox.Remove(dels);
			if (b)
			{
				Pause();
				WriteResult();
				GameOver();
			}
		}
		void StartGame(object sender, RoutedEventArgs e)
		{
			if (start) GameOver();
			if (e.Source == Eazy) speed = 5;
			if (e.Source == Medium) speed = 7;
			if (e.Source == Hard) speed = 10;
			Over.Visibility = Visibility.Hidden;
			ScoreLabel.Visibility = Visibility.Visible;
			Main_Grid.Background = new SolidColorBrush(Color.FromRgb(0,162,232));
			ground.Visibility = Visibility.Visible;
			cloud.Visibility = Visibility.Visible;
			player = new Player(Convert.ToInt32(speed));
			Main_Grid.Children.Add(player);
			Grid.SetRow(player,2);
			Spawntimer.Start();
			checktimer.Start();
			scoretimer.Start();
			start = true;
			pause = false;
		}
		void GameOver()
		{
			start = false;
			Spawntimer.Stop();
			checktimer.Stop();
			scoretimer.Stop();
			Over.Visibility = Visibility.Visible;
			Over.Content = "Игра окончена, вы проиграли. Ваш счет " + time.ToString();
			Clear();
			
		}
		void Clear()
		{
			Main_Grid.Background = new SolidColorBrush(Color.FromRgb(255,255,255));
			ground.Visibility = Visibility.Hidden;
			cloud.Visibility = Visibility.Hidden;
			ScoreLabel.Visibility = Visibility.Hidden;
			Main_Grid.Children.Remove(player);
			List<Box> fordel = new List<Box>();
			foreach (Box box in listbox) {
				Main_Grid.Children.Remove(box);
				fordel.Add(box);
			}
			foreach (Box box in fordel) {
				listbox.Remove(box);
			}
			time = 0;
		}
		void Score(object sender, EventArgs e)
		{
			time+=1;
			ScoreLabel.Content = "Ваш счет: " + time.ToString();
			if (time == 1000) Win();
		}
		void Win()
		{
			Pause();
			WriteResult();
			GameOver();
			Over.Content = "Вы победили"; 
		}
		void Pause()
		{
			pause = true;
			player.timer.Stop();
			player.jtimer.Stop();
			foreach ( Box box in listbox) {
				box.timer.Stop();
			}
			scoretimer.Stop();
			checktimer.Stop();
			Spawntimer.Stop();
			Over.Visibility = Visibility.Visible;
			Over.Content = "Пауза";
		}
		void Unpause()
		{
			pause = false;
			player.timer.Start();
			player.jtimer.Start();
			foreach ( Box box in listbox) {
				box.timer.Start();
			}
			scoretimer.Start();
			checktimer.Start();
			Spawntimer.Start();
			Over.Visibility = Visibility.Hidden;
		}
		void WriteResult()
		{
			
			this.Hide();
			Window3 form = new Window3();
			form.ShowDialog();
			string name = form.getName;
			string diff = "No";
			string fullpath = path+"result.txt";
			if (name == "") name = "Аноним";
			if (player.speed == 5) diff = "Легкая";
			if (player.speed == 7) diff = "Средняя";
			if (player.speed == 10) diff = "Тяжелая";
			using (StreamWriter writer = new StreamWriter(fullpath,true,Encoding.Unicode))
			{
				writer.WriteLine(diff+";"+name +";"+time.ToString());
				
			}
			this.Show();
		}
		void Save_Click(object sender, RoutedEventArgs e)
		{
			if (start)
			{
				Pause();
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
				    using (StreamWriter sw = File.CreateText(path))
		            {
				    	sw.WriteLine(player.fall.ToString());
				    	sw.WriteLine(player.jump.ToString());
				    	sw.WriteLine(player.speed.ToString());
				    	sw.WriteLine(player.Margin.Bottom.ToString());
				    	sw.WriteLine(player.pic.ToString());
				    	sw.WriteLine(time.ToString());
				    	sw.WriteLine(speed.ToString());
				    	int i = listbox.Count;
				    	sw.WriteLine(i.ToString());
				    	for (int j = 0; j < i; j++) {
				    		sw.WriteLine(listbox[j].Margin.Right.ToString());
				    	}
		            }  	
			    }	
			}	    
		}
		void Load_Click(object sender, RoutedEventArgs e)
		{
			if (start) Pause();
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
		    	GameOver();
				Main_Grid.Background = new SolidColorBrush(Color.FromRgb(0,162,232));
				ground.Visibility = Visibility.Visible;
				cloud.Visibility = Visibility.Visible;
				ScoreLabel.Visibility = Visibility.Visible;
				player = new Player(Convert.ToInt32(1));
				Main_Grid.Children.Add(player);
				Grid.SetRow(player,2);
				start = true;
				pause = true;
		    	using (StreamReader sr = new StreamReader(path))
	            {
		    		player.fall = Convert.ToBoolean(sr.ReadLine());
		    		player.jump = Convert.ToBoolean(sr.ReadLine());
		    		player.speed = Convert.ToInt32(sr.ReadLine());
		    		player.Margin = new Thickness(0,0,0,Convert.ToDouble(sr.ReadLine()));
		    		player.pic = Convert.ToInt32(sr.ReadLine());
		    		time = Convert.ToInt32(sr.ReadLine());
		    		speed = Convert.ToDouble(sr.ReadLine());
			    	int i = Convert.ToInt32(sr.ReadLine());
			    	listbox.Clear();
			    	for (int j = 0; j < i; j++) {
			    		Box box = new Box(Convert.ToInt32(speed));
			    		box.Margin = new Thickness(0,0,Convert.ToDouble(sr.ReadLine()),0);
						Main_Grid.Children.Add(box);
						Grid.SetRow(box,2);
						listbox.Add(box);
			    	}
		    	}
				
		    }
		    Pause();
		}
		void ShowResult(object sender, RoutedEventArgs e)
		{
			Window2 form = new Window2();
			form.Owner = this;
			form.Show();
		}
	}
}