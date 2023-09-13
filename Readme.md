<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128593690/23.1.1%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E1554)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:


* [Module.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Module/Module.cs)
* [NewViewFilterObjectController.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Module/Controllers/NewViewFilterObjectController.cs)
* [WinShowFilterDialogController.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Win/Controllers/WinShowFilterDialogController.cs) 
* [BlazorShowFilterDialogController.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Blazor.Server/Controllers/BlazorShowFilterDialogController.cs) 
* [ViewFilterContainer.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Module/BusinessObjects/ViewFilterContainer.cs) 
* [ViewFilterObject.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Module/BusinessObjects/ViewFilterObject.cs) 
<!-- default file list end -->
# How to show filter dialog before showing ListView


<p><strong>Scenario:</strong></p>
<p>When a user executes a navigation item that displays a large ListView, the application should display a popup window that allows you to define a filter for this ListView before loading records in it. This filter dialog should remember the user's choice and provide the capability to select one of the stored filters.</p>
<p><strong>Ste</strong><strong>ps to implement:</strong></p>
<p>1. Create a ListView model extender - <strong>IModelListViewAdditionalCriteria</strong> - that adds the AdditionalCriteria property to the ListView model to store the filter selected by the user.</p>
<p>2. Implement the <strong>ViewFilterContainer</strong> class whose DetailView is used as a filter dialog.</p>
<p>3. Implement the <strong>ViewFilterObject</strong> class which is used to store filters.</p>
<p>4. Implement the <strong>ShowFilterDialogController</strong> which shows the filter dialog instead of displaying the ListView, and then shows the filtered ListView. To do this, subscribe to the ShowNavigationItemController.ShowNavigationItemAction.Execute event and replace the ListView from the e.ShowViewParameters.CreatedView property with the ViewFilterContainer DetailView. Then show the filtered ListView using the Window.SetView method.
<p>5. Implement the <strong>NewViewFilterObjectController</strong> which sets the ObjectType property of the ViewFilterObject object created by the ViewFilterContainer.Filter lookup's New action.<br /><br /></p>

<strong>Blazor:</strong><br /><br />
We described Blazor specificites in the following Support Center ticket: <a href="https://supportcenter.devexpress.com/ticket/details/t1091183">How to show a filter dialog before showing a large ListView in Blazor</a>.</p>
<p> </p>
<p><strong>See Also:</strong><br /> <a href="http://documentation.devexpress.com/#Xaf/CustomDocument3014"><u>How to: Use Criteria Property Editors</u></a><br /> <a href="http://documentation.devexpress.com/#Xaf/CustomDocument2785"><u>How to: Extend the Application Model and Schema</u></a><br /> <a href="http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppSystemModuleShowNavigationItemControllertopic"><u>ShowNavigationItemController Class</u></a><br /> <a href="http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppSystemModuleDialogControllertopic"><u>Dialog Controller</u></a></p>

<br/>


