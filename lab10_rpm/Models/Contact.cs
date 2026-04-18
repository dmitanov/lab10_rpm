using System;
using System.Text.RegularExpressions;
using lab10_rpm.ViewModels;

namespace lab10_rpm.Models
{
    public class Contact : ObservableObject
    {
        private string _name = string.Empty;
        private string _phone = string.Empty;

        public Contact(string name, string phone)
        {
            _name = name?.Trim() ?? string.Empty;
            _phone = phone?.Trim() ?? string.Empty;

            if (!Validate())
                throw new ArgumentException("Некорректные данные контакта.");
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value?.Trim() ?? string.Empty);
        }

        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value?.Trim() ?? string.Empty);
        }

        public bool Validate()
        {
            return IsValidName(Name) && IsValidPhone(Phone);
        }

        public static bool IsValidName(string? name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public static bool IsValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return Regex.IsMatch(phone.Trim(), @"^(\+7\d{10}|\d{10})$");
        }
    }
}
