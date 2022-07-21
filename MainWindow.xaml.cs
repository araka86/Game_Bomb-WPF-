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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BombGame
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            bombTimer.Tick += bombTimer_Tick;
        }
        //прямоугольник отсечения
        private void canvasBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectangleGeometry rect = new RectangleGeometry();
            rect.Rect = new Rect(0, 0, canvasBackground.ActualWidth, canvasBackground.ActualHeight); //размеры по х=100, y=0 ширины и высоты
            canvasBackground.Clip = rect;
        }
        // Запускает события в потоке пользовательского интерфейса.
        private DispatcherTimer bombTimer = new DispatcherTimer();






        // Изначально бомбы падают каждые 1,3 секунды достигая земли за 3,5 секунды.
        private double initialSecondsBetweenBombs = 1.3;
        private double initialSecondsToFall = 3.5;
        private double secondsBetweenBombs;
        private double secondsToFall;
        // Счетчики сброшенных и перехваченных бомб
        private int droppedCount = 0;
        private int savedCount = 0;

        // Старт игры
        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            cmdStart.IsEnabled = false;

            // Сброс игры
            droppedCount = 0;
            savedCount = 0;
            secondsBetweenBombs = initialSecondsBetweenBombs;
            secondsToFall = initialSecondsToFall;

            // Запуск Таймера бомб            
            bombTimer.Interval = TimeSpan.FromSeconds(secondsBetweenBombs);
            bombTimer.Start();
        }

       

        // Позволяет находить раскадровку по бомбе
        private Dictionary<Bomb, Storyboard> storyboards = new Dictionary<Bomb, Storyboard>();



        // Вносим поправки каждые 15 сек.
        private double secondsBetweenAdjustments = 15;
        private DateTime lastAdjustmentTime = DateTime.MinValue;
        // При каждой поправке вычитать по 0.1 сек. из обоих значений
        private double secondsBetweenBombsReduction = 0.1;
        private double secondsToFallReduction = 0.1;


        //Сброс бомб.
        private void bombTimer_Tick(object sender, EventArgs e)
        {
            // При необходимомти внести поправку
            if ((DateTime.Now.Subtract(lastAdjustmentTime).TotalSeconds >
                secondsBetweenAdjustments))
            {
                lastAdjustmentTime = DateTime.Now;

                secondsBetweenBombs -= secondsBetweenBombsReduction;
                secondsToFall -= secondsToFallReduction;

                // Формально необходимо предпринимать проверку на 0 или отрицательное значение
                // однако на практике этого не произойдет, поскольку игра всегда закончится раньше.

                // Установить таймер для сброса следующей бомы в соответстующее время
                bombTimer.Interval = TimeSpan.FromSeconds(secondsBetweenBombs);
                bombTimer.Interval = TimeSpan.FromSeconds(secondsToFall);

                // Обновить сообщение о состоянии
                lblRate.Text = String.Format("Сл.бомба сбрасивается через {0} секунд.", secondsBetweenBombs);
                lblSpeed.Text = String.Format("каждая бомба летит {0}.", secondsToFall);
            }

            // Создать бомбу
            Bomb bomb = new Bomb();
            bomb.IsFalling = true;

            // Позиционирование            
            Random random = new Random();
            bomb.SetValue(Canvas.LeftProperty,
                (double)random.Next(0, (int)canvasBackground.ActualWidth - 100)); // позиционирование ширины (старт)
            bomb.SetValue(Canvas.TopProperty, -100.0); // позиционирование высоты (старт)

            // присоеденить собитие щелчка мыши (для перехвата бомб)
            bomb.MouseLeftButtonDown += bomb_MouseLeftButtonDown;

            // Создать анимацию для падающей бомбы.
            Storyboard storyboard = new Storyboard();
            DoubleAnimation fallAnimation = new DoubleAnimation();
            fallAnimation.To = canvasBackground.ActualHeight;
            fallAnimation.Duration = TimeSpan.FromSeconds(secondsToFall);

            Storyboard.SetTarget(fallAnimation, bomb);
            Storyboard.SetTargetProperty(fallAnimation, new PropertyPath("(Canvas.Top)"));
            // Добавить падающую бомбу на Canvas 
            storyboard.Children.Add(fallAnimation);

            // Создать анимацию для раскачивания бомбы.
            DoubleAnimation wiggleAnimation = new DoubleAnimation();
            wiggleAnimation.To = random.Next(0, (int)canvasBackground.ActualWidth - 300);
            wiggleAnimation.Duration = TimeSpan.FromSeconds(1.8);
            wiggleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            wiggleAnimation.AutoReverse = true;

            Storyboard.SetTarget(wiggleAnimation, bomb);
            Storyboard.SetTargetProperty(wiggleAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(wiggleAnimation);

            // Добавить раскачанную бомбу на Canvas 
            canvasBackground.Children.Add(bomb);

            // Добавление раскадровки в коллекцию (Dictonary)
            storyboards.Add(bomb, storyboard);

            // конфигурация и старт раскадровки.
            storyboard.Duration = fallAnimation.Duration;
            storyboard.Completed += storyboard_Completed;
            storyboard.Begin();
        }
       
        //перехват бомбы
        private void bomb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // получить бомбу
            Bomb bomb = (Bomb)sender;
            bomb.IsFalling = false;

            // Получить текущую позицию (положение) бомбы
            Storyboard storyboard = storyboards[bomb];
            //запомнить(получить) ее текущую (анимированую) позицию
            double currentTop = Canvas.GetTop(bomb);

            // Остановить падение бомбы
            storyboard.Stop();

            // Повторно запустить раскадровку, но с новыми анимациями
            // Запустить бомбу по новой траектории анимируя Canvas.Top and Canvas.Left.
            storyboard.Children.Clear(); //очистить

            DoubleAnimation riseAnimation = new DoubleAnimation();
            riseAnimation.From = currentTop;
            riseAnimation.To = 0;
            riseAnimation.Duration = TimeSpan.FromSeconds(1);

            Storyboard.SetTarget(riseAnimation, bomb);
            Storyboard.SetTargetProperty(riseAnimation, new PropertyPath("(Canvas.Top)")); //для свойства  Top
            storyboard.Children.Add(riseAnimation);

            DoubleAnimation slideAnimation = new DoubleAnimation();
            double currentLeft = Canvas.GetLeft(bomb); //получить  позицию левого края
            // Выбросить бомбу за ближайшый край поля
            if (currentLeft < canvasBackground.ActualWidth / 2)
            {
                slideAnimation.To = -10;
            }
            else
            {
                slideAnimation.To = canvasBackground.ActualWidth + 10;
            }
            slideAnimation.Duration = TimeSpan.FromSeconds(1.1); //скорость отбрасывания
            Storyboard.SetTarget(slideAnimation, bomb);
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(Canvas.Left)")); //для свойства  Left
            storyboard.Children.Add(slideAnimation);

            // Запустить новую анимацию
            storyboard.Duration = slideAnimation.Duration;
            storyboard.Begin();
        }



        // Условие для окончания игры = 5 пропущенных бомб
        private int maxDropped = 5;

        private void storyboard_Completed(object sender, EventArgs e)
        {
            ClockGroup clockGroup = (ClockGroup)sender;

            // получить первую анимацию в раскадровке и воспользоватся ею для нахождения анимированной бомбы
            DoubleAnimation completedAnimation = (DoubleAnimation)clockGroup.Children[0].Timeline;
            Bomb completedBomb = (Bomb)Storyboard.GetTarget(completedAnimation);

            // Определить, упала бомба или отбита за пределы Canvas в результате щелчка
            if (completedBomb.IsFalling)
            {
                droppedCount++; 
            }
            else
            {
                savedCount++;
            }

            // Обновить отображение
            lblStatus.Text = String.Format("упавшых бомб {0} \n сбитые бомбы {1}.", droppedCount, savedCount);

            // Проверка условия завершения игры
            if (droppedCount >= maxDropped)
            {
                bombTimer.Stop();
                lblStatus.Text += "\r\n\r\nGame over.";

                // Найти все действующие раскадровки
                foreach (KeyValuePair<Bomb, Storyboard> item in storyboards)
                {
                    Storyboard storyboard = item.Value;
                    Bomb bomb = item.Key;

                    storyboard.Stop();
                    canvasBackground.Children.Remove(bomb);
                }
                // Очистить коллекцию раскадровок
                storyboards.Clear();

                // Разрешить пользователю начать игру заново
                cmdStart.IsEnabled = true;
            }
            else
            {
                Storyboard storyboard = (Storyboard)clockGroup.Timeline;
                storyboard.Stop();

                storyboards.Remove(completedBomb);
                canvasBackground.Children.Remove(completedBomb);
            }
        }

        private void pause_click(object sender, RoutedEventArgs e)
        {
            bombTimer.Stop(); //остановит таймер сброса бомб
            storyboards.Clear(); //очистить раскадровку
            // Сброс игры
            droppedCount = 0;
            savedCount = 0;
            secondsBetweenBombs = initialSecondsBetweenBombs;
            secondsToFall = initialSecondsToFall;
            // Обновить отображение
            lblStatus.Text = String.Format("упавшых бомб {0} сбитые бомбы {1}.", droppedCount, savedCount);
            cmdStart.IsEnabled = true;  // Разрешить пользователю начать игру заново
        }

    }
}



