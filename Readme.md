<!-- default file list -->
*Files to look at*:

* [WinModule.cs](./CS/E1554.Module.Win/WinModule.cs) (VB: [WinModule.vb](./VB/E1554.Module.Win/WinModule.vb))
* [WinShowFilterDialogController.cs](./CS/E1554.Module.Win/WinShowFilterDialogController.cs) (VB: [WinShowFilterDialogController.vb](./VB/E1554.Module.Win/WinShowFilterDialogController.vb))
* [Module.cs](./CS/E1554.Module/Module.cs)
* [NewViewFilterObjectController.cs](./CS/E1554.Module/NewViewFilterObjectController.cs) (VB: [NewViewFilterObjectController.vb](./VB/E1554.Module/NewViewFilterObjectController.vb))
* **[ShowFilterDialogController.cs](./CS/E1554.Module/ShowFilterDialogController.cs) (VB: [ShowFilterDialogController.vb](./VB/E1554.Module/ShowFilterDialogController.vb))**
* [ViewFilterContainer.cs](./CS/E1554.Module/ViewFilterContainer.cs) (VB: [ViewFilterContainer.vb](./VB/E1554.Module/ViewFilterContainer.vb))
* [ViewFilterObject.cs](./CS/E1554.Module/ViewFilterObject.cs) (VB: [ViewFilterObject.vb](./VB/E1554.Module/ViewFilterObject.vb))
<!-- default file list end -->
# How to show filter dialog before showing ListView


<p><strong>Scenario:</strong></p>
<p>When a user executes a navigation item that displays a large ListView, the application should display a popup window that allows you to define a filter for this ListView before loading records in it. This filter dialog should remember the user's choice and provide the capability to select one of the stored filters.</p>
<p><strong>Ste</strong><strong>ps to implement:</strong></p>
<p>1. Create a ListView model extender - <strong>IModelListViewExt</strong> - that adds the AdditionalCriteria property to the ListView model to store the filter selected by the user.</p>
<p>2. Implement the <strong>ViewFilterContainer</strong> class whose DetailView is used as a filter dialog.</p>
<p>3. Implement the <strong>ViewFilterObject</strong> class which is used to store filters.</p>
<p>4. Implement the <strong>ShowFilterDialogController</strong> which shows the filter dialog instead of displaying the ListView, and then shows the filtered ListView. To do this, subscribe to the ShowNavigationItemController.ShowNavigationItemAction.Execute event and replace the ListView from the e.ShowViewParameters.CreatedView property with the ViewFilterContainer DetailView. Then show the filtered ListView using the Window.SetView method for WinForms SDI and Web applications or by setting the ShowViewParameters.CreatedView property for WinForms MDI applications.</p>
<p>5. Implement the <strong>NewViewFilterObjectController</strong> which sets the ObjectType property of the ViewFilterObject object created by the ViewFilterContainer.Filter lookup's New action.<br /><br /></p>
<p><strong>ASP.NET:</strong><br /><br />6. Since the specified filters are stored in the AdditionalCriteria model property, it is necessary to save the value of this property between sessions. For this purpose, enable the database model differences store, as described in the <a href="https://documentation.devexpress.com/#Xaf/CustomDocument3698">How to: Store the Application Model Differences in the Database</a> topic.<br /><br /><strong>WinForms:</strong><br /><br />7. If your application uses <strong>MDI</strong> mode, note that when a View is already opened in an inactive tab, and you are opening it using the navigation panel again, the newly created View will not be used. Instead of this, the existing window will be activated, and your filter will be ignored. So, you will need to write additional code to filter the View displayed in this window. In this example, this is implemented through a custom Show View Strategy. See code from the <strong>WinShowFilterDialogController.cs</strong> and <strong>WinModule.cs</strong> files.</p>
<p> </p>
<p><strong>See Also:</strong><br /> <a href="http://documentation.devexpress.com/#Xaf/CustomDocument3014"><u>How to: Use Criteria Property Editors</u></a><br /> <a href="http://documentation.devexpress.com/#Xaf/CustomDocument2785"><u>How to: Extend the Application Model and Schema</u></a><br /> <a href="http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppSystemModuleShowNavigationItemControllertopic"><u>ShowNavigationItemController Class</u></a><br /> <a href="http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppSystemModuleDialogControllertopic"><u>Dialog Controller</u></a></p>

<br/>


