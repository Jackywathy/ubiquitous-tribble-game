Public MustInherit Class Block
    Inherits RenderObject
    Friend Const blockWidth = 32
    Friend Const blockHeight = 32

    Public Overrides Property RenderImage As Image
    Public MustOverride Property spriteSet As SpriteSet

    Public Sub New(width As Integer, height As Integer, location As Point, scene As Scene)
        MyBase.New(width, height, location, scene)
    End Sub

    
    Public Overrides Sub CollisionTop(sender As Entity, scene As Scene)
        MyBase.CollisionTop(sender, scene)
    End Sub
    Public Overrides Sub CollisionBottom(sender As Entity, scene As Scene)
        MyBase.CollisionBottom(sender, scene)
    End Sub
    Public Overrides Sub CollisionLeft(sender As Entity, scene As Scene)

        If sender.GetType = GetType(EntFireball) Then
            sender.Destroy(scene)
        Else
            MyBase.CollisionLeft(sender, scene)
        End If
    End Sub
    Public Overrides Sub CollisionRight(sender As Entity, scene As Scene)

        If sender.GetType = GetType(EntFireball) Then
            sender.Destroy(scene)
        Else
            MyBase.CollisionRight(sender, scene)
        End If
    End Sub

End Class
