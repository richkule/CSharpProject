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
namespace  Game_Pazzle
{
	public partial class Puzzle_Button : Button
	{
		public int nRow;
		public int nCol;
		public int Row;
		public int Col;
		public int ind;
		public bool naj = false;
		public bool mest = false;
		public string img_path;
	}
	
	public partial class Window1 : Window
	{
		public Window1()
		{
			timer.Tick += new EventHandler(timerTick);
			timer.Interval = new TimeSpan(0,0,1);
			InitializeComponent();

		}
		void prov()
		{
			bool k = true;
			foreach (Puzzle_Button element in PB) {
				if (element.mest == false)
				{
					k = false;
					break;
				}
			}
			if (k) gameOver();
			
		}
		
		void perem(List<Puzzle_Button> PB,int i)
		{
			Random rand = new Random();
			List<Puzzle_Button> LPB = new List<Puzzle_Button>();
			foreach (Puzzle_Button element in PB) {
				LPB.Add(element);
			}
			List<int[]> coor = new List<int[]>();
			for (int i2 = 0;i2<i;i2++)
			{
				for(int j = 0;j<i;j++)
				{
					int[] xy = new int[2];
					xy[0] = i2;
					xy[1] = j;
					coor.Add(xy);
				}
			}
			int x = -1;
			while (LPB.Count>0) {
				x++;
				int chet = 0;
				int[] coor2 = new int[2];
				coor2[0] = LPB[0].nCol;
				coor2[1] = LPB[0].nRow;
				int[] xy = new int[2];
				do{
					xy = coor[rand.Next(0, coor.Count)];
					chet++;
					if (chet==100){
						break;
					}
				}
				while (coor2[0]==xy[0] && coor2[1]==xy[1]);
				PB[x].Col = xy[0];
				PB[x].Row = xy[1];
				Grid.SetColumn(PB[x],PB[x].Col);
				Grid.SetRow(PB[x],PB[x].Row);
				coor.Remove(xy);
				LPB.RemoveAt(0);
				
			}
			
		}
		public List<Puzzle_Button> PB = new List<Puzzle_Button>();
		public string path = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory),zp = "";
		public int shag,zi = 0;
		public List<int[]> shagNas = new List<int[]>();
		DispatcherTimer timer = new DispatcherTimer();
		void Puz_Click(object sender, RoutedEventArgs e)
		{
			Puzzle_Button pb = (Puzzle_Button)sender;
			if (pb.mest == false){
				if (pb.naj == true){
					pb.Background = null;
					pb.naj = false;
				}
				else{
					Puzzle_Button pb2 = pb;
					foreach (Puzzle_Button elem in PB) {
						if (elem.naj==true){
							pb2 = elem;
							break;
						}
					}
					if (pb2 == pb){
						pb.Background =new SolidColorBrush(Colors.Blue);
						pb.naj = true;
					}
					else{
						int c = pb2.Col;
						int r = pb2.Row;
						pb2.Col = pb.Col;
						Grid.SetColumn(pb2,pb.Col);
						pb2.Row = pb.Row;
						Grid.SetRow(pb2,pb.Row);
						pb.Col = c;
						Grid.SetColumn(pb,c);
						pb.Row = r;
						Grid.SetRow(pb,r);
						pb2.naj = false;
						pb2.Background = null;
						if (pb.Col == pb.nCol&&pb.Row==pb.nRow){
							pb.mest = true;
							pb.Background = new SolidColorBrush(Colors.LightGreen);
						}
						if (pb2.Col == pb2.nCol&&pb2.Row==pb2.nRow){
							pb2.mest = true;
							pb2.Background = new SolidColorBrush(Colors.LightGreen);
						}
						int[] sn = new int[2];
						sn[0] = pb.ind;
						sn[1] = pb2.ind;
						shagNas.Add(sn);
						prov();
					}
				}
			}
			
		}
		
