Imports System.Xml
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.Services.Protocols


Namespace WebService_CGMIS


Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " 组件设计器生成的代码 "

    Public Sub New()
        MyBase.New()

        '该调用是组件设计器所必需的。
        InitializeComponent()

        '在 InitializeComponent() 调用之后添加任何初始化

    End Sub

    '组件设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意：以下过程是组件设计器所必需的
    '可以使用组件设计器修改此过程。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

    Private Function GetLogin(ByRef userName As String, ByRef password As String, ByRef system As String) As Boolean
        Dim context As HttpContext = Me.Context
        Dim HttpStream As System.IO.Stream = context.Request.InputStream

        'Save the current position of stream.
        Dim posStream As Long = HttpStream.Position

        'If the request contains an HTTP_SOAPACTION header, look at this message.
        If context.Request.ServerVariables("HTTP_SOAPACTION") Is Nothing Then
            Return False
        End If

        'Load the body of the HTTP message into an XML document.
        Dim dom As System.Xml.XmlDocument = New System.Xml.XmlDocument()

        Try
            dom.Load(HttpStream)

            'Reset the stream position.
            HttpStream.Position = posStream

            Dim nodeList As System.Xml.XmlNodeList

            'Bind to the Authentication header.
            nodeList = dom.GetElementsByTagName("System")
            If nodeList.Count > 0 Then
                system = nodeList.Item(0).InnerText
            End If

            nodeList = dom.GetElementsByTagName("UserName")
            If nodeList.Count > 0 Then
                userName = nodeList.Item(0).InnerText
            End If

            nodeList = dom.GetElementsByTagName("Password")
            If nodeList.Count > 0 Then
                password = nodeList.Item(0).InnerText
            End If

            Return True
        Catch e As Exception
            'Reset the position of stream.
            HttpStream.Position = posStream

            'Throw a SOAP exception.
            Dim name As XmlQualifiedName = New XmlQualifiedName("Load")
            Dim se As SoapException = New SoapException("Unable to read SOAP request", name, e)
            Throw se
        End Try

        Return False
    End Function

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' 在应用程序启动时激发
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' 在会话启动时激发
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 在每个请求开始时激发
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ''尝试对使用进行身份验证时激发()
        'Dim lock As SenseLock = New SenseLock()

        'Try
        '    If lock.Open(6891, -1926, -4768) <> 0 Then
        '        Me.Context.AddError(New System.Security.SecurityException())
        '    End If
        'Catch ex As Exception
        '    Me.Context.AddError(ex)
        'Finally
        '    If Not lock Is Nothing Then
        '        lock.Close()
        '        lock = Nothing
        '    End If
        'End Try

        'Dim connectionString As String = system.Configuration.ConfigurationSettings.AppSettings("DBConnection")

        'Try
        '    Dim permissionObject As SWSystem.Security.Permissions.Permission = New SWSystem.Security.Permissions.Permission(connectionString)

        '    Dim system As String
        '    Dim userName As String
        '    Dim password As String

        '    If Not GetLogin(userName, password, system) Then
        '        Throw New SoapHeaderException("没有指定必须的 SoapHeader: Authentication。", SoapException.MustUnderstandFaultCode)
        '    End If

        '    If Not permissionObject.VerifyLogin(userName, password, system) Then
        '        Throw New SoapException("InvalidPermission", SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri)
        '    End If
        'Catch
        '    Throw
        'End Try
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' 在发生错误时激发
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' 在会话结束时激发
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' 在应用程序结束时激发
    End Sub

End Class

End Namespace
