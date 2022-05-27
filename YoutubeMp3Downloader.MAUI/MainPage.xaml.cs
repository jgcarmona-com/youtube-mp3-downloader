namespace YoutubeMp3Downloader.MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                ClickMeButton.Text = $"Clicked {count} time";
            else
                ClickMeButton.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(ClickMeButton.Text);
        }
    }
}