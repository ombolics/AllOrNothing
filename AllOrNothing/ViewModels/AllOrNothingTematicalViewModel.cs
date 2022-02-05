using CommunityToolkit.Mvvm.ComponentModel;
using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingTematicalViewModel : ObservableRecipient
    {

        #region Singleton pattern

        private static AllOrNothingTematicalViewModel _instance = null;
        private static readonly object padlock = new object();

        public int MyProperty { get; set; }

        private AllOrNothingTematicalViewModel()
        {
        }

        public static AllOrNothingTematicalViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AllOrNothingTematicalViewModel();
                    }
                    return _instance;
                }
            }
        }
        #endregion 


        public QuestionSerie Serie => null;//QuestionSerieDummyData.QS1;

        public void ResetReachablePages()
        {

        }
    }
}