		void fill_grid(int i, string pic){
			int picnumber;
			int size = 0;
			if (i==3) size = 140;
			if (i==4) size = 105;
			if (i==5) size = 84;
			for (int i2 = 0;i2<i;i2++)
			{
				RowDefinition nrow = new RowDefinition();
				nrow.Height = new GridLength();
				GameGrid.RowDefinitions.Add(nrow);
				for(int j = 0;j<i;j++)
				{
					picnumber = i*i2 + j + 1;
					ColumnDefinition ncol = new ColumnDefinition();
					ncol.Width = new GridLength();
					GameGrid.ColumnDefinitions.Add(ncol);
					string fullpath = path+"/image/"+pic+"/"+Convert.ToString(i)+"/("+Convert.ToString(picnumber)+").jpg";
					Puzzle_Button nBut = new Puzzle_Button();
					nBut.nRow = i2;
					nBut.nCol = j;
					nBut.Col = j;
					nBut.Row = i2;
					nBut.Width = size;
					nBut.Height = size;
					nBut.Click += new RoutedEventHandler(Puz_Click);
					nBut.img_path = fullpath;
					nBut.ind = picnumber-1;
					Image im = new Image();
					System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
					bi.BeginInit();
					bi.UriSource = new Uri(fullpath);
					bi.EndInit();
					im.Source = bi;
					im.Width = size-10;
					im.Height = size-10;
					im.HorizontalAlignment = HorizontalAlignment.Center;
					im.VerticalAlignment = VerticalAlignment.Center;
					nBut.Content = im;
					PB.Add(nBut);
					GameGrid.Children.Add(nBut);
				}
			}
			perem(PB,i);
			
		}
		void start_Click(object sender, RoutedEventArgs e)
		{
			int i = 0;
			string pic = "";
			if(eazyB.IsChecked==true) i = 3;
			if(medB.IsChecked==true) i = 4;
			if(hardB.IsChecked==true) i = 5;
			if(tigerB.IsChecked==true) pic = "tiger";
			if(carB.IsChecked==true) pic = "car";
			if(treeB.IsChecked==true) pic = "tree";
			if(pic == "" || i == 0) MessageBox.Show("Выберите настройки игры");
			else{
				zi = i;
				zp = pic;
				GameGrid.Children.Clear();
				PB.Clear();
				fill_grid(i,pic);
				shagNas.Clear();
				shag = 0;
				timer.Start();
			}
		}
		
		void button1_Click(object sender, RoutedEventArgs e)
		{
			
			if (helpm.Visibility == Visibility.Visible) helpm.Visibility = Visibility.Hidden;
			else{
				string pic = "";
				if(tigerB.IsChecked==true) pic = "tiger";
				if(carB.IsChecked==true) pic = "car";
				if(treeB.IsChecked==true) pic = "tree";
				if (zp!= "") pic = zp;
				if (pic == "") MessageBox.Show("Не выбрана картинка");
				else
				{
					string fullpath =  path+"/image/"+pic+"/"+pic+".jpg";
					System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
					bi.BeginInit();
					bi.UriSource = new Uri(fullpath);
					bi.EndInit();
					helpm.Source = bi;
					helpm.Visibility = Visibility.Visible;
				}
			}

		}
		
		void step_Click(object sender, RoutedEventArgs e)
		{
			if (shagNas.Count == 0) MessageBox.Show("Игра в изначальном состоянии");
			else{
				int[] sn = shagNas[shagNas.Count-1];
				Puzzle_Button pb = PB[sn[0]];
				Puzzle_Button pb2 = PB[sn[1]];
				int c = pb2.Col;
				int r = pb2.Row;
				pb2.Col = pb.Col;
				Grid.SetColumn(pb2,pb.Col);
				pb2.Row = pb.Row;
				Grid.SetRow(pb2,pb.Row);
				pb.Col = c;
				Grid.SetColumn(pb,c);
				pb.Row = r;
				Grid.SetRow(pb,r);
				pb2.naj = false;
				pb2.Background = null;
				pb2.mest = false;
				pb.naj = false;
				pb.Background=null;
				pb.mest = false;
				shagNas.RemoveAt(shagNas.Count-1);
			}
			
		}
		
