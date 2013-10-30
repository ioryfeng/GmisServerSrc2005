Imports System.Runtime.InteropServices


Namespace WebService_CGMIS


Public Class SenseLock
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> Public Structure typeSS3
        Public ReturnFlag As Short 'function result
        Public FunctionCode As Short 'function address
        <VBFixedArray(2), MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Public Password() As Short   'password
        Public OutWords As Short 'number of words send to sense3
        <VBFixedArray(3), MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public OutBuff() As Short 'out buffer
        Public InWords As Short 'number of words received from sense3
        <VBFixedArray(3), MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Public InBuff() As Short 'in buffer
        Public Setting As Short 'ignored
        <VBFixedArray(31), MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)> Public Reserved() As Short 'reserved for system, can't change

        Public Sub Initialize()
            ReDim Password(2)
            ReDim OutBuff(3)
            ReDim InBuff(3)
            ReDim Reserved(31)
        End Sub
    End Structure

    Public Structure Sense3AutoData
        <VBFixedArray(41)> Public ad() As Short

        '必须调用“Initialize”来初始化此结构的实例。 单击以获得更多信息：'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1026"'
        Public Sub Initialize()
            ReDim ad(41)
        End Sub
    End Structure

    ' FunctionCode:
    Private Const Sense3Init As Int16 = -1
    Private Const Sense3Exit As Int16 = 1024

    Private Const Read_Data As Int16 = 4 ' It return the data you want to read
    Private Const Read_Addr As Int16 = 7 ' The address of memory you want to read
    Private Const Write_Addr As Int16 = 0 ' The address of memory you want to write
    Private Const Write_Data As Int16 = 1 ' The data you want to write, it return the old data
    Private Const Encrypt_Highword_Data As Int16 = 0 ' The high word of data you want to encrypt
    Private Const Encrypt_Lowword_Data As Int16 = 2 ' The low word of data you want to encrypt
    Private Const Decrypt_Lowword_Data As Int16 = 1 ' The low word of data you want to decrypt
    Private Const Decrypt_Highword_Data As Int16 = 3 ' The high word of data you want to decrypt
    Private Const EncryptBak_Lowword_Data As Int16 = 1 ' The low word of data you want to encrypt
    Private Const EncryptBak_Highword_Data As Int16 = 2 ' The high word of data you want to encrypt
    Private Const DecryptBak_Lowword_Data As Int16 = 1 ' The low word of data you want to decrypt
    Private Const DecryptBak_Highword_Data As Int16 = 2 ' The high word of data you want to decrypt

    Private Const InAddr As Int16 = 17
    Private Const WriMemAddr As Int16 = 1
    Private Const DecAddr As Int16 = 3
    Private Const ReadMemAddr As Int16 = 8
    Private Const EncAddr As Int16 = 10

    Private ss3 As typeSS3

    Private Declare Sub SENSE3 Lib "Sense3.dll" (ByRef ss3 As typeSS3)

    Public Shared Sub Close()
        Dim s3 As typeSS3 = New typeSS3

        s3.FunctionCode = Sense3Exit
        Call SENSE3(s3)
    End Sub

    Public Function Open(ByVal p1 As Int16, ByVal p2 As Int16, ByVal p3 As Int16) As Int16
        Try
            ss3.Initialize()

            ss3.Password(0) = p1
            ss3.Password(1) = p2
            ss3.Password(2) = p3
            ss3.FunctionCode = Sense3Init

            Call SENSE3(ss3)
            Return ss3.ReturnFlag
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class

End Namespace
