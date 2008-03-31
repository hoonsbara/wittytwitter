using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Witty
{
    public delegate void FadeOutFinishedDelegate(Popup p);
    public delegate void PopupReplyClickedDelegate(string screenName);
    public delegate void PopupDirectMessageClickedDelegate(string screenName);    

    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {        
        private MainWindow parent;
        private Storyboard sbFadeOut;
        private Storyboard ShowPopup;
        private TimeSpan ts = new TimeSpan();
        public event FadeOutFinishedDelegate FadeOutFinished;
        public event PopupReplyClickedDelegate ReplyClicked;
        public event PopupDirectMessageClickedDelegate DirectMessageClicked;        
        
        public Popup(MainWindow parent, Int32 numPopups, String heading, String body, String imageSource)
        {
            InitializeComponent();

            this.Left = SystemParameters.VirtualScreenWidth - this.Width;
            this.Top = SystemParameters.WorkArea.Height - (this.Height * (numPopups + 1));
            this.parent = parent;

            tweetText.Text = body;
            userName.Text = heading;

            ImageSourceConverter conv = new ImageSourceConverter();
            avatarImage.Source = (ImageSource)conv.ConvertFromString(imageSource);

            this.Topmost = true;

            sbFadeOut = (Storyboard)FindResource("sbFadeOut");
            sbFadeOut.Completed += new EventHandler(sbFadeOut_Completed);

            ShowPopup = (Storyboard)FindResource("ShowPopup");
            ShowPopup.Completed += new EventHandler(ShowPopup_Completed);
            ShowPopup.Begin(this, true);


        }

        void ShowPopup_Completed(object sender, EventArgs e)
        {
            sbFadeOut.Begin(this, true);
        }

        void sbFadeOut_Completed(object sender, EventArgs e)
        {
            if (FadeOutFinished != null)
                FadeOutFinished(this);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ts = (TimeSpan)sbFadeOut.GetCurrentTime(this);
                sbFadeOut.Stop(this);
                this.Opacity = 1;
            }
            catch (Exception ex)
            {
                App.Logger.Error("Popup SNARL Error", ex);
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            sbFadeOut.Begin(this, true);
            sbFadeOut.Seek(this, ts, TimeSeekOrigin.BeginTime);
        }

        private void ContextMenuReply_Click(object sender, RoutedEventArgs e)
        {
            ReplyClicked(userName.Text);
        }

        private void ContextMenuDirectMessageClick_Click(object sender, RoutedEventArgs e)
        {
            DirectMessageClicked(userName.Text);
        }        
    }
}
