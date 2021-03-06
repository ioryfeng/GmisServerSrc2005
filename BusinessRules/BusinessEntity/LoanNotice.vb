Option Explicit On 

Imports System
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Public Class LoanNotice
    Public Const Table_LoanNotice As String = "loan_notice"

    '定义全局数据库连接对象
    Private conn As SqlConnection

    '定义全局数据库连接适配器
    Private dsCommand_LoanNotice As SqlDataAdapter

    '定义查询命令
    Private GetLoanNoticeInfoCommand As SqlCommand

    '定义事务
    Private ts As SqlTransaction

    '构造函数
    Public Sub New(ByVal DbConnection As SqlConnection, ByRef trans As SqlTransaction)
        MyBase.New()
        conn = DbConnection


        '实例化适配器
        dsCommand_LoanNotice = New SqlDataAdapter()

        '打开数据库连接
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        '引用外部事务
        ts = trans

        '填充适配器
        GetLoanNoticeInfo("null")
    End Sub

    '获取放款通知书信息
    Public Function GetLoanNoticeInfo(ByVal strSQL_Condition_LoanNotice As String) As DataSet

        Dim tempDs As New DataSet()

        If GetLoanNoticeInfoCommand Is Nothing Then

            GetLoanNoticeInfoCommand = New SqlCommand("GetLoanNoticeInfo", conn)
            GetLoanNoticeInfoCommand.CommandType = CommandType.StoredProcedure
            GetLoanNoticeInfoCommand.Parameters.Add(New SqlParameter("@Condition", SqlDbType.NVarChar))

        End If

        With dsCommand_LoanNotice
            .SelectCommand = GetLoanNoticeInfoCommand
            .SelectCommand.Transaction = ts
            GetLoanNoticeInfoCommand.Parameters("@Condition").Value = strSQL_Condition_LoanNotice
            .Fill(tempDs, Table_LoanNotice)
        End With

        Return tempDs

    End Function

    '更新放款通知书信息
    Public Function UpdateLoanNotice(ByVal LoanNoticeSet As DataSet)

        If LoanNoticeSet Is Nothing Then
            Exit Function
        End If


        '如果记录集未发生任何变化，则退出过程
        If LoanNoticeSet.HasChanges = False Then
            Exit Function
        End If

        Dim bd As SqlCommandBuilder = New SqlCommandBuilder(dsCommand_LoanNotice)

        With dsCommand_LoanNotice
            .InsertCommand = bd.GetInsertCommand
            .UpdateCommand = bd.GetUpdateCommand
            .DeleteCommand = bd.GetDeleteCommand

            .InsertCommand.Transaction = ts
            .UpdateCommand.Transaction = ts
            .DeleteCommand.Transaction = ts

            .Update(LoanNoticeSet, Table_LoanNotice)

        End With

        LoanNoticeSet.AcceptChanges()

    End Function
End Class
