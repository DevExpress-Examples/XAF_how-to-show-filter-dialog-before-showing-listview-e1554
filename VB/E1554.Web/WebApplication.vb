Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Web
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Xpo

Namespace E1554.Web
	' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/DevExpressExpressAppWebWebApplicationMembersTopicAll
	Partial Public Class E1554AspNetApplication
		Inherits WebApplication
		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule
		Private module3 As E1554.Module.E1554Module
		Private module4 As E1554.Module.Web.E1554AspNetModule
		Private securityModule1 As DevExpress.ExpressApp.Security.SecurityModule
		Private securityStrategyComplex1 As DevExpress.ExpressApp.Security.SecurityStrategyComplex
		Private authenticationActiveDirectory1 As DevExpress.ExpressApp.Security.AuthenticationActiveDirectory
		Private businessClassLibraryCustomizationModule1 As DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule
		Private validationModule As DevExpress.ExpressApp.Validation.ValidationModule

		Public Sub New()
			InitializeComponent()
		End Sub
		Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
			args.ObjectSpaceProviders.Add(New XPObjectSpaceProvider(args.ConnectionString, args.Connection, True))
			args.ObjectSpaceProviders.Add(New NonPersistentObjectSpaceProvider(TypesInfo, Nothing))
		End Sub
		Private Sub E1554AspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
#If EASYTEST Then
			e.Updater.Update()
			e.Handled = True
#Else
			If System.Diagnostics.Debugger.IsAttached Then
				e.Updater.Update()
				e.Handled = True
			Else
				Dim message As String = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application." & Constants.vbCrLf & "This error occurred  because the automatic database update was disabled when the application was started without debugging." & Constants.vbCrLf & "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " & "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " & "or manually create a database using the 'DBUpdater' tool." & Constants.vbCrLf & "Anyway, refer to the following help topics for more detailed information:" & Constants.vbCrLf & "'Update Application and Database Versions' at http://help.devexpress.com/#Xaf/CustomDocument2795" & Constants.vbCrLf & "'Database Security References' at http://help.devexpress.com/#Xaf/CustomDocument3237" & Constants.vbCrLf & "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/"

				If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
					message &= Constants.vbCrLf & Constants.vbCrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
				End If
				Throw New InvalidOperationException(message)
			End If
#End If
		End Sub
		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
			Me.module3 = New E1554.Module.E1554Module()
			Me.module4 = New E1554.Module.Web.E1554AspNetModule()
			Me.securityModule1 = New DevExpress.ExpressApp.Security.SecurityModule()
			Me.securityStrategyComplex1 = New DevExpress.ExpressApp.Security.SecurityStrategyComplex()
			Me.authenticationActiveDirectory1 = New DevExpress.ExpressApp.Security.AuthenticationActiveDirectory()
			Me.validationModule = New DevExpress.ExpressApp.Validation.ValidationModule()
			Me.businessClassLibraryCustomizationModule1 = New DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule()
			CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' securityStrategyComplex1
			' 
			Me.securityStrategyComplex1.Authentication = Me.authenticationActiveDirectory1
			Me.securityStrategyComplex1.RoleType = GetType(DevExpress.ExpressApp.Security.Strategy.SecuritySystemRole)
			Me.securityStrategyComplex1.UserType = GetType(DevExpress.ExpressApp.Security.Strategy.SecuritySystemUser)
			' 
			' authenticationActiveDirectory1
			' 
			Me.authenticationActiveDirectory1.CreateUserAutomatically = True
			Me.authenticationActiveDirectory1.LogonParametersType = Nothing
			' 
			' validationModule
			' 
			Me.validationModule.AllowValidationDetailsAccess = True
			Me.validationModule.IgnoreWarningAndInformationRules = False
			' 
			' E1554AspNetApplication
			' 
			Me.ApplicationName = "E1554"
			Me.CollectionsEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit
			Me.Modules.Add(Me.module1)
			Me.Modules.Add(Me.module2)
			Me.Modules.Add(Me.businessClassLibraryCustomizationModule1)
			Me.Modules.Add(Me.module3)
			Me.Modules.Add(Me.module4)
			Me.Modules.Add(Me.securityModule1)
			Me.Modules.Add(Me.validationModule)
			Me.Security = Me.securityStrategyComplex1
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.E1554AspNetApplication_DatabaseVersionMismatch);
			CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub
	End Class
End Namespace
