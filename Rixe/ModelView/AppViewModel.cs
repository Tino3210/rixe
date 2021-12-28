using Rixe.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rixe.ModelView
{
    class AppViewModel : BaseViewModel
    {
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;


        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        private void OnGoMainScreen(object obj)
        {
            ChangeViewModel(PageViewModels[0]);
        }

        private void OnGoGameScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
        }

        public AppViewModel()
        {
            // Add available pages and set page
            PageViewModels.Add(new MainMenu());
            PageViewModels.Add(new Game());

            CurrentPageViewModel = PageViewModels[0];

            Mediator.Subscribe("GoToMenuScreen", OnGoMainScreen);
            Mediator.Subscribe("GoToGameScreen", OnGoGameScreen);
        }
    }
}
