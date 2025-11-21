using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public interface IFlowViewModel
    {
        public IEnumerable<FlowContainerViewModel> Children { get; }
    }
}
