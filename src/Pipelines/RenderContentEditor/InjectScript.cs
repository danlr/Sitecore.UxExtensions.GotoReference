using System.Web.UI;
using Sitecore.Pipelines;

namespace Sitecore.UxExtensions.GotoReference.Pipelines.RenderContentEditor
{
    public class InjectScript
    {
        private const string JsTag = "    <script type=\"text/javascript\" src=\"{0}\"></script>\r\n";

        public void Process(PipelineArgs args)
        {
            string jsPath = Sitecore.Configuration.Settings.GetSetting("Sitecore.UxExtensions.GotoReference.Script");

            if (string.IsNullOrWhiteSpace(jsPath))
            {
                return;
            }

            Sitecore.Context.Page.Page.Header.Controls.Add(new LiteralControl(string.Format(JsTag, jsPath)));
        }
    }
}