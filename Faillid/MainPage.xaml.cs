﻿namespace Faillid
{
    public partial class MainPage : ContentPage
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string fileName;
        string fileContent;
        int num = 0;
        Label editText, contentLabel;
        Frame frame;
        CarouselView carousel;
        Button openNewPageButton, openOneMoreNewPageButton;
        Image img;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateFilesList();

        }
        private void UpdateFilesList()
        {
            FilesList.ItemsSource = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));
            FilesList.SelectedItem = null;
        }
        private void ToList_Clicked(object sender, EventArgs e)
        {
            fileName = (string)((MenuItem)sender).BindingContext;
            fileContent = File.ReadAllText(Path.Combine(folderPath, fileName));
            listFailist.ItemsSource = File.ReadLines(Path.Combine(folderPath, fileName)).ToList();
        }
        private void FilesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            fileName = e.SelectedItem.ToString();
            fileContent = File.ReadAllText(Path.Combine(folderPath, fileName));
            editText.Text = fileContent;
            FilesList.SelectedItem = null;
        }
        public MainPage()
        {
            carousel = new CarouselView
            {
                VerticalOptions = LayoutOptions.Center,
            };
            List<string> fileNames = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f)).ToList();

            carousel.ItemsSource = fileNames.Select(name => new { Name = name, Content = File.ReadAllText(Path.Combine(folderPath, name)) });

            carousel.ItemTemplate = new DataTemplate(() =>
            {
                Label header = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 25,
                };
                header.SetBinding(Label.TextProperty, "Name");

                contentLabel = new Label()
                {
                    Opacity = 0,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 25,
                };
                img = new Image
                {
                    Source = "ez.png",
                    WidthRequest = 200,
                    HeightRequest = 200,
                };
                contentLabel.SetBinding(Label.TextProperty, new Binding("Content"));
                StackLayout stackLayout = new StackLayout()
                {
                    Children = { header, contentLabel, img }
                };


                frame = new Frame
                {
                    CornerRadius = 10,
                    Margin = 20,
                    WidthRequest = 300,
                    HeightRequest = 300,
                    Content = stackLayout,
                    BackgroundColor = Colors.LightBlue,
                };
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += Tap_Tapped1;
                frame.GestureRecognizers.Add(tap);
                return frame;
            });
            editText = new Label();
            FilesList = new ListView();
            listFailist = new ListView();

            openNewPageButton = new Button
            {
                Text = "Lisa kaart",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            openNewPageButton.Clicked += OpenNewPageButton_Clicked;

            openOneMoreNewPageButton = new Button
            {
                Text = "Friends",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            openOneMoreNewPageButton.Clicked += openOneMoreNewPageButton_Clicked;

            Content = new StackLayout
            {
                Children = { carousel, openNewPageButton, openOneMoreNewPageButton }
            };
        }
        private async void openOneMoreNewPageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DBListPage());

        }
        private async void OpenNewPageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Failide_Page());
        }
        private async void Tap_Tapped1(object sender, TappedEventArgs e)
        {
            Frame tappedFrame = (Frame)sender;
            num++;
            if ((num % 2) == 0)
            {
                ((Label)((StackLayout)((Frame)sender).Content).Children[1]).Opacity = 0;
                ((Label)((StackLayout)((Frame)sender).Content).Children[0]).Opacity = 1;
            }
            else
            {
                ((Label)((StackLayout)((Frame)sender).Content).Children[1]).Opacity = 1;
                ((Label)((StackLayout)((Frame)sender).Content).Children[0]).Opacity = 0;
            }
        }
        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (FilesList.SelectedItem != null)
            {
                string fileName = FilesList.SelectedItem.ToString();
                string filePath = Path.Combine(folderPath, fileName);
                File.Delete(filePath);
                UpdateFilesList();
            }
            else
            {
                DisplayAlert("Error", "No file selected for deletion.", "OK");
            }
        }
    }
}
