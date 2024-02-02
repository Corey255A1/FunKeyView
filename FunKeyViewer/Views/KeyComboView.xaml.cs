// WunderVision 2024
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FunKeyViewer.Views
{
    public partial class KeyComboView : UserControl
    {

        /// <summary>
        ///  Not actually binding to the story board...
        /// </summary>
        public double FadeOutSeconds
        {
            get { return (double)GetValue(FadeOutSecondsProperty); }
            set { SetValue(FadeOutSecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FadeOutSeconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FadeOutSecondsProperty =
            DependencyProperty.Register("FadeOutSeconds", typeof(double), typeof(KeyComboView), new PropertyMetadata(0.0));


        public bool FadeOut
        {
            get { return (bool)GetValue(FadeOutProperty); }
            set { SetValue(FadeOutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FadeOut.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FadeOutProperty =
            DependencyProperty.Register("FadeOut", typeof(bool), typeof(KeyComboView), new PropertyMetadata(false));


        public KeyComboView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (FadeOut)
            {
                BeginFadeOut();
            }
        }

        public void BeginFadeOut()
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.BeginTime = new TimeSpan(0);
            doubleAnimation.From = 1.0;
            doubleAnimation.To = 0.0;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(FadeOutSeconds));
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(UserControl.OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin(this);
        }
    }
}
