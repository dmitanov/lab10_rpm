using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using lab10_rpm.Models;
using lab10_rpm.Services;

namespace lab10_rpm.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;

        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contact? _selectedContact;

        public ObservableCollection<Contact> Contacts { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (Set(ref _name, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                if (Set(ref _phone, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public Contact? SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (Set(ref _selectedContact, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public ICommand AddCommand { get; }

        public ICommand DeleteCommand { get; }

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService
                ?? throw new ArgumentNullException(nameof(dialogService));

            Contacts = new ObservableCollection<Contact>();

            AddCommand = new RelayCommand(AddContact, CanAddContact);

            DeleteCommand = new RelayCommand<Contact>(
                DeleteContact,
                CanDeleteContact);

            Contacts.Add(new Contact("Иван Кузнецов", "+79991234567"));
            Contacts.Add(new Contact("Артем Пиунов", "9123456789"));
        }

        private void AddContact()
        {
            string trimmedPhone = Phone.Trim();

            if (Contacts.Any(c => c.Phone == trimmedPhone))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!");
                return;
            }

            try
            {
                Contact newContact = new Contact(Name, Phone);
                Contacts.Add(newContact);

                _dialogService.ShowInfo("Контакт успешно добавлен.");

                Name = string.Empty;
                Phone = string.Empty;
            }
            catch (ArgumentException ex)
            {
                _dialogService.ShowError(ex.Message);
            }
            catch (Exception)
            {
                _dialogService.ShowError("Не удалось добавить контакт.");
            }
        }

        private bool CanAddContact()
        {
            return Contact.IsValidName(Name) && Contact.IsValidPhone(Phone);
        }

        private void DeleteContact(Contact? contact)
        {
            if (contact == null)
                return;

            bool confirmed = _dialogService.ShowConfirmation(
                $"Удалить контакт \"{contact.Name}\"?");

            if (!confirmed)
                return;

            Contacts.Remove(contact);

            if (SelectedContact == contact)
                SelectedContact = null;
        }

        private bool CanDeleteContact(Contact? contact)
        {
            return contact != null;
        }
    }
}

