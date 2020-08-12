using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Optimization;

namespace WebClient.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/CssDefault").Include(
                //Global stylesheets
                //"https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900",
                "~/Assets/css/icons/icomoon/styles.css",
                "~/Assets/css/bootstrap.css",
                "~/Assets/css/core.css",
                "~/Assets/css/components.css",
                "~/Assets/css/colors.css"
                ));
            bundles.Add(new CustomScriptBundle("~/bundles/ScriptDefault").Include(
                //Core JS files
                "~/Assets/js/plugins/loaders/pace.min.js",
                "~/Assets/js/core/libraries/jquery.min.js",
                "~/Assets/js/core/libraries/bootstrap.min.js",
                "~/Assets/js/plugins/loaders/blockui.min.js",
                //Theme JS files
                "~/Assets/js/plugins/visualization/d3/d3.min.js",
                "~/Assets/js/plugins/visualization/d3/d3_tooltip.js",
                "~/Assets/js/plugins/forms/styling/switchery.min.js",
                "~/Assets/js/plugins/forms/styling/uniform.min.js",
                "~/Assets/js/plugins/forms/selects/bootstrap_multiselect.js",
                "~/Assets/js/plugins/ui/moment/moment.min.js",
                "~/Assets/js/plugins/pickers/daterangepicker.js",
                "~/Assets/js/js/core/app.js"
                ));
        }
    }

    public class CustomScriptBundle : Bundle
    {
        public CustomScriptBundle(string virtualPath)
            : this(virtualPath, null)
        {
        }

        public CustomScriptBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath, null)
        {
            this.ConcatenationToken = ";" + Environment.NewLine;
            this.Builder = new CustomBundleBuilder();
        }
    }

    public class CustomBundleBuilder : IBundleBuilder
    {
        internal static string ConvertToAppRelativePath(string appPath, string fullName)
        {
            return (string.IsNullOrEmpty(appPath) || !fullName.StartsWith(appPath, StringComparison.OrdinalIgnoreCase) ? fullName : fullName.Replace(appPath, "~/")).Replace('\\', '/');
        }

        public string BuildBundleContent(Bundle bundle, BundleContext context, IEnumerable<BundleFile> files)
        {
            if (files == null)
                return string.Empty;
            if (context == null)
                throw new ArgumentNullException("context");
            if (bundle == null)
                throw new ArgumentNullException("bundle");

            StringBuilder stringBuilder = new StringBuilder();
            foreach (BundleFile bundleFile in files)
            {
                bundleFile.Transforms.Add(new CustomJsMinify());
                stringBuilder.Append(bundleFile.ApplyTransforms());
                stringBuilder.Append(bundle.ConcatenationToken);
            }

            return stringBuilder.ToString();
        }
    }

    public class CustomJsMinify : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            if (includedVirtualPath.EndsWith("min.js", StringComparison.OrdinalIgnoreCase))
            {
                return input;
            }

            Minifier minifier = new Minifier();
            var codeSettings = new CodeSettings();
            codeSettings.EvalTreatment = EvalTreatment.MakeImmediateSafe;
            codeSettings.PreserveImportantComments = false;

            string str = minifier.MinifyJavaScript(input, codeSettings);

            if (minifier.ErrorList.Count > 0)
                return "/* " + string.Concat(minifier.Errors) + " */";

            return str;
        }
    }
}