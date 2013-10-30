'�ָ���ͣ������
Option Explicit On 

Imports System
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Public Class ImplResumeTask
    Implements IFlowTools


    '����ȫ�����ݿ����Ӷ���
    Private conn As SqlConnection

    '��������
    Private ts As SqlTransaction

    Private WfProjectTaskAttendee As WfProjectTaskAttendee

    Private WfProjectTask As WfProjectTask

    Private WfProjectTaskTransfer As WfProjectTaskTransfer
    Private WfProjectTimingTask As WfProjectTimingTask
    Private WfProjectTrack As WfProjectTrack

    Public Sub New(ByVal DbConnection As SqlConnection, ByRef trans As SqlTransaction)
        MyBase.New()
        conn = DbConnection


        '�����ݿ�����
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If


        '�����ⲿ����
        ts = trans


        WfProjectTaskAttendee = New WfProjectTaskAttendee(conn, ts)

        WfProjectTask = New WfProjectTask(conn, ts)

        WfProjectTaskTransfer = New WfProjectTaskTransfer(conn, ts)
        WfProjectTimingTask = New WfProjectTimingTask(conn, ts)
        WfProjectTrack = New WfProjectTrack(conn, ts)


    End Sub


    Public Function UseFlowTools(ByVal workFlowID As String, ByVal projectID As String, ByVal taskID As String, ByVal finishedFlag As String, ByVal userID As String) Implements IFlowTools.UseFlowTools
        Dim strSql As String
        Dim i As Integer

        '����������IDΪ13��17��������е�����״̬ΪC ����ΪP
        strSql = "{project_code='" & projectID & "' and workflow_id not in ('13','17','22') and task_status='C'}"
        Dim dsTemp As DataSet = WfProjectTaskAttendee.GetWfProjectTaskAttendeeInfo(strSql)
        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dsTemp.Tables(0).Rows(i).Item("task_status") = "P"
        Next

        WfProjectTaskAttendee.UpdateWfProjectTaskAttendee(dsTemp)

        'ɾ���ݻ�������
        delCancelProject(workFlowID, projectID, taskID, finishedFlag, userID)

    End Function

    'ɾ���ݻ�������: workflow_id �ֱ�Ϊ 13��һ����ݻ����̣� �� 
    '17�������ݻ����̣��ڼ�¼��������������ύ���ݻ����̣���һ���ݻ����������������������ϻᡱ�Ĺ��ܣ� 
    Public Function delCancelProject(ByVal workFlowID As String, ByVal projectID As String, ByVal taskID As String, ByVal finishedFlag As String, ByVal userID As String)
        Dim strSql As String
        Dim i As Integer
        strSql = "{project_code='" & projectID & "' and workflow_id in ('13','17','22')}"
        Dim dsTemp As DataSet


        'ɾ�������˱�
        dsTemp = WfProjectTaskAttendee.GetWfProjectTaskAttendeeInfo(strSql)

        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dsTemp.Tables(0).Rows(i).Delete()
        Next

        WfProjectTaskAttendee.UpdateWfProjectTaskAttendee(dsTemp)

        'ɾ��ת�Ʊ�
        dsTemp = WfProjectTaskTransfer.GetWfProjectTaskTransferInfo(strSql)

        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dsTemp.Tables(0).Rows(i).Delete()
        Next

        WfProjectTaskTransfer.UpdateWfProjectTaskTransfer(dsTemp)

        'ɾ����ʱ�����
        dsTemp = WfProjectTimingTask.GetWfProjectTimingTaskInfo(strSql)

        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dsTemp.Tables(0).Rows(i).Delete()
        Next

        WfProjectTimingTask.UpdateWfProjectTimingTask(dsTemp)

        'ɾ�����ٱ�
        dsTemp = WfProjectTrack.GetWfProjectTrackInfo(strSql)

        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dsTemp.Tables(0).Rows(i).Delete()
        Next

        WfProjectTrack.UpdateWfProjectTrack(dsTemp)


        'ɾ�������
        dsTemp = WfProjectTask.GetWfProjectTaskInfo(strSql)

        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dsTemp.Tables(0).Rows(i).Delete()
        Next

        WfProjectTask.UpdateWfProjectTask(dsTemp)

    End Function

End Class