using BaseClassLibrary;
using LocalTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControlLibrary.CascadeRelationChose
{
    public class DataApp : BaseModel
    {
        public delegate void Event_Handler();
        public event Event_Handler Event_ParameterChanged;
        public CascadeRelationDataResource dataResource= new CascadeRelationDataResource();
        private void OnParameterChanged()
        {
            if (Event_ParameterChanged != null)
            {
                Event_ParameterChanged();
            }
        }
        public DataApp()
        {
            dataResource.Event_ParameterChanged += OnParameterChanged;
        }
    }
}
