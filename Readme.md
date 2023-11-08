<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128593690/23.1.1%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E1554)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->



# How to show filter dialog before showing ListView

When a user executes a navigation item that displays a large ListView, the application should display a popup window that allows you to define a filter for this ListView before loading records in it. This filter dialog should remember the user's choice and provide the capability to select one of the stored filters.

![chrome_xTdZReiKm4](https://github.com/DevExpress-Examples/XAF_how-to-show-filter-dialog-before-showing-listview-e1554/assets/14300209/ebfa62d4-f4b7-489e-94f1-3c76d691375f)

## Implementation Details

1. Create a ListView model extender - IModelListViewAdditionalCriteria - that adds the AdditionalCriteria property to the ListView model to store the filter selected by the user.

2. Implement the non persistent ViewFilterContainer class whose DetailView is used as a filter dialog.

3. Implement the persistent ViewFilterObject class which is used to store filters.

4. Implement the ShowFilterDialogController which shows the filter dialog instead of displaying the ListView, and then shows the filtered ListView. To do this, subscribe to the ShowNavigationItemController.ShowNavigationItemAction.Execute event and replace the ListView from the e.ShowViewParameters.CreatedView property with the ViewFilterContainer DetailView. Then show the filtered ListView using the Window.SetView method.

5. Implement the NewViewFilterObjectController which sets the ObjectType property of the ViewFilterObject object created by the ViewFilterContainer.Filter lookup's New action.


## Files to Review


* [Module.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Module/Module.cs)
* [NewViewFilterObjectController.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Module/Controllers/NewViewFilterObjectController.cs)
* [WinShowFilterDialogController.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Win/Controllers/WinShowFilterDialogController.cs) 
* [BlazorShowFilterDialogController.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Blazor.Server/Controllers/BlazorShowFilterDialogController.cs) 
* [ViewFilterContainer.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Module/BusinessObjects/ViewFilterContainer.cs) 
* [ViewFilterObject.cs](CS/EFCore/DialogBeforeListViewEF/DialogBeforeListViewEF.Module/BusinessObjects/ViewFilterObject.cs) 

## Documentation

- [Non-Persistent Objects](https://docs.devexpress.com/eXpressAppFramework/116516/business-model-design-orm/non-persistent-objects)
- [Data Types Supported by built-in Editors](https://docs.devexpress.com/eXpressAppFramework/113014/business-model-design-orm/data-types-supported-by-built-in-editors)
- [How to: Extend and Access the Application Model Nodes from Controllers](https://docs.devexpress.com/eXpressAppFramework/112785/ui-construction/application-model-ui-settings-storage/customize-application-model-in-code/how-to-extend-the-application-model-nodes-from-controllers)
- [ShowNavigationItemController class](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.SystemModule.ShowNavigationItemController)
- [DialogController class](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.SystemModule.DialogController)
