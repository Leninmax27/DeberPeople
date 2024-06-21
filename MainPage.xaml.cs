using PeopleEjercicio.Models;
using System.Collections.Generic;

namespace PeopleEjercicio
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        public async void OnNewButtonClicked(object sender, EventArgs args)
        {
            LCStatusMessage.Text = "";

            await App.PersonRepo.AddNewPerson(LCNewPerson.Text);
            LCStatusMessage.Text = App.PersonRepo.StatusMessage;
        }

        public async void OnGetButtonClicked(object sender, EventArgs args)
        {
            LCStatusMessage.Text = "";

            List<Person> people = await App.PersonRepo.GetAllPeople();
            LCPeopleList.ItemsSource = people;
        }
    }

}
