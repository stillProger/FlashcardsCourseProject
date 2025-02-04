﻿using FlashcardsCourseProject.Models;
using FlashcardsCourseProject.Services;
using FlashcardsCourseProject.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FlashcardsCourseProject.ViewModels
{
    public class CardSetViewModel : BaseViewModel
    {
        private IDataStore<CardSet> CardSetDataStore => DependencyService.Get<IDataStore<CardSet>>();

        private CardSet _selectedItem;

        public ObservableCollection<CardSet> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<CardSet> CardSetTapped { get; }

        public CardSetViewModel()
        {
            Title = "Наборы карточек";
            Items = new ObservableCollection<CardSet>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            CardSetTapped = new Command<CardSet>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await CardSetDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public CardSet SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewCardSetPage));
        }

        async void OnItemSelected(CardSet cardSet)
        {
            if (cardSet == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(CardSetDetailPage)}?{nameof(CardSetDetailViewModel.ItemId)}={cardSet.Id}");
        }
    }
}
