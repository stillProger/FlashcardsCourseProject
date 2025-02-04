using FlashcardsCourseProject.Models;
using FlashcardsCourseProject.Services;
using System;
using Xamarin.Forms;

namespace FlashcardsCourseProject.ViewModels
{
    public class NewCardSetViewModel : BaseViewModel
    {
        private IDataStore<CardSet> CardSetDataStore => DependencyService.Get<IDataStore<CardSet>>();

        private string name;
        private string picture;
        public NewCardSetViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(picture);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Picture
        {
            get => picture;
            set => SetProperty(ref picture, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            CardSet newItem = new CardSet()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Picture = Picture
            };

            await CardSetDataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