		void again_Click(object sender, RoutedEventArgs e)
		{
			if (zi == 0) MessageBox.Show("Игра не запущена");
			else
			{
				GameGrid.Children.Clear();
				PB.Clear();
				shagNas.Clear();
				fill_grid(zi,zp);
				shag = 0;
			}
		}
		void Save_Click(object sender, RoutedEventArgs e)
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
			    using (StreamWriter sw = File.CreateText(path))
	            {
			    	sw.WriteLine(Convert.ToString(shag));
			    	sw.WriteLine(Convert.ToString(PB.Count));
			    	foreach (Puzzle_Button elem in PB) {
			    		string m = Convert.ToString(elem.mest);
			    		string n = Convert.ToString(elem.naj);
			    		string col = Convert.ToString(elem.Col);
			    		string row = Convert.ToString(elem.Row);
			    		string ncol = Convert.ToString(elem.nCol);
			    		string nrow = Convert.ToString(elem.nRow);
			    		string ind = Convert.ToString(elem.ind);
			    		sw.WriteLine(m+"`"+n+"`"+col+"`"+row+"`"+ncol+"`"+nrow+"`"+ind+"`"+elem.img_path);
			    	}
			    	sw.WriteLine(Convert.ToString(shagNas.Count));
			    	foreach (int[] element in shagNas) {
			    		sw.WriteLine(Convert.ToString(element[0])+"`"+Convert.ToString(element[1]));
			    	}
			    	sw.WriteLine(zi.ToString());
			    	sw.WriteLine(zp);
	            }  	
		    }
		    
		}
		void Load_Click(object sender, RoutedEventArgs e)
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
		    	GameGrid.Children.Clear();
				PB.Clear();
				shagNas.Clear();
		    	using (StreamReader sw = new StreamReader(path))
	            {
			    	shag = Convert.ToInt32(sw.ReadLine());
			    	shL.Content = "Количество шагов: " + Convert.ToString(shag);
			    	int pbCount = Convert.ToInt32(sw.ReadLine());
			    	for(int j = 0;j<pbCount;j++)
			    	{
			    		string stpb = sw.ReadLine();
			    		string[] mas = stpb.Split('`');
			    		int size = 0;
						if (pbCount==9)
						{
							for (int i = 0; i < 3; i++) {
								ColumnDefinition ncol = new ColumnDefinition();
								ncol.Width = new GridLength();
								GameGrid.ColumnDefinitions.Add(ncol);
								RowDefinition nrow = new RowDefinition();
								nrow.Height = new GridLength();
								GameGrid.RowDefinitions.Add(nrow);	
							}
	
							size = 140;
						}
						if (pbCount==16)
						{
							for (int i = 0; i < 4; i++) {
								ColumnDefinition ncol = new ColumnDefinition();
								ncol.Width = new GridLength();
								GameGrid.ColumnDefinitions.Add(ncol);
								RowDefinition nrow = new RowDefinition();
								nrow.Height = new GridLength();
								GameGrid.RowDefinitions.Add(nrow);	
							}						
							size = 105;
						}
						if (pbCount==25)
						{
							for (int i = 0; i < 5; i++) {
								ColumnDefinition ncol = new ColumnDefinition();
								ncol.Width = new GridLength();
								GameGrid.ColumnDefinitions.Add(ncol);
								RowDefinition nrow = new RowDefinition();
								nrow.Height = new GridLength();
								GameGrid.RowDefinitions.Add(nrow);	
							}		
							size = 84;
						}
			    		Puzzle_Button nBut = new Puzzle_Button();
			    		nBut.mest = Convert.ToBoolean(mas[0]);
			    		if (nBut.mest) nBut.Background =new SolidColorBrush(Colors.LightGreen);
			    		nBut.naj = Convert.ToBoolean(mas[1]);
			    		if (nBut.naj) nBut.Background = new SolidColorBrush(Colors.Blue); 
			    		nBut.nRow = Convert.ToInt32(mas[5]);
			    		nBut.nCol = Convert.ToInt32(mas[4]);
			    		nBut.Col = Convert.ToInt32(mas[2]);
			    		nBut.Row = Convert.ToInt32(mas[3]);
						nBut.Width = size;
						nBut.Height = size;
						nBut.Click += new RoutedEventHandler(Puz_Click);
						nBut.img_path = mas[7];
						nBut.ind = Convert.ToInt32(mas[6]);
						Image im = new Image();
						System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
						bi.BeginInit();
						bi.UriSource = new Uri(mas[7]);
						bi.EndInit();
						im.Source = bi;
						im.Width = size-10;
						im.Height = size-10;
						im.HorizontalAlignment = HorizontalAlignment.Center;
						im.VerticalAlignment = VerticalAlignment.Center;
						nBut.Content = im;
						PB.Add(nBut);
						GameGrid.Children.Add(nBut);
						Grid.SetColumn(nBut,nBut.Col);
						Grid.SetRow(nBut,nBut.Row);
			    	}
			    	int shNcount = Convert.ToInt32(sw.ReadLine());
			    	for(int j = 0;j<shNcount;j++)
			    	{
			    		string[] mas = sw.ReadLine().Split('`');
			    		int[] sn = new int[2];
			    		sn[0] = Convert.ToInt32(mas[0]);
			    		sn[1] = Convert.ToInt32(mas[1]);
						shagNas.Add(sn);
			    	}
			    	zi = Convert.ToInt32(sw.ReadLine());
			    	zp = sw.ReadLine();
			    	zp = zp;
	            }
		    }
  
		}
		void PobForm(object sender, RoutedEventArgs e)
		{
			Recrord PF = new Game_Pazzle.Recrord();
			PF.Owner = this;
			PF.Show();
		}
		void keybroad(object sender, KeyEventArgs  e)
		{
			if (tname.IsFocused != true)
			{
				if(e.Key.ToString() == "H") button1_Click(button1,null);
				if(e.Key.ToString() == "Return") start_Click(start,null);
				if(e.Key.ToString() == "Escape") step_Click(step,null);
				if(e.Key.ToString() == "R") again_Click(again,null);
			}
		}
		void gameOver()
		{
			MessageBox.Show("Игра Окончена");
			string name = tname.Text;
			if (name == "") name = "Аноним";
			name = name + "`" + Convert.ToString(shag);
			string fullpath = path+@"\tab\"+Convert.ToString(zi)+"x"+Convert.ToString(zi)+".txt";
			using (StreamWriter writer = new StreamWriter(fullpath,true,Encoding.Unicode))
			{
				writer.WriteLine(name);
			}
            Recrord PF = new Game_Pazzle.Recrord();
            timer.Stop();
		}
		void timerTick(object sender, EventArgs e)
        {
            shag++;
            shL.Content = "Прошло времени: " + Convert.ToString(shag);
        }
	}
}