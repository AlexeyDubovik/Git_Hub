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
using System.Windows.Shapes;
using System.Media;

namespace Miner
{
    public partial class MainWindow : Window
    {
        private Random random;
        private bool close;
        public MainWindow()
        {
            InitializeComponent();
            close = false;
            random = new Random();
            for (int y = 0; y < App.SizeY; y++)
            {
                for (int x = 0; x < App.SizeX; x++)
                {
                    var mineLabel = new MineLabel
                    {
                        X = x,
                        Y = y,
                        FontSize = 30,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    mineLabel.MouseDown += MineLabel_MouseDown;
                    this.RegisterName("label_" + x + "_" + y, mineLabel);
                    Field.Children.Add(mineLabel);
                }
            }
            RestartGame();
        }
        void check_win()
        {
            int mine_count = 0;
            int tmp = 0;
            int opened_count = 1;
            int size = App.SizeX * App.SizeY;
            MineLabel label = null;
            foreach (var child in Field.Children)
            {
                label = child as MineLabel;
                if (label.IsMine == true)
                    mine_count++;
            }
            foreach (var child in Field.Children)
            {
                label = child as MineLabel;
                if (label.labelState == LabelState.Open)
                    opened_count++;
            }
            foreach (var child in Field.Children)
            {
                label = child as MineLabel;
                if (label.labelState == LabelState.Marked && label.IsMine == true)
                    tmp++;
                if (tmp == mine_count || size == opened_count + mine_count)
                {
                    if (close == true)
                        return;
                    if (MessageBoxResult.Yes == MessageBox.Show("Еще раз?", "Вы выграли", MessageBoxButton.YesNo))
                    {
                        RestartGame();
                        return;
                    }
                    else
                    {
                        close = true;
                        this.Close();
                    }
                }
            }
        }
        void openLabel(int x, int y, MouseButtonEventArgs e)
        {
            if (x < 0 || y < 0 || x >= App.SizeX || y >= App.SizeY)
                return;
            String[] names = {
                "label_" + (x-1) + "_" + (y-1),
                "label_" + (x-1) + "_" + y,
                "label_" + (x-1) + "_" + (y+1),
                "label_" + (x)   + "_" + (y-1),
                "label_" + (x)   + "_" + (y+1),
                "label_" + (x+1) + "_" + (y-1),
                "label_" + (x+1) + "_" + (y),
                "label_" + (x+1) + "_" + (y+1) };
            int mines = 0;
            MineLabel label = null;
            foreach (var child in Field.Children)
            {
                label = child as MineLabel;
                if (label.X == x && label.Y == y)
                    break;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (label.labelState == LabelState.Marked)
                {
                    label.Foreground = Brushes.Black;
                    label.labelState = LabelState.Unvisited;
                    label.Content = "\x2754";
                    check_win();
                    return;
                }
                else
                {
                    label.Foreground = Brushes.Red;
                    label.labelState = LabelState.Marked;
                    label.Content = "\x26FF";
                    check_win();
                    return;
                }
            }

            if (label.IsMine == true)
            {
                if (close == true)
                    return;
                label.Content = "\x2622";
                if (MessageBoxResult.Yes == MessageBox.Show("Еще раз?", "Игра провалена", MessageBoxButton.YesNo))
                {
                    RestartGame();
                    return;
                }
                else
                {
                    close = true;
                    this.Close();
                }
            }
            if (label.labelState == LabelState.Open)
                return;
            foreach (String name in names)
            {
                var mlabel = this.FindName(name) as MineLabel;
                if (mlabel != null)
                {
                    if (mlabel.IsMine)
                        mines++;
                }
            }
            if (mines == 1)
                label.Foreground = Brushes.Blue;
            else if (mines == 2)
                label.Foreground = Brushes.Green;
            else if (mines == 3)
                label.Foreground = Brushes.Red;
            else if (mines == 4)
                label.Foreground = Brushes.DarkBlue;
            else if (mines == 5)
                label.Foreground = Brushes.Brown;
            if (mines == 0)
                label.Content = " ";
            else
                label.Content = mines.ToString();
            label.labelState = LabelState.Open;
            if (label.IsMine == false)
            {
                foreach (var child in Field.Children)
                {
                    label = child as MineLabel;
                    if (label.X == x - 1 && label.Y == y - 1 && label.IsMine == false && mines == 0)
                        openLabel(x - 1, y - 1, e);
                    else if (label.X == x && label.Y == y - 1 && label.IsMine == false && mines == 0)
                        openLabel(x, y - 1, e);
                    else if (label.X == x + 1 && label.Y == y - 1 && label.IsMine == false && mines == 0)
                        openLabel(x + 1, y - 1, e);
                    else if (label.X == x - 1 && label.Y == y && label.IsMine == false && mines == 0)
                        openLabel(x - 1, y, e);
                    else if (label.X == x + 1 && label.Y == y && label.IsMine == false && mines == 0)
                        openLabel(x + 1, y, e);
                    else if (label.X == x - 1 && label.Y == y + 1 && label.IsMine == false && mines == 0)
                        openLabel(x - 1, y + 1, e);
                    else if (label.X == x && label.Y == y + 1 && label.IsMine == false && mines == 0)
                        openLabel(x, y + 1, e);
                    else if (label.X == x + 1 && label.Y == y + 1 && label.IsMine == false && mines == 0)
                        openLabel(x + 1, y + 1, e);
                }
            }
        }
        private void MineLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            check_win();
            var mineLabel = sender as MineLabel;
            if (mineLabel == null) return;
            openLabel(mineLabel.X, mineLabel.Y, e);
        }
        private void RestartGame()
        {
            foreach (var child in Field.Children)
            {
                MineLabel label = child as MineLabel;
                if (label != null)
                {
                    bool isMine = random.Next(5) == 0;
                    label.Foreground = Brushes.Black;
                    label.IsMine = isMine;
                    //label.Content = isMine ? "\x2764" : "\x2754";
                    label.Content = "\x2754";
                    label.labelState = LabelState.Unvisited;
                }
            }
        }
    }
    class MineLabel : Label
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsMine { get; set; }
        public LabelState labelState;
    }
    enum LabelState
    {
        Unvisited,
        Marked,
        Open
    }
}
