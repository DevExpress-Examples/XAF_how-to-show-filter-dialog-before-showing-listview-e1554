Imports Microsoft.VisualBasic
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
				AddHandler showNavigationItemController.ShowNavigationItemAction.Execute, AddressOf ShowNavigationItemAction_Execute
				AddHandler showNavigationItemController.CustomUpdateSelectedItem, AddressOf showNavigationItemController_CustomUpdateSelectedItem
			End If
		End Sub

		Private Sub ShowNavigationItemAction_Execute(ByVal sender As Object, ByVal e As SingleChoiceActionExecuteEventArgs)
			If TypeOf e.ShowViewParameters.CreatedView Is ListView Then
				oldListView = CType(e.ShowViewParameters.CreatedView, ListView)
				Dim nonPersistentObjectSpace As NonPersistentObjectSpace = CType(Application.CreateObjectSpace(GetType(ViewFilterContainer)), NonPersistentObjectSpace)
				Dim persistentObjectSpace As IObjectSpace = Application.CreateObjectSpace(GetType(ViewFilterObject))
				nonPersistentObjectSpace.AdditionalObjectSpaces.Add(persistentObjectSpace)
				Dim newViewFilterContainer As ViewFilterContainer = nonPersistentObjectSpace.CreateObject(Of ViewFilterContainer)()
				newViewFilterContainer.ObjectType = oldListView.ObjectTypeInfo.Type
				newViewFilterContainer.Filter = GetFilterObject(persistentObjectSpace, (CType(oldListView.Model, IModelListViewAdditionalCriteria)).AdditionalCriteria, newViewFilterContainer.ObjectType)
				Dim filterDetailView As DetailView = Application.CreateDetailView(nonPersistentObjectSpace, newViewFilterContainer)
				filterDetailView.Caption = String.Format("Filter for the {0} ListView", oldListView.Caption)
				filterDetailView.ViewEditMode = ViewEditMode.Edit
				e.ShowViewParameters.CreatedView = filterDetailView
				e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow
				Dim dialogCotnroller As DialogController = Application.CreateController(Of DialogController)()
				AddHandler dialogCotnroller.Accepting, AddressOf dialogCotnroller_Accepting
				AddHandler dialogCotnroller.ViewClosed, AddressOf dialogCotnroller_ViewClosed
				e.ShowViewParameters.Controllers.Add(dialogCotnroller)
			End If
		End Sub

		Private Sub dialogCotnroller_Accepting(ByVal sender As Object, ByVal e As DialogControllerAcceptingEventArgs)
			Dim currentViewFilterContainer As ViewFilterContainer = CType(e.AcceptActionArgs.CurrentObject, ViewFilterContainer)
			Dim targetView As ListView = GetTargetView()
			CType(targetView.Model, IModelListViewAdditionalCriteria).AdditionalCriteria = currentViewFilterContainer.Criteria
			targetView.CollectionSource.Criteria("ByViewFilterObject") = CriteriaEditorHelper.GetCriteriaOperator(currentViewFilterContainer.Criteria, currentViewFilterContainer.ObjectType, targetView.ObjectSpace)
			Dim parameters As New ShowViewParameters(targetView)
			parameters.TargetWindow = TargetWindow.Current
			parameters.Context = TemplateContext.View
			Dim source As New ShowViewSource(Frame, showNavigationItemController.ShowNavigationItemAction)
			Application.ShowViewStrategy.ShowView(parameters, source)
			oldListView = Nothing
		End Sub

		Protected Overridable Function GetTargetView() As ListView
			Return oldListView
		End Function

		Private Sub dialogCotnroller_ViewClosed(ByVal sender As Object, ByVal e As EventArgs)
			oldListView = Nothing
		End Sub

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

		Private Sub showNavigationItemController_CustomUpdateSelectedItem(ByVal sender As Object, ByVal e As CustomUpdateSelectedItemEventArgs)
			If oldListView IsNot Nothing Then
				e.ProposedSelectedItem = showNavigationItemController.FindNavigationItemByViewShortcut(oldListView.CreateShortcut())
				e.Handled = True
			End If
		End Sub

		Protected Overrides Sub OnDeactivated()
			MyBase.OnDeactivated()
			If showNavigationItemController IsNot Nothing Then
				RemoveHandler showNavigationItemController.ShowNavigationItemAction.Execute, AddressOf ShowNavigationItemAction_Execute
				RemoveHandler showNavigationItemController.CustomUpdateSelectedItem, AddressOf showNavigationItemController_CustomUpdateSelectedItem
				showNavigationItemController = Nothing
			End If
			oldListView = Nothing
		End Sub
	End Class
End Namespace
