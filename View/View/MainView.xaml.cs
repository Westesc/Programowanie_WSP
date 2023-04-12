using System.Windows;

namespace TPW.Presentation.View {
    public partial class MainView {
        public MainView() {
            InitializeComponent();
        }

        // THIS NEEDS TO BE IN VIEWMODEL INSTEAD !
        //private void StartButtonClick(object sender, RoutedEventArgs e) {
        //    StartBtn.IsEnabled = false;
        //    StopBtn.IsEnabled = true;
        //}
        //
        //private void StopButtonClick(object sender, RoutedEventArgs e) {
        //    StartBtn.IsEnabled = true;
        //    StopBtn.IsEnabled = false;
        //}
    }
}