using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WpfAppComboBox.ViewModel.Simplified;
using System.Windows;
using System.Threading;
using System.Collections;

namespace WpfAppComboBox.ViewModel
{
    internal class MainWindowViewModel : BaseInpc
    {
        public const int AlwaysShowValueInComboBox = 2; 
        private RelayCommand? initComboBox;
        public RelayCommand InitComboBox => initComboBox
            ??= new RelayCommand(() =>
            {
                //SItems = new(sourseData);
                //IndexSItems = findIndex(AlwaysShowValueInComboBox, sourseData);
                SItems.Clear();
                Array.ForEach(sourseData, i => SItems.Add(i));
                IndexSItems = findIndex(AlwaysShowValueInComboBox, sourseData);
            });

        private RelayCommand? twoQuickChanges;
        public RelayCommand TwoQuickChanges => twoQuickChanges
           ??= new RelayCommand(() =>
           {
               IndexSItems = 0;
               Changed(-5, true);
              // IndexSItems = 0;
               Changed(-5, false);
           });


        //private ObservableCollection<int> sItems = new();
        //public ObservableCollection<int> SItems
        //{
        //    get => sItems;
        //    set => Set(ref sItems, value);
        //}
        public ObservableCollection<int> SItems { get; } = new();

        private int indexSItems = -1;
        public int IndexSItems
        {
            get => indexSItems;
            set => Set(ref indexSItems, value);
        }

        private int[] sourseData = new int[3] { 0, 1, 2 };

        /// <summary> Ищет значение первого индекса в коллекции для значение, для упрощения -  значение всегда есть в коллекции</summary>
        private int findIndex(int value, IEnumerable<int> sourse) => sourse.TakeWhile(x => x != value).Count();

        /// <summary>Добавляет или удаляет value в ComboBox, отсортировав при этот список. При этом отображаемое значение не меняется </summary>        
        private void Changed(int value, bool isAdded)
        { // всё искусственно и надумано, но логика метода не нарушина.
            lock (((ICollection)SItems).SyncRoot)
            {
                //List<int> CurrentList = new(SItems);
                //if (isAdded)
                //{
                //    CurrentList.Add(value);
                //}
                //else
                //{
                //    var k = findIndex(value, CurrentList);
                //    CurrentList.RemoveAt(findIndex(value, CurrentList));
                //}
                //CurrentList.Sort();
                //var newIndex = findIndex(AlwaysShowValueInComboBox, CurrentList);
                //SItems.Clear();
                //for (int i = 0; i < CurrentList.Count; i++)
                //{
                //    SItems.Add(CurrentList[i]);
                //}
                //// DoEvents();
                //IndexSItems = newIndex;

                List<int> CurrentList = new(SItems);
                if (isAdded)
                {
                    CurrentList.Add(value);
                }
                else
                {
                    var k = findIndex(value, CurrentList);
                    CurrentList.RemoveAt(findIndex(value, CurrentList));
                }
                CurrentList.Sort();
                var newIndex = findIndex(AlwaysShowValueInComboBox, CurrentList);
                SItems.Clear();
                for (int i = 0; i < CurrentList.Count; i++)
                {
                    SItems.Add(CurrentList[i]);
                }
                IndexSItems = newIndex;
            }
        }
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }

        public async void DoEvents2()
        {
            var task = new Task(() => Thread.Sleep(10));
            task.Start();
            await task;
        }

    }
}
