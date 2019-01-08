using System;
using System.Collections.Concurrent;
using Client.DataProviders;
using Client.HttpClinents;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class EditUserDetailsViewModel
    {
        private readonly IEditDetailsDataProvider dataProvider;

        public EditUserDetailsViewModel(IEditDetailsDataProvider dataProvider)
        {
            this.dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));

            InitializeUserDetails();
        }

        private void InitializeUserDetails()
        {
            UserDetails = new ObservableConcurrentDictionary<string, object>();
            var awaiter = dataProvider
                .GetUserDetails()
                .GetAwaiter();
            var source = awaiter.GetResult().Item2;

            foreach (var item in source)
                UserDetails.Add(item.Key, item.Value);
        }

        public EditUserDetailsViewModel() : this(new EditDetailsHttpClient()) { }

        public ObservableConcurrentDictionary<string, object> UserDetails { get; private set; }

        public void SaveChanges()
        {
            dataProvider.UpdateUserDetails(UserDetails);
        }
    }
}
