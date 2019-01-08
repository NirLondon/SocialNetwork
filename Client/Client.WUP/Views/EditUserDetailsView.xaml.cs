using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client.WUP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditUserDetailsView : Page
    {
        EditUserDetailsViewModel ViewModel { get; set; }

        public EditUserDetailsView()
        {
            ViewModel = new EditUserDetailsViewModel();
            InitializeNoneInitializedFields();

            this.InitializeComponent();
        }

        private void InitializeNoneInitializedFields()
        {
            var fields = new[] 
            {
                ("FirstName", typeof(string)),
                ("Lastname", typeof(string)),
                ("BirthDate", typeof(DateTimeOffset)),
                ("Job", typeof(string))
            };

            foreach (var field in fields)
            {
                if (!ViewModel.UserDetails.ContainsKey(field.Item1))
                    ViewModel.UserDetails.Add(field.Item1, GetDefault(field.Item2));
            }
        }

        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }
    }
}
