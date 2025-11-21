using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class EmptyFlowViewModel : IFlowViewModel
    {
        public IEnumerable<FlowContainerViewModel> Children => [];
    }
}
