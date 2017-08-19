Imports System.Drawing.Text
Imports System.Runtime.InteropServices

Public Class FontController
    Implements IDisposable
    Private myFonts As PrivateFontCollection
    Private fontBuffer As IntPtr


    Sub New (fontName As String)
        myFonts = new PrivateFontCollection()
        ' actual fontName array'
        Dim fontArray = CType(My.Resources.ResourceManager.GetObject(fontName), Byte())
        ' pointer to heap that has fontName copied into' 
        fontBuffer = Marshal.AllocCoTaskMem(fontArray.Length)

        Marshal.Copy(fontArray, 0, fontBuffer, fontArray.Length)
        myFonts.AddMemoryFont(fontBuffer, fontArray.Length)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Marshal.FreeCoTaskMem(fontBuffer)
    End Sub
    Public Function GetFontFamily() As FontFamily
        Return myFonts.Families(0)
    End Function
        
End Class

Public Module CustomFontFamily
    Public NES As New FontController("NES")
    Public SuperMario As New FontController("SuperMario256")
End Module
