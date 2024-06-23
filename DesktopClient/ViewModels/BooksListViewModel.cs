using CommunityToolkit.Mvvm.ComponentModel;
using DesktopClient.Models;
using DesktopClient.Services;
using DesktopClient.Views;
using Microsoft.Extensions.DependencyInjection;
using PresentationDesktop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static System.Reflection.Metadata.BlobBuilder;

namespace DesktopClient.ViewModels
{
    public partial class BooksListViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private IServiceProvider serviceProvider;
        private INavigationParams navigationParams;
        private BookstoreApiService bookstoreApiService;

        [ObservableProperty]
        private List<DataColor> _colors = [];

        [ObservableProperty]
        private ObservableCollection<BookModel> _books = [];

        public BooksListViewModel(
            IServiceProvider serviceProvider, 
            INavigationParams navigationParams,
            BookstoreApiService bookstoreApiService)
        {
            this.serviceProvider = serviceProvider;
            this.navigationParams = navigationParams;
            this.bookstoreApiService = bookstoreApiService;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                InitializeViewModel();
            }

            try
            {
                Task.Run(() => LoadBooks());
            }
            catch (Exception ex)
            {
                // Log error
                Console.Write(ex.ToString());
            }
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        private async void LoadBooks()
        {
            try
            {
                var books = await bookstoreApiService.BooksAllAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Books.Clear();

                    foreach (var book in books)
                    {
                        Books.Add(new BookModel(book));
                    }
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnOpenErrorMessageBox().Wait();
                    Application.Current.Shutdown();
                });
            }
        }

        [RelayCommand]
        public void DeleteBook(object sender)
        {
            if (sender is BookModel book)
            {
                Task.Run(async () =>
                {
                    await bookstoreApiService.BooksDELETEAsync(book.Id);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var uiMessageBox = new Wpf.Ui.Controls.MessageBox
                        {
                            Title = "Atención",
                            Content = "Libro eliminado correctamente",
                        };

                        _ = uiMessageBox.ShowDialogAsync();
                    });

                    LoadBooks();
                });
            }
        }

        [RelayCommand]
        public void EditBook(object sender)
        {
            if (sender is BookModel book)
            {
                navigationParams.Set("BookToUpdate", book);
                var navigationWindow = (serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow)!;
                _ = navigationWindow.Navigate(typeof(DesktopClient.Views.Pages.EditBookPage));
            }
        }


        private async Task OnOpenErrorMessageBox()
        {
            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "Error",
                Content = "Ocurrió un error al intentar conectarse con el servidor",
            };

            _ = await uiMessageBox.ShowDialogAsync();
        }
    }
}
