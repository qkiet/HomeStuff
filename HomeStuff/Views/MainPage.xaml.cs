using CollectionViewDemos.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeStuff.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public class StringNullOrEmptyBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var s = value as string;
            return !string.IsNullOrWhiteSpace(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ParameterViewModel mydata = new ParameterViewModel();
            BindingContext = mydata;
        }

    }

    
}
