namespace Oracle.Forms.MaterialSkin
{
    internal interface IMaterialControl
    {
        int Depth { get; set; }

        MouseState MouseState { get; set; }

        MaterialSkinManager SkinManager { get; }
    }

    public enum MouseState
    {
        Hover,

        Down,

        Out
    }
}