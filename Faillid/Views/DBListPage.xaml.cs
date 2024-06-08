using Microsoft.Maui.Controls;
using System;
using System.Linq;
using Faillid.Models;

namespace Faillid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DBListPage : ContentPage
    {
        public DBListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            friendsList.ItemsSource = App.Database.GetItems();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Friend selectedFriend = (Friend)e.SelectedItem;
            if (selectedFriend == null)
                return;

            DBFriendPage friendPage = new DBFriendPage();
            friendPage.BindingContext = selectedFriend;
            await Navigation.PushAsync(friendPage);

            // Deselect item
            friendsList.SelectedItem = null;
        }

        private async void CreateFriend(object sender, EventArgs e)
        {
            Friend friend = new Friend();
            DBFriendPage friendPage = new DBFriendPage();
            friendPage.BindingContext = friend;
            await Navigation.PushAsync(friendPage);
        }
    }
}
