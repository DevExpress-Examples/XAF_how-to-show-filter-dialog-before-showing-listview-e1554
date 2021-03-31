Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.SystemModule
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.Data.Filtering

Namespace E1554.Module
	Public Class ShowFilterDialogController
		Inherits WindowController

		Public Sub New()
			TargetWindowType = WindowType.Main
		End Sub

		Private showNavigationItemController As ShowNavigationItemController
		Private oldListView As ListView

		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			showNavigationItemController = Frame.GetController(Of ShowNavigationItemController)()
			If showNavigationItemController IsNot Nothing Then
				AddHandler showNavigationItemController.CustomShowNavigationItem, AddressOf ShowNavigationItemController_CustomShowNavigationItem
			End If
		End Sub
		Private Sub ShowNavigationItemController_CustomShowNavigationItem(ByVal sender As Object, ByVal e As CustomShowNavigationItemEventArgs)
			Dim shortcut As ViewShortcut = TryCast(e.ActionArguments.SelectedChoiceActionItem.Data, ViewShortcut)
			If shortcut IsNot Nothing Then
				oldListView = TryCast(Application.ProcessShortcut(shortcut), ListView)
				If oldListView IsNot Nothing Then
					e.Handled = True
					Dim nonPersistentObjectSpace As NonPersistentObjectSpace = CType(Application.CreateObjectSpace(GetType(ViewFilterContainer)), NonPersistentObjectSpace)
					Dim persistentObjectSpace As IObjectSpace = Application.CreateObjectSpace(GetType(ViewFilterObject))
					nonPersistentObjectSpace.AdditionalObjectSpaces.Add(persistentObjectSpace)
					Dim newViewFilterContainer As ViewFilterContainer = nonPersistentObjectSpace.CreateObject(Of ViewFilterContainer)()
					newViewFilterContainer.ObjectType = oldListView.ObjectTypeInfo.Type
					newViewFilterContainer.Filter = GetFilterObject(persistentObjectSpace, DirectCast(oldListView.Model, IModelListViewAdditionalCriteria).AdditionalCriteria, newViewFilterContainer.ObjectType)
					Dim filterDetailView As DetailView = Application.CreateDetailView(nonPersistentObjectSpace, newViewFilterContainer)
					filterDetailView.Caption = String.Format("Filter for the {0} ListView", oldListView.Caption)
					filterDetailView.ViewEditMode = ViewEditMode.Edit
					e.ActionArguments.ShowViewParameters.CreatedView = filterDetailView
					e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow
					Dim dialogCotnroller As DialogController = Application.CreateController(Of DialogController)()
					AddHandler dialogCotnroller.Accepting, AddressOf dialogCotnroller_Accepting
					AddHandler dialogCotnroller.Cancelling, AddressOf dialogCotnroller_Cancelling
					AddHandler dialogCotnroller.ViewClosed, AddressOf dialogCotnroller_ViewClosed
					e.ActionArguments.ShowViewParameters.Controllers.Add(dialogCotnroller)
				End If
			End If
		End Sub
		Private Sub dialogCotnroller_Cancelling(ByVal sender As Object, ByVal e As EventArgs)
			oldListView.Dispose()
		End Sub
		Private Sub dialogCotnroller_Accepting(ByVal sender As Object, ByVal e As DialogControllerAcceptingEventArgs)
			Dim currentViewFilterContainer As ViewFilterContainer = CType(e.AcceptActionArgs.CurrentObject, ViewFilterContainer)
			Dim targetView As ListView = GetTargetView()
			DirectCast(targetView.Model, IModelListViewAdditionalCriteria).AdditionalCriteria = currentViewFilterContainer.Criteria
			targetView.CollectionSource.Criteria("ByViewFilterObject") = CriteriaEditorHelper.GetCriteriaOperator(currentViewFilterContainer.Criteria, currentViewFilterContainer.ObjectType, targetView.ObjectSpace)
			oldListView = Nothing
			ShowView(targetView, e.ShowViewParameters)
		End Sub
		Private Sub dialogCotnroller_ViewClosed(ByVal sender As Object, ByVal e As EventArgs)
			oldListView = Nothing
		End Sub
		Protected Overridable Sub ShowView(ByVal targetView As View, ByVal showViewParameters As ShowViewParameters)
			Window.SetView(targetView)
		End Sub
		Protected Overridable Function GetTargetView() As ListView
			Return oldListView
		End Function
		Private Function GetFilterObject(ByVal objectSpace As IObjectSpace, ByVal listViewCriteria As String, ByVal objectType As Type) As ViewFilterObject
			Dim criteria As CriteriaOperator = CriteriaOperator.Parse("Criteria = ? and ObjectType = ?", listViewCriteria, objectType)
			Dim filterObject As ViewFilterObject = objectSpace.FindObject(Of ViewFilterObject)(criteria)
			If filterObject Is Nothing Then
				filterObject = objectSpace.CreateObject(Of ViewFilterObject)()
				filterObject.ObjectType = objectType
				filterObject.Criteria = listViewCriteria
				filterObject.FilterName = "Default"
			End If
			Return filterObject
		End Function
		Protected Overrides Sub OnDeactivated()
			MyBase.OnDeactivated()
			If showNavigationItemController IsNot Nothing Then
				RemoveHandler showNavigationItemController.CustomShowNavigationItem, AddressOf ShowNavigationItemController_CustomShowNavigationItem
				showNavigationItemController = Nothing
			End If
			oldListView = Nothing
		End Sub
	End Class
End Namespace
