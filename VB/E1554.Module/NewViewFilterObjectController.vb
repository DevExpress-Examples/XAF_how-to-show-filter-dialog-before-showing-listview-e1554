Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.SystemModule

Namespace E1554.Module
	Public Class NewViewFilterObjectController
		Inherits ObjectViewController(Of ListView, ViewFilterObject)

		Public Sub New()
			TargetViewId = "ViewFilterObject_LookupListView"
		End Sub
		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			If TypeOf View.CollectionSource Is PropertyCollectionSource AndAlso CType(View.CollectionSource, PropertyCollectionSource).MasterObjectType Is GetType(ViewFilterContainer) Then
				Dim newObjectViewController As NewObjectViewController = Frame.GetController(Of NewObjectViewController)()
				If newObjectViewController IsNot Nothing Then
					AddHandler newObjectViewController.ObjectCreated, AddressOf ViewController1_ObjectCreated
				End If
			End If
		End Sub
		Private Sub ViewController1_ObjectCreated(ByVal sender As Object, ByVal e As ObjectCreatedEventArgs)
			If TypeOf e.CreatedObject Is ViewFilterObject Then
				Dim newViewFilterObject As ViewFilterObject = CType(e.CreatedObject, ViewFilterObject)
				Dim pcs As PropertyCollectionSource = CType(View.CollectionSource, PropertyCollectionSource)
				newViewFilterObject.ObjectType = CType(pcs.MasterObject, ViewFilterContainer).ObjectType
			End If
		End Sub
		Protected Overrides Sub OnDeactivated()
			MyBase.OnDeactivated()
			If TypeOf View.CollectionSource Is PropertyCollectionSource AndAlso CType(View.CollectionSource, PropertyCollectionSource).MasterObjectType Is GetType(ViewFilterContainer) Then
				Dim newObjectViewController As NewObjectViewController = Frame.GetController(Of NewObjectViewController)()
				If newObjectViewController IsNot Nothing Then
					RemoveHandler newObjectViewController.ObjectCreated, AddressOf ViewController1_ObjectCreated
				End If
			End If
		End Sub
	End Class
End Namespace
