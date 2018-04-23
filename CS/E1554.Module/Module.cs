using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using System.Reflection;
using DevExpress.ExpressApp.Model;
using System.ComponentModel;


namespace E1554.Module {
    public sealed partial class E1554Module : ModuleBase {
        public E1554Module() {
            InitializeComponent();
        }
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelListView, IModelListViewAdditionalCriteria>();
        }
    }
    public interface IModelListViewAdditionalCriteria : IModelNode {
        [DefaultValue("")]
        string AdditionalCriteria { get; set; }
    }
}
