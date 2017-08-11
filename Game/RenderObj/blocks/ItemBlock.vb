Public Class ItemBlock
    Inherits Block
    Sub New (location As Point)
        MyBase.New(blockWidth, blockHeight, location, My.Resources.itemblock)
    End Sub 

End Class
