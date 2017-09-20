
Public Class Flag
    Inherits HitboxItem
   
    Friend Const FlagWidth = 8
    Friend Const FlagHeight = StandardHeight * 9
    
    Private top As FlagTop

    Public Sub New(location As Point, mapScene As MapScene, Optional center As Boolean=True)
        MyBase.New(FlagWidth, FlagHeight, location, Resize(My.Resources.flagpole_stem, FlagWidth, FlagHeight), mapScene)
        if center
            Dim x As Integer = location.X / StandardWidth
            Me.Location =New Point(x * StandardWidth + (StandardWidth-FlagWidth) / 2, Location.Y)
        End If
        top = New FlagTop(New Point(Me.X - FlagWidth, Me.Y+FlagHeight), mapScene)
        mapScene.AddHitbox(top)
    End Sub


    

End Class
' friend = same compilation unit
' protected = subclass
Public Class FlagTop
    Inherits HitboxItem
    Public Sub New(location As Point, mapScene As MapScene)
        MyBase.New(Flag.FlagWidth * 3, Flag.FlagWidth * 3, location, Resize(My.Resources.flagpole_head, Flag.FlagWidth * 3, Flag.FlagWidth * 3), mapScene)
        Me.CollisionWidth = Flag.FlagWidth
    End Sub

End Class