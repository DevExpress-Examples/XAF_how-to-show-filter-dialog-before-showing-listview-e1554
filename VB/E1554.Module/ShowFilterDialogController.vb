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
        Private selectedChoiceActionItem As ChoiceActionItem
        Private showFromController As Boolean = False

        Protected Overrides Sub OnActivated()
            MyBase.OnActivated()
            showNavigationItemController = Frame.GetController(Of ShowNavigationItemController)()
            If showNavigationItemController IsNot Nothing Then
                AddHandler showNavigationItemController.CustomShowNavigationItem, AddressOf ShowNavigationItemController_CustomShowNavigationItem
            End If
        End Sub
        Private Sub ShowNavigationItemAction_Executed(ByVal sender As Object, ByVal e As ActionBaseEventArgs)
            RemoveHandler showNavigationItemController.ShowNavigationItemAction.Executed, AddressOf ShowNavigationItemAction_Executed
            If showFromController Then
                e.ShowViewParameters.CreatedView = oldListView
            End If
        End Sub
        Private Sub ShowNavigationItemController_CustomShowNavigationItem(ByVal sender As Object, ByVal e As CustomShowNavigationItemEventArgs)
            If showFromController Then
                Return
            End If
            Dim shortcut As ViewShortcut = TryCast(e.ActionArguments.SelectedChoiceActionItem.Data, ViewShortcut)
            If shortcut IsNot Nothing Then
                oldListView = TryCast(Application.ProcessShortcut(shortcut), ListView)
            End If

            If oldListView IsNot Nothing Then
                e.Handled = True
                selectedChoiceActionItem = e.ActionArguments.SelectedChoiceActionItem
                Dim nonPersistentObjectSpace As NonPersistentObjectSpace = CType(Application.CreateObjectSpace(GetType(ViewFilterContainer)), NonPersistentObjectSpace)
                Dim persistentObjectSpace As IObjectSpace = Application.CreateObjectSpace(GetType(ViewFilterObject))
                nonPersistentObjectSpace.AdditionalObjectSpaces.Add(persistentObjectSpace)
                Dim newViewFilterContainer As ViewFilterContainer = nonPersistentObjectSpace.CreateObject(Of ViewFilterContainer)()
                newViewFilterContainer.ObjectType = oldListView.ObjectTypeInfo.Type
                newViewFilterContainer.Filter = GetFilterObject(persistentObjectSpace, CType(oldListView.Model, IModelListViewAdditionalCriteria).AdditionalCriteria, newViewFilterContainer.ObjectType)
                Dim filterDetailView As DetailView = Application.CreateDetailView(nonPersistentObjectSpace, newViewFilterContainer)
                filterDetailView.Caption = String.Format("Filter for the {0} ListView", oldListView.Caption)
                filterDetailView.ViewEditMode = ViewEditMode.Edit
                e.ActionArguments.ShowViewParameters.CreatedView = filterDetailView
                e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow
                Dim dialogCotnroller As DialogController = Application.CreateController(Of DialogController)()
                AddHandler dialogCotnroller.Accepting, AddressOf dialogCotnroller_Accepting
                AddHandler dialogCotnroller.ViewClosed, AddressOf dialogCotnroller_ViewClosed
                e.ActionArguments.ShowViewParameters.Controllers.Add(dialogCotnroller)
            End If
        End Sub

        Private Sub dialogCotnroller_Accepting(ByVal sender As Object, ByVal e As DialogControllerAcceptingEventArgs)
            Dim currentViewFilterContainer As ViewFilterContainer = CType(e.AcceptActionArgs.CurrentObject, ViewFilterContainer)
            Dim targetView As ListView = GetTargetView()
            CType(targetView.Model, IModelListViewAdditionalCriteria).AdditionalCriteria = currentViewFilterContainer.Criteria
            targetView.CollectionSource.Criteria("ByViewFilterObject") = CriteriaEditorHelper.GetCriteriaOperator(currentViewFilterContainer.Criteria, currentViewFilterContainer.ObjectType, targetView.ObjectSpace)
            AddHandler showNavigationItemController.ShowNavigationItemAction.Executed, AddressOf ShowNavigationItemAction_Executed
            Try
                showFromController = True
                showNavigationItemController.ShowNavigationItemAction.DoExecute(selectedChoiceActionItem)
            Finally
                showFromController = False
                oldListView = Nothing
                selectedChoiceActionItem = Nothing
            End Try
        End Sub

        Protected Overridable Function GetTargetView() As ListView
            Return oldListView
        End Function

        Private Sub dialogCotnroller_ViewClosed(ByVal sender As Object, ByVal e As EventArgs)
            oldListView = Nothing
            selectedChoiceActionItem = Nothing
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
                RemoveHandler showNavigationItemController.CustomShowNavigationItem, AddressOf ShowNavigationItemController_CustomShowNavigationItem
                showNavigationItemController = Nothing
            End If
            oldListView = Nothing
            selectedChoiceActionItem = Nothing
        End Sub
    End Class
End Namespace
