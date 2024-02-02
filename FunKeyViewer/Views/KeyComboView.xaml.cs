// WunderVision 2024
using System.Windows;
using System.Windows.Controls;

namespace FunKeyViewer.Views
{
    public partial class KeyComboView : UserControl
    {

        /// <summary>
        ///  Not actually binding to the story board...
        /// </summary>
        public int FadeOutSeconds
        {
            get { return (int)GetValue(FadeOutSecondsProperty); }
            set { SetValue(FadeOutSecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FadeOutSeconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FadeOutSecondsProperty =
            DependencyProperty.Register("FadeOutSeconds", typeof(int), typeof(KeyComboView), new PropertyMetadata(0));



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
        }
    }
}
