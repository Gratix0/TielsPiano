using System;
using System.Drawing;
using System.Windows.Forms;

namespace PianoTiles
{
    public partial class Form1 : Form
    {
        // map & cell size 
        public int[,] map = new int[8, 4];
        public int cellWidth = 50;
        public int cellHeight = 80;

        public Form1()
        {

            InitializeComponent();
            // name & size 
            this.Text = "Piano";
            this.Width = cellWidth * 4 + 15;
            this.Height = cellHeight * 8 + 40;
            this.Paint += new PaintEventHandler(Repaint);
            this.KeyUp += new KeyEventHandler(OnKeyboardPressed);
            Init();
        }

        // Handling keystrokes
        private void OnKeyboardPressed(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "D1": // D1 - это просто обазначение клавиши на клавиатуре
                    CheckForPressedButton(0);
                    break;
                case "D2":
                    CheckForPressedButton(1);
                    break;
                case "D3":
                    CheckForPressedButton(2);
                    break;
                case "D4":
                    CheckForPressedButton(3);
                    break;
            }
        }

        public void CheckForPressedButton(int i)
        {
            if (map[7, i] != 0) // если нажатая кнопка верна, то продолжаем и воспроизводим звук 
            {
                MoveMap();
                PlaySound(i);
            }
            else // если была нажата не верная клавиша - начинаем сначала
            {
                MessageBox.Show("You lost!");
                Init();
            }
        }

        public void PlaySound(int sound) // в зависимости от нажатия клавиши воспроизводим звук из файла
        {
            System.IO.Stream str = null;
            switch (sound)
            {
                case 0:
                    str = Properties.Resources.g6;
                    break;
                case 1:
                    str = Properties.Resources.f6;
                    break;
                case 2:
                    str = Properties.Resources.d6;
                    break;
                case 3:
                    str = Properties.Resources.e6;
                    break;
            }
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        public void MoveMap()
        {
            for (int i = 7; i > 0; i--) // смещает все ячейки на одну вниз
            {
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = map[i - 1, j];
                }
            }
            AddNewLine();
            Invalidate();
        }

        public void AddNewLine() // создается новая пустая строка, в которую записываем случайный елемент равный 1 
        {
            Random r = new Random();
            int j = r.Next(0, 4);
            for (int k = 0; k < 4; k++)
                map[0, k] = 0;
            map[0, j] = 1;
        }

        public void Init()
        {
            ClearMap();
            GenerateMap();
            Invalidate();
        }

        // cleaner
        public void ClearMap()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        // generating initial map
        public void GenerateMap()
        {
            Random r = new Random();
            for (int i = 0; i < 8; i++)
            {
                int j = r.Next(0, 4);
                map[i, j] = 1;
            }
        }

        // generates a subsequent map
        public void DrawMap(Graphics g)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (map[i, j] == 0) // если элемент в карте равен 0 то заполним этот элемент карты белым цветом
                    {
                        g.FillRectangle(new SolidBrush(Color.White), cellWidth * j, cellHeight * i, cellWidth, cellHeight);
                    }
                    if (map[i, j] == 1) // если одному, то заполняем чёрным 
                    {
                        g.FillRectangle(new SolidBrush(Color.Black), cellWidth * j, cellHeight * i, cellWidth, cellHeight);
                    }
                }
            }
            // отрисовка клетчатых границ между ячейками (типа полей)
            for (int i = 0; i < 8; i++)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Black)), 0, i * cellHeight, 4 * cellWidth, i * cellHeight);
            }
            for (int i = 0; i < 4; i++)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Black)), i * cellWidth, 0, i * cellWidth, 8 * cellHeight);
            }
        }

        private void Repaint(object sender, PaintEventArgs e) // связка для вызова дроумеп
        {
            Graphics g = e.Graphics;
            DrawMap(g);
        }
    }
}
