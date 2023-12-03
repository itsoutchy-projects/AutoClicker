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
using System.Runtime.InteropServices;
using Input.Simulator;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Windows.Threading;

namespace AutoClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SetIntervalDialog intervalDialog = new SetIntervalDialog();
        public DispatcherTimer timer;
        /// <summary>
        /// Gets the current mouse position on screen
        /// </summary>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(this);

            // Add the window position
            return new Point(position.X + Left, position.Y + Top);
        }

        public Point clickPos = new Point();

        public bool mouseDown
        {
            get
            {
                return System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed;
            }
        }
        public bool clicking = false;

        public bool GetMouseDown()
        {
            return mouseDown;
        }

        public bool GetMouseUp()
        {
            return !mouseDown;
        }

        public MainWindow()
        {
            InitializeComponent();
            pickPosBttn.Click += PickPosBttn_Click;
            startBttn.Click += StartBttn_Click;
            IntervalBttn.Click += IntervalBttn_Click;
            stopBttn.Click += StopBttn_Click;
            curPosRadio.Click += CurPosRadio_Click;
            InfoBttn.Click += InfoBttn_Click;
        }

        private void InfoBttn_Click(object sender, RoutedEventArgs e)
        {
            InfoDialog infoDialog = new InfoDialog();
            infoDialog.ShowDialog();
        }

        private void CurPosRadio_Click(object sender, RoutedEventArgs e)
        {
            if (curPosRadio.IsEnabled)
            {
                posText.Content = "X: current, Y: current";
            }
        }

        private void StopBttn_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        public void Stop()
        {
            IntervalBttn.Dispatcher.Invoke(new Action(() => {
                IntervalBttn.IsEnabled = true;
            }));

            startBttn.Dispatcher.Invoke(new Action(() =>
            {
                startBttn.IsEnabled = true;
            }));

            pickPosRadio.Dispatcher.Invoke(new Action(() =>
            {
                pickPosRadio.IsEnabled = true;
            }));

            curPosRadio.Dispatcher.Invoke(new Action(() =>
            {
                curPosRadio.IsEnabled = true;
            }));

            clicking = false;
        }

        private void IntervalBttn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetIntervalDialog setIntervalDialog = new SetIntervalDialog();
                setIntervalDialog.hoursField.Text = intervalDialog.hoursField.Text;
                setIntervalDialog.minutesField.Text = intervalDialog.minutesField.Text;
                setIntervalDialog.secondsField.Text = intervalDialog.secondsField.Text;
                setIntervalDialog.millisecondsField.Text = intervalDialog.millisecondsField.Text;

                setIntervalDialog.Show();
                intervalDialog = setIntervalDialog;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ", " + ex.Source, "Fatal error");
            }
        }

        private void StartBttn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //timer = new Timer(TimeEx.GetMilliseconds(intervalDialog.hours, intervalDialog.minutes, intervalDialog.seconds, intervalDialog.milliseconds));
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(TimeEx.GetMilliseconds(intervalDialog.hours, intervalDialog.minutes, intervalDialog.seconds, intervalDialog.milliseconds));
                clicking = true;
                IntervalBttn.Dispatcher.Invoke(new Action(() =>
                {
                    IntervalBttn.IsEnabled = false;
                }));

                startBttn.Dispatcher.Invoke(new Action(() =>
                {
                    startBttn.IsEnabled = false;
                }));

                pickPosRadio.Dispatcher.Invoke(new Action(() =>
                {
                    pickPosRadio.IsEnabled = false;
                }));

                curPosRadio.Dispatcher.Invoke(new Action(() =>
                {
                    curPosRadio.IsEnabled = false;
                }));

                timer.Start();
                timer.Tick += Timer_Elapsed;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ", " + ex.Source, "Fatal error");
            }
        }

        private void Timer_Elapsed(object? sender, EventArgs e)
        {
            try
            {
                if (clicking)
                {
                    if (Simulator.IsKeyPressed(Simulator.VirtualKeyStates.VK_CONTROL) && Simulator.IsKeyPressed(Simulator.VirtualKeyStates.VK_MENU) && Simulator.IsKeyPressed(Simulator.VirtualKeyStates.VK_D))
                    {
                        Stop();
                        timer.Stop();
                        return;
                    }
                    if (pickPosRadio.IsChecked == true)
                    {
                        Simulator.SetMousePos((int)clickPos.X, (int)clickPos.Y);
                        LeftClick(0, 0);
                    }
                    else if (curPosRadio.IsChecked == true)
                    {
                        LeftClick((int)GetMousePosition().X, (int)GetMousePosition().Y);
                    }
                }
                else
                {
                    timer.Stop();
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ", " + ex.Source, "Fatal error");
            }
        }

        private async void PickPosBttn_Click(object sender, RoutedEventArgs e)
        {
            posText.Content = "Release the left mouse button";
            await TaskEx.WaitUntil(new Func<bool>(GetMouseUp));
            posText.Content = "Please wait...";
            await Task.Delay(1000);
            posText.Content = "Click to pick position";
            await TaskEx.WaitUntil(new Func<bool>(GetMouseDown));
            clickPos = GetMousePosition();
            posText.Content = $"X: {clickPos.X}, Y: {clickPos.Y}";
        }

        public async void LeftClick(int x, int y)
        {
            Simulator.Mouse(Simulator.MouseEventFlags.LEFTDOWN, x, y, Simulator.MouseEventDataXButtons.XBUTTON1, new UIntPtr());
            await Task.Delay(100);
            Simulator.Mouse(Simulator.MouseEventFlags.LEFTUP, x, y, Simulator.MouseEventDataXButtons.XBUTTON1, new UIntPtr());
        }
    }
}
