Imports System.Drawing.Text
Imports System.Runtime.InteropServices

''' <summary>
''' Uses a PrivateFontCollection to load fonts
''' See Sub New() for how to use
''' </summary>
Public Class FontController
    Implements IDisposable
    Private myFonts As PrivateFontCollection
    Private fontBuffer As IntPtr

    ''' <summary>
    ''' Initalizes a font using a font built-in to resources
    ''' Use getFontFamily to get the loaded font family, which can be turned into a font with font size etc
    ''' </summary>
    ''' <param name="fontName">Name of resource in resources</param>
    Sub New (fontName As String)
        myFonts = new PrivateFontCollection()
        ' actual fontName array'
        Dim fontArray = CType(My.Resources.ResourceManager.GetObject(fontName), Byte())
        ' pointer to heap that has fontName copied into' 
        fontBuffer = Marshal.AllocCoTaskMem(fontArray.Length)

        Marshal.Copy(fontArray, 0, fontBuffer, fontArray.Length)
        myFonts.AddMemoryFont(fontBuffer, fontArray.Length)
    End Sub

    ''' <summary>
    ''' Be sure to call to free memory (its only ~1mb but still :D)
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Marshal.FreeCoTaskMem(fontBuffer)
    End Sub

    ''' <summary>
    ''' Gets the font family loaded by this fontcontroller
    ''' </summary>
    ''' <returns></returns>
    Public Function GetFontFamily() As FontFamily
        Return myFonts.Families(0)
    End Function
    
    Public Shared Narrowing Operator CType(ByVal b As FontController) As FontFamily
        Return b.GetFontFamily()
    End Operator
End Class

Public Module CustomFontFamily
    Public NES As New FontController("NES")
    Public SuperMario As New FontController("SuperMario256")
End Module
