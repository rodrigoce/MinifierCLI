public class Config
{
    // js
    public string JsRootSource { get; set; }
    public string JsRootTarget { get; set; }
    public bool JsInsertMinInfix { get; set; }

    // css
    public string CssRootSource { get; set; }
    public string CssRootTarget { get; set; }
    public bool CssInsertMinInfix { get; set; }

    // general
    public bool InsertTimeStampInAppSettings { get; set; }
}