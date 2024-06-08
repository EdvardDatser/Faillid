using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Faillid
{
    public partial class Failide_Page : ContentPage
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public Failide_Page()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateFilesList();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string fileName = fileNameEntry.Text;
            if (String.IsNullOrEmpty(fileName)) { return; }
            if (File.Exists(Path.Combine(folderPath, fileName)))
            {
                bool isRewritten = await DisplayAlert("Kinitus", "Fail juba olemas. Kas tahad ümber kirjutada?", "jah", "ei");
                if (!isRewritten) { return; }
            }
            File.WriteAllText(Path.Combine(folderPath, fileName), textEditor.Text);
            UpdateFilesList();
        }

        private void UpdateFilesList()
        {
            FilesList.ItemsSource = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f)).ToList();
            FilesList.SelectedItem = null;
        }

        private void FilesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            string fileName = e.SelectedItem.ToString();
            textEditor.Text = File.ReadAllText(Path.Combine(folderPath, fileName));
            fileNameEntry.Text = fileName;
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            string fileName = (string)((MenuItem)sender).BindingContext;
            File.Delete(Path.Combine(folderPath, fileName));
            UpdateFilesList();
        }

        private void ToList_Clicked(object sender, EventArgs e)
        {
            string fileName = (string)((MenuItem)sender).BindingContext;
            List<string> list = File.ReadLines(Path.Combine(folderPath, fileName)).ToList();
            listFailist.ItemsSource = list;
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
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
                await DisplayAlert("Error", "Kustutamiseks ei ole valitud ühtegi faili.", "OK");
            }
        }
    }
}
