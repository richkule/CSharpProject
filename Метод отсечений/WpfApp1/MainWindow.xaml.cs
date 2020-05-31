using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /**
         * Метод добавляющий пробелы, пока строка не станет равна n символам
         */
        static string rstrip(string text, int n)
        {
            int len = text.Length;
            for(int i = 0;i<n-len;i++)
            {
                text = text + " ";
            }
            return text;
        }
        /**
       * Функция округления всего массива
       * Округляет числа до целого, если оно отличается от целого не более чем на eps
       */
        static void RoundMas(double[][] mas, double eps)
        {
            for (int i = 0; i < mas.Length; i++)
            {
                for (int j = 0; j < mas[0].Length; j++)
                {

                    double ch = Math.Round(mas[i][j]);
                    if (Math.Abs(ch - mas[i][j]) <= eps)
                    {
                        mas[i][j] = ch;
                    }
                }
            }
        }
        /**
         * Метод получение целочисленного решения методом отсечений
         * и его вывод
         * mas - Конечная симплекс таблица
         * basisIndex - массив индексов базисных переменных
         */
        static void Otsech(double[][] mas, int[] basisIndex,TextBox text, int bCount)
        {
            while (!CheckResult(mas))
            {
                int xCount = mas[0].Length;
                mas = Add_Otsech(mas);
                int[] newBasisIndex = new int[basisIndex.Length + 1];
                for (int i = 0; i < basisIndex.Length; i++)
                {
                    newBasisIndex[i] = basisIndex[i];
                }
                newBasisIndex[basisIndex.Length] = xCount - 1;
                basisIndex = newBasisIndex;
                Simplex(mas, basisIndex);
            }
            PrintSimplex(mas, basisIndex, text,bCount);
        }

        /**
         * Метод строящий строку с новым отсечением
         * mas - массив ограничений
         * stroka - строка по которой необходимо построить отсечение
         */
        static double[] OtsechLine(double[][] mas, int stroka)
        {
            double[] newLine = new double[mas[0].Length + 1];
            for (int i = 0; i < mas[0].Length - 1; i++)
            {
                double celchis = Math.Truncate(mas[stroka][i]);
                double otDrob = -(mas[stroka][i] - celchis);
                newLine[i] = otDrob;
            }
            newLine[mas[0].Length - 1] = 1;
            newLine[mas[0].Length] = -(mas[stroka][mas[0].Length - 1] - Math.Truncate(mas[stroka][mas[0].Length - 1]));
            return newLine;
        }
        /**
         * Метод добавляющий отсечение
         */
        static double[][] Add_Otsech(double[][] mas)
        {
            int xCount = mas[0].Length;
            int stroka = -1;
            double maxDrob = 0;
            for (int i = 0; i < mas.Length - 1; i++)
            {
                double celchis = Math.Truncate(mas[i][xCount - 1]);
                if (celchis != mas[i][xCount - 1])
                {
                    double drob = mas[i][xCount - 1] - celchis;
                    if (drob > maxDrob)
                    {
                        maxDrob = drob;
                        stroka = i;
                    }
                }
            }
            double[] newLine = OtsechLine(mas, stroka);
            return AddLine(mas, newLine);
        }
        /**
         * Метод возврающий массив с новой добавленной строкой
         */
        static double[][] AddLine(double[][] mas, double[] line)
        {

            double[][] newMas = new double[mas.Length + 1][];
            for (int j = 0; j < mas.Length - 1; j++)
            {
                newMas[j] = new double[mas[j].Length + 1];
                for (int z = 0; z < mas[j].Length - 1; z++)
                {
                    newMas[j][z] = mas[j][z];
                }
                newMas[j][mas[0].Length - 1] = 0;
                newMas[j][mas[0].Length] = mas[j][mas[0].Length - 1];
            }
            newMas[mas.Length - 1] = line;
            newMas[mas.Length] = new double[mas[0].Length + 1];
            for (int z = 0; z < mas[0].Length - 1; z++)
            {
                newMas[mas.Length][z] = mas[mas.Length - 1][z];
            }
            newMas[mas.Length][mas[0].Length - 1] = 0;
            newMas[mas.Length][mas[0].Length] = mas[mas.Length - 1][mas[0].Length - 1];
            return newMas;
        }
        /**
         * Метод возвращает
         * true - если полученное решение целочисленное
         */
        static bool CheckResult(double[][] mas)
        {
            RoundMas(mas, 0.000001);
            int xCount = mas[0].Length;
            for (int i = 0; i < mas.Length - 1; i++)
            {
                if (Math.Truncate(mas[i][xCount - 1]) != mas[i][xCount - 1]) return false;
            }
            return true;
        }
        /**
         * Метод возвращающий индекс строки, в котором содержится данный базис
         * mas - массив коэффициентов переменных
         * ind - индекс переменной
         * basisIndex - массив индексов базисных переменных
         */
        static int FindInd(double[][] mas, int ind, int[] basisIndex)
        {
            int lineInd = -1;
            for (int j = 0; j < mas.Length - 1; j++)
            {
                bool flag = true;
                foreach (int var in basisIndex)
                {
                    if (ind == var)
                    {
                        if (mas[j][var] != 1)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else
                    {
                        if (mas[j][var] != 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    lineInd = j;
                    break;
                }
            }
            return lineInd;
        }
        /**
         * Метод возращяющий номер строку c переменной c индексjv потребителя/поставщика 
         */
        static string indexPerem(int x, int bCount)
        {
            int xa = x / bCount + 1;
            int xb = x % bCount + 1;
            return "x" + xa.ToString() + " " + xb.ToString(); 
        }
        /**
         * Метод выводящий таблицу Симплекс-Метода
         * mas - Таблица коэффицентов переменных
         * basisIndex - массив базисных переменных
         * textBox - куда происходит вывод
         * aCount - количество поставщиков
         * bCount - количество потребителей
         */
        static void PrintSimplex(double[][] mas, int[] basisIndex, TextBox textBox,  int bCount)
        {
            const int countSymbol = 7;
            textBox.Text += rstrip("",countSymbol);
            for (int i = 0; i < mas[0].Length - 1; i++)
            {
                if (basisIndex.Contains(i)) continue;
                else textBox.Text += rstrip(indexPerem(i,bCount), countSymbol);
            }
            textBox.Text += "B\n";
            foreach (int ind in basisIndex)
            {
                int lineInd = FindInd(mas, ind, basisIndex);
                textBox.Text += rstrip(indexPerem(ind,bCount), countSymbol);
                for (int i = 0; i < mas[lineInd].Length; i++)
                {
                    if (basisIndex.Contains(i)) continue;
                    else textBox.Text += rstrip(mas[lineInd][i].ToString(),countSymbol);
                }
                textBox.Text += "\n";
            }
            textBox.Text += rstrip("F", countSymbol);;
            for (int i = 0; i < mas[mas.Length - 1].Length; i++)
            {
                if (basisIndex.Contains(i)) continue;
                else textBox.Text += rstrip(mas[mas.Length - 1][i].ToString(), countSymbol);
            }
        }
        /**
         * Метод пересчитывающий таблицу по правилу прямоугольников
         * stroka - строка с решающим элементом
         * stolb - столбец с решающим элементом
         */
        static void RefreshTable(double[][] mas, int stroka, int stolb, int[] basisIndex)
        {
            for (int i = 0; i < basisIndex.Length; i++)
            {
                if (FindInd(mas, basisIndex[i], basisIndex) == stroka)
                {
                    basisIndex[i] = stolb;
                    break;
                }
            }
            double[][] newmas = new double[mas.Length][];
            int xCount = mas[0].Length;
            double reshelem = mas[stroka][stolb];
            for (int i = 0; i < mas.Length; i++)
            {
                if (i == stroka)
                {
                    newmas[i] = new double[xCount];
                    for (int j = 0; j < xCount; j++)
                    {
                        newmas[i][j] = mas[i][j] / reshelem;
                    }
                }
                else
                {
                    newmas[i] = new double[xCount];
                    for (int j = 0; j < xCount; j++)
                    {
                        newmas[i][j] = mas[i][j] - mas[stroka][j] * mas[i][stolb] / reshelem;
                    }
                }
            }
            for (int i = 0; i < mas.Length; i++)
            {
                for (int j = 0; j < xCount; j++)
                {
                    mas[i][j] = newmas[i][j];
                }
            }
        }
        /**
         * Метод приводящий сисмплекс таблицу к опорному плану
         */
        static void ToBasePlan(double[][] mas, int[] basisIndex)
        {
            int xCount = mas[0].Length;
            while (true)
            {
                double min = 1;
                int strokind = -1;
                bool flag = false;
                // Поиск строки с наибольшим отрицательным B
                for (int i = 0; i < mas.Length - 1; i++)
                {
                    if (mas[i][xCount - 1] < 0 && mas[i][xCount - 1] < min)
                    {
                        min = mas[i][xCount - 1];
                        strokind = i;
                        flag = true;
                    }
                }
                min = 1;
                int stolbind = -1;
                // Если строка существует, поиск в ней столбца с наибольшей отрицательной переменной
                if (flag)
                {
                    flag = false;
                    for (int i = 0; i < xCount - 1; i++)
                    {
                        if (mas[strokind][i] < 0 && mas[strokind][i] < min)
                        {
                            min = mas[strokind][i];
                            stolbind = i;
                            flag = true;
                        }
                    }
                }
                // Если такая переменная существует замена базиса и пересчет таблицы
                if (flag)
                {

                    RefreshTable(mas, strokind, stolbind, basisIndex);
                }
                if (!flag) break;
            }
        }
        /**
         * Функция реализующая сисмплекс метод
         * mas - изначальная симплекс таблица
         * cij - массив стоимости перевозок для создания целевой функции
         * basisIndex - индексы базисных переменных
         */
        static void Simplex(double[][] mas, int[] basisIndex)
        {
            ToBasePlan(mas, basisIndex);
            double min;
            double max;
            int stroka = -1;
            int stolb = -1;
            int xCount = mas[0].Length;
            int lineCount = mas.Length;
            while (true)
            {

                min = 1e300;
                max = -1e300;
                bool flag = false;
                for (int i = 0; i < xCount - 1; i++)
                {
                    if (mas[lineCount - 1][i] > 0)
                    {
                        flag = true;
                        if (mas[lineCount - 1][i] > max)
                        {
                            stolb = i;
                            max = mas[lineCount - 1][i];
                        }
                    }
                }
                if (flag)
                {
                    flag = false;
                    for (int j = 0; j < lineCount - 1; j++)
                    {
                        if (mas[j][stolb] > 0)
                        {
                            if (mas[j][xCount - 1] / mas[j][stolb] < min)
                            {
                                min = mas[j][xCount - 1] / mas[j][stolb];
                                stroka = j;
                                flag = true;
                            }
                        }
                    }
                }
                if (!flag) break;
                RefreshTable(mas, stroka, stolb, basisIndex);
            }
        }
        /**
         * Функция преобразующая гауссовую матрицу - в матрицу
         * для симплекс метода и возвращающая индексы базисных переменных
         */
        static int[] GaussToSimplex(double[][] mas)
        {
            int countLine = mas.Length;
            int countX = mas[0].Length;
            int[] basisIndex = new int[countLine - 1];
            for (int i = countLine - 2; i >= 0; i--)
            {
                int ind = 0;
                for (int j = 0; j < countX; j++)
                {
                    if (mas[i][j] != 0)
                    {
                        ind = j;
                        basisIndex[i] = j;
                        break;
                    }
                }
                for (int j = i - 1; j >= 0; j--)
                {
                    if (mas[j][ind] != 0)
                    {
                        double mn = -mas[j][ind];
                        SumLine(mas[j], mas[i], mn);
                    }
                    if (mas[countLine - 1][ind] != 0)
                    {
                        double mn = -mas[countLine - 1][ind];
                        SumLine(mas[countLine - 1], mas[i], mn);
                    }
                }
            }
            return basisIndex;
        }
        /**
         * Метод удаляющий i-ую строку из массива
         */
        static double[][] DeleteLine(double[][] mas, int i)
        {
            double[][] newMas = new double[mas.Length - 1][];
            for (int j = 0; j < i; j++)
            {
                newMas[j] = new double[mas[j].Length];
                for (int z = 0; z < newMas[j].Length; z++)
                {
                    newMas[j][z] = mas[j][z];
                }
            }
            for (int j = i + 1; j < mas.Length; j++)
            {
                newMas[j - 1] = new double[mas[j].Length];
                for (int z = 0; z < newMas[j - 1].Length; z++)
                {
                    newMas[j - 1][z] = mas[j][z];
                }
            }
            return newMas;

        }
        /**
         * Метод суммирующий строки
         * line - Строка к которой происходит прибавление
         * plusLine - Строка, которая прибавляется
         * mn - Множитель на который домножается прибавляемая строка
         */
        static void SumLine(double[] line, double[] plusLine, double mn)
        {
            int len = line.Length;
            for (int i = 0; i < len; i++)
            {
                line[i] += plusLine[i] * mn;
            }
        }
        /**
         * Метод проверяющий строку на то является ли она "лестничной"
         * И приводящий ее к такому виду, чтобы первый не нулевой индекс был равен 1
         * line - проверяемая строка
         * ind - количество первых элементов, которые должны равняться нулю
         * return - true, если лестничная, иначе false
         */
        static bool CheckLine(double[] line, int ind)
        {
            for (int i = 0; i < ind; i++)
            {
                if (line[i] != 0) return false;
            }
            if (line[ind] == 0) return false;
            if (line[ind] == 1) return true;
            double mn = line[ind];
            for (int i = 0; i < line.Length; i++)
            {
                line[i] /= mn;
            }
            return true;
        }

        /**
         * Метод осуществляющий постройку расширенной матрицы ограничений
         * и приводящий ее к лестничному виду методом Жордана-ГГаусса
         */
        static double[][] Gauss(int[] ai, int[] bi, int[][] cij)
        {
            // Постройка изначальной матрицы
            int aCount = ai.Length;
            int bCount = bi.Length;
            int countLine = aCount + bCount;
            int countX = aCount * bCount + 1;
            double[][] gaussX = new double[countLine + 1][];
            for (int i = 0; i < aCount; i++)
            {
                gaussX[i] = new double[countX];
                for (int j = 0; j < countX; j++)
                {
                    if (j >= i * bCount && j < (i + 1) * bCount) gaussX[i][j] = 1;
                    else gaussX[i][j] = 0;
                }
                gaussX[i][countX - 1] = ai[i];
            }
            for (int i = 0; i < bCount; i++)
            {
                gaussX[i + aCount] = new double[countX];
                int ind = i;
                for (int j = 0; j < countX; j++)
                {
                    if (j == ind)
                    {
                        gaussX[i + aCount][j] = 1;
                        ind += bCount;
                    }
                    else gaussX[i + aCount][j] = 0;
                }
                gaussX[i + aCount][countX - 1] = bi[i];
            }
            gaussX[countLine] = new double[countX];
            int indx = 0;
            foreach (int[] var in cij)
            {
                foreach (int var2 in var)
                {
                    gaussX[countLine][indx] = -var2;
                    indx++;
                }
            }
            gaussX[countLine][countX - 1] = 0;
            int zeroCount = 0;
            for (int i = 0; i < countLine; i++)
            {
                bool flag = true;
                // Поиск строки, которая будет прибавляться к остальным на итерации
                while (flag && zeroCount < countX)
                {
                    for (int j = 0; j < countLine; j++)
                    {
                        if (CheckLine(gaussX[j], zeroCount))
                        {
                            double[] temp = gaussX[j];
                            gaussX[j] = gaussX[i];
                            gaussX[i] = temp;
                            flag = false;
                            break;
                        }
                    }
                    zeroCount++;
                }
                // Приведение остальных строк к лестничнуму виду
                for (int j = i + 1; j < countLine + 1; j++)
                {
                    if (gaussX[j][zeroCount - 1] != 0)
                    {
                        double mn = -gaussX[j][zeroCount - 1];
                        SumLine(gaussX[j], gaussX[i], mn);
                    }
                }
            }
            for (int i = 0; i < countLine; i++)
            {
                bool flag = true;
                for (int j = 0; j < countX; j++)
                {
                    if (gaussX[i][j] != 0) flag = false;
                }
                if (flag)
                {
                    gaussX = DeleteLine(gaussX, i);
                    countLine--;
                }
            }
            return gaussX;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string temp = "5";
            int bCount = Convert.ToInt16(Microsoft.VisualBasic.Interaction.InputBox("Введите количество потребителей", "Ввод", " ", -1, -1));
            int[] bi = new int[bCount];
            Console.Write("Введите количество поставщиков ");
            //temp = "3";
            int aCount = Convert.ToInt16(Microsoft.VisualBasic.Interaction.InputBox("Введите количество поставщиков", "Ввод", " ", -1, -1));
            int[] ai = new int[aCount];
            int[][] cij = new int[aCount][];
            string temp = Microsoft.VisualBasic.Interaction.InputBox("Введите строку с потребностями потребителей через пробел", "Ввод", " ", -1, -1);
            //temp = "20 12 5 8 15";
            int i = 0;
            foreach (string elem in temp.Split(' '))
            {
                bi[i] = Convert.ToInt32(elem);
                i++;
            }
            temp = Microsoft.VisualBasic.Interaction.InputBox("Введите строку с запасами поставщиков через пробел", "Ввод", " ", -1, -1);
            //temp = "15 25 20";
            i = 0;
            foreach (string elem in temp.Split(' '))
            {
                ai[i] = Convert.ToInt32(elem);
                i++;
            }
            for (int j = 0; j < aCount; j++)
            {
                cij[j] = new int[bCount];
                temp = Microsoft.VisualBasic.Interaction.InputBox("Вводите построчно стоимости перевозки каждого поставщика потребителям через пробел", "Ввод", " ", -1, -1);
                /* Тест транспортной задачи с сайта http://cyclowiki.org/wiki/Решение_транспортной_задачи_симплекс-методом но там без решения, но выглядит что работает
                if (j == 0) temp = "1 0 3 4 2";
                if (j == 1) temp = "5 1 2 3 3";
                if (j == 2) temp = "4 8 1 4 3";
                */
                i = 0;
                foreach (string elem in temp.Split(' '))
                {
                    cij[j][i] = Convert.ToInt32(elem);
                    i++;
                }
            }
            double[][] mas = Gauss(ai, bi, cij);
            int[] basisIndex = GaussToSimplex(mas);
            Simplex(mas, basisIndex);
            Otsech(mas, basisIndex,textBox1,bCount);
            /*
             * Тест метода Отсечений с сайта https://lektsii.org/17-46230.html 
             
            double[][] test = new double[3][];
            test[0] = new double[] { -1, 3, 1,0, 6 };
            test[1] = new double[] { 7, 1, 0,1, 35 };
            test[2] = new double[] { 7, 10, 0, 0, 0 };
            int[] basisIndex = new int[] { 2, 3 };
            Simplex(test, basisIndex);
            Otsech(test, basisIndex);
            */
            Console.ReadLine();

        }
    }
}
